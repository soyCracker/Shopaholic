using Microsoft.Extensions.Logging;
using Shopaholic.Entity.Models;
using Shopaholic.Service.Common.Constants;
using Shopaholic.Service.Interfaces;
using Shopaholic.Service.Model.Moels;
using Shopaholic.Util.Utilities;
using Shopaholic.Web.Common.Factory;
using Shopaholic.Web.Model.Requests;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Web;

namespace Shopaholic.Service.Services
{
    public class EcPayPurchaseService : IPurchaseService
    {
        private readonly ShopaholicContext dbContext;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger logger;
        private readonly string orderIdCreateApi;
        private readonly string ecpayApi;
        private readonly string payConfirmApi;
        private readonly string payConfirmReturnPage;
        private readonly string merchantID;

        public EcPayPurchaseService(ILogger<EcPayPurchaseService> logger, ShopaholicContext dbContext, EnvirFactory envirFactory, 
            IHttpClientFactory httpClientFactory)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            orderIdCreateApi = envirFactory.GetEnvir().GetOrderIdCreateApi();
            ecpayApi = envirFactory.GetEnvir().GetEcPayApi();
            payConfirmApi = envirFactory.GetEnvir().GetEcPayConfirmApi();
            payConfirmReturnPage = envirFactory.GetEnvir().GetPayConfirmReturnPage();
            this.httpClientFactory = httpClientFactory;
            merchantID = envirFactory.GetEnvir().GetEcPayMerchantID();
        }

        public async Task<string> CreateOrder(PurchaseOrderCreateReq req)
        {
            using (dbContext)
            {
                req.OrderTypeCode = OrderTypeCode.CREDIT_CARD;
                var customer = dbContext.CustomerAccounts.SingleOrDefault(x => x.Email==req.Email);
                if(customer!=null)
                {
                    req.UserId = customer.AccountId;
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
                    if (!string.IsNullOrEmpty(orderId))
                    {
                        dbContext.ShoppingCarts.RemoveRange(carts);
                        dbContext.SaveChanges();
                    }
                    return orderId;
                }
                return null;
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

        public async Task<T> Pay<T>(PurchasePayReq req) where T : class
        {
            List<OrderDetail> orderDetails = dbContext.OrderDetails.Where(x => x.OrderId == req.OrderId).ToList();
            int totalPrice = 0;
            string itemName = "";
            foreach (var detail in orderDetails)
            {
                var product = dbContext.Products.SingleOrDefault(x => x.Id==detail.ProductId);
                totalPrice+=detail.Quantity*detail.CurrentPrice;
                itemName+=product.Name + " x" + detail.Quantity + "#";
            }
            var ecpayReq = new Dictionary<string, string>
            {
                {"MerchantID", merchantID},
                {"MerchantTradeNo", req.OrderId},
                {"MerchantTradeDate", TimeUtil.GetUtcDateTime().ToString(TimeUtil.yyyyMMddhhmmssFormat_02)},
                {"TotalAmount", totalPrice.ToString()},
                {"ItemName", itemName},
                {"ReturnURL", req.ConfirmUrl + payConfirmApi},
                {"PaymentType", "aio"},
                {"TradeDesc", "Shopaholic 商城購物"},
                {"ChoosePayment", "Credit"},
                {"ClientBackURL", req.ConfirmUrl + payConfirmReturnPage},
            };
            ecpayReq["CheckMacValue"] = GetCheckMacValue(ecpayReq);
            logger.LogInformation("EcPayPurchaseService Pay() ReturnURL:" + ecpayReq["ReturnURL"]);
            return ecpayReq as T;
        }

        private string GetCheckMacValue(Dictionary<string, string> order)
        {
            var param = order.Keys.OrderBy(x => x).Select(key => key + "=" + order[key]).ToList();
            var checkValue = string.Join("&", param);
            //測試用的 HashKey
            var hashKey = "5294y06JbISpM5x9";
            //測試用的 HashIV
            var HashIV = "v77hoKGq4kWxNNIS";
            checkValue = $"HashKey={hashKey}" + "&" + checkValue + $"&HashIV={HashIV}";
            checkValue = HttpUtility.UrlEncode(checkValue).ToLower();
            checkValue = GetSHA256(checkValue);
            return checkValue.ToUpper();
        }

        /// <summary>
        /// SHA256 編碼
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetSHA256(string value)
        {
            var result = new StringBuilder();
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(value);
                var hash = sha256.ComputeHash(bytes);
                for (int i = 0; i < hash.Length; i++)
                {
                    result.Append(hash[i].ToString("X2"));
                }
                return result.ToString();
            }            
        }

        public bool PayConfirm<T>(T req)
        {
            logger.LogError("EcPayPurchaseService PayConfirm()");
            EcPayConfirmReq ecPayConfirmReq = req as EcPayConfirmReq;
            using(dbContext)
            {
                var order = dbContext.OrderHeaders.SingleOrDefault(x => x.OrderId==ecPayConfirmReq.MerchantTradeNo);
                if(order != null)
                {
                    order.StateCode = OrderStateCode.PAID;
                    order.IsPaid = true;
                    dbContext.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        
    }
}
