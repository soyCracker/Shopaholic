using Shopaholic.Entity.Models;
using Shopaholic.Service.Interfaces;
using Shopaholic.Web.Model.Requests;
using System.Text;
using Shopaholic.Service.Common.Constants;
using System.Text.Json;
using Shopaholic.Web.Model.Responses;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using Shopaholic.Web.Common.Factory;
using Shopaholic.Service.Model.Moels;

namespace Shopaholic.Service.Services
{
    public class LinePayPurchaseService : IPurchaseService
    {
        private readonly ShopaholicContext dbContext;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly string api ;
        private readonly string channelSecret;
        private readonly string channelId;
        private readonly string linepayUrl;
        private readonly string orderIdCreateApi;
        private readonly string linePayConfirmApi;

        public LinePayPurchaseService(ShopaholicContext dbContext, IHttpClientFactory httpClientFactory, IConfiguration configuration, EnvirFactory envirFactory)
        {
            this.dbContext = dbContext;
            this.httpClientFactory = httpClientFactory;
            api = envirFactory.GetEnvir().GetLinePayApiUrl();
            channelSecret = envirFactory.GetEnvir().GetLinePayChannelSecret();
            channelId = envirFactory.GetEnvir().GetLinePayChannelId();
            linepayUrl = envirFactory.GetEnvir().GetLinePayBaseUrl();
            orderIdCreateApi = envirFactory.GetEnvir().GetOrderIdCreateApi();
            linePayConfirmApi = envirFactory.GetEnvir().GetLinePayConfirmApi();
        }

        public async Task<string> CreateOrder(PurchaseOrderCreateReq req)
        {
            using(dbContext)
            {
                req.OrderTypeCode = OrderTypeCode.LINE_PAY;
                req.UserId = dbContext.CustomerAccounts.SingleOrDefault(x => x.Email==req.Email).AccountId;
                var carts = dbContext.ShoppingCarts.Where(x => x.AccountId==req.UserId).ToList();
                List<PurchaseProductModel> ProductList = new List<PurchaseProductModel>();
                foreach (var cart in carts)
                {
                    var product = dbContext.Products.SingleOrDefault(p => p.Id==cart.ProductId);
                    ProductList.Add(new PurchaseProductModel
                    {
                        ProductId = cart.ProductId,
                        Quantity = cart.Quantity,
                        CurrentPrice = product.Price
                    });
                }
                req.ProductList = ProductList;
                string orderId = await OrderCreatePost(req);
                if(!string.IsNullOrEmpty(orderId))
                {
                    dbContext.ShoppingCarts.RemoveRange(carts);
                    dbContext.SaveChanges();
                }             
                return orderId;
            }
        }      

        public async Task<T> Pay<T>(PurchasePayReq req) where T : class
        {
            using (dbContext)
            {
                var order = dbContext.OrderHeaders.SingleOrDefault(x => x.OrderId==req.OrderId);
                if(order.StateCode==OrderStateCode.CREATE && order.StateCode==OrderFailCode.COMMON)
                {
                    string linepayOrderId = Guid.NewGuid().ToString();
                    var reqBody = JsonSerializer.Serialize(GetLinePayReqBody(order.OrderId, linepayOrderId, req.ConfirmUrl + linePayConfirmApi));
                    var linepayRes = await LinePayPost(reqBody);

                    if(linepayRes.returnCode == "0000")
                    {
                        string transactionId = linepayRes.info.transactionId.ToString();
                        CreateLinePayOrder(req.OrderId, linepayOrderId, transactionId);
                        dbContext.SaveChanges();
                    }         

                    LinePayPurchaseRes purchasePayRes = new LinePayPurchaseRes
                    {
                        IsSuccess = (linepayRes.returnCode == "0000"),
                        Msg = linepayRes.returnMessage,
                        CallbackUrl = (linepayRes.returnCode == "0000")? linepayRes.info.paymentUrl.web :"",
                    };
                    return purchasePayRes as T;
                }
                return new LinePayPurchaseRes
                {
                    IsSuccess = false
                } as T;
            }                           
        }

        public async Task<LinePayRes> LinePayPost(string reqBody)
        {
            using (var httpClient = httpClientFactory.CreateClient())
            {               
                string nonce = Guid.NewGuid().ToString();              
                string Signature = HashLinePayRequest(channelSecret, api, reqBody, nonce, channelSecret);
                httpClient.DefaultRequestHeaders.Add("X-LINE-ChannelId", channelId);
                httpClient.DefaultRequestHeaders.Add("X-LINE-Authorization-Nonce", nonce);
                httpClient.DefaultRequestHeaders.Add("X-LINE-Authorization", Signature);
                var content = new StringContent(reqBody, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(linepayUrl + api, content);
                var result = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<LinePayRes>(result);
            }
        }

        private LinePayReq GetLinePayReqBody(string orderId, string linepayOrderId, string confirmUrl)
        {
            List<OrderDetail> orderDetails = dbContext.OrderDetails.Where(x => x.OrderId == orderId).ToList();
            int totalPrice = 0;
            List<Web.Model.Requests.Product> lineProducts = new List<Web.Model.Requests.Product>();
            foreach (var detail in orderDetails)
            {
                var product = dbContext.Products.SingleOrDefault(x => x.Id==detail.ProductId);
                totalPrice+=detail.Quantity*detail.CurrentPrice;

                lineProducts.Add(new Web.Model.Requests.Product
                {
                    id = product.Id.ToString(),
                    name = product.Name,
                    imageUrl = product.Image,
                    quantity = detail.Quantity,
                    price = detail.CurrentPrice
                });
            }

            return new LinePayReq
            {
                amount = totalPrice,
                currency = "TWD",
                orderId = linepayOrderId,
                redirectUrls = new Redirecturls()
                {
                    confirmUrl = confirmUrl
                },
                packages = new List<Package>()
                    {
                        new Package()
                        {
                            id = "package-1",
                            name = "測試包裹",
                            amount = totalPrice,
                            products = lineProducts
                        }
                    }
            };

        }

        private string HashLinePayRequest(string channelSecret, string apiUrl, string body, string nonce, string key)
        {
            var request = channelSecret + apiUrl + body + nonce;
            key = key ?? "";
            var encoding = new UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(key);
            byte[] messageBytes = encoding.GetBytes(request);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }

        private void CreateLinePayOrder(string orderId, string linepayOrderId, string transactionId)
        {
            var exist = dbContext.LinePayOrders.SingleOrDefault(x => x.OrderId == orderId);
            if (exist==null)
            {
                LinePayOrder linePayOrder = new LinePayOrder
                {
                    OrderId = orderId,
                    LinePayOrderId = linepayOrderId,
                    TransactionId = transactionId
                };
                dbContext.LinePayOrders.Add(linePayOrder);
            }
            else
            {              
                exist.LinePayOrderId = linepayOrderId;
                exist.TransactionId = transactionId;
            }

        }

        public bool PayConfirm<T>(T req)
        {
            using(dbContext)
            {
                LinePayConfirmReq purchaseConfirmReq =  req as LinePayConfirmReq;
                var linepayOrder = dbContext.LinePayOrders.SingleOrDefault(x => x.LinePayOrderId==purchaseConfirmReq.OtherSysOrderId);
                if(linepayOrder!=null)
                {
                    var orderHeader = dbContext.OrderHeaders.SingleOrDefault(x => x.OrderId==linepayOrder.OrderId);
                    if(orderHeader!=null)
                    {
                        orderHeader.StateCode = OrderStateCode.PAID;
                        orderHeader.IsPaid = true;
                        dbContext.SaveChanges();
                        return true;
                    }                  
                }
                return false;
            }
        }

        private async Task<string> OrderCreatePost(PurchaseOrderCreateReq req)
        {
            using (var httpClient = httpClientFactory.CreateClient())
            {
                var content = new StringContent(JsonSerializer.Serialize(req), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(orderIdCreateApi, content);
                var result = await response.Content.ReadAsStringAsync();
                var model = JsonSerializer.Deserialize<MessageModel<string>>(result);
                return model.Data;
            }
        }
    }
}
