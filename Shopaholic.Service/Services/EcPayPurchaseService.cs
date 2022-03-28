using Microsoft.Extensions.Logging;
using Shopaholic.Entity.Models;
using Shopaholic.Service.Common.Constants;
using Shopaholic.Service.Interfaces;
using Shopaholic.Util.Utilities;
using Shopaholic.Web.Common.Factory;
using Shopaholic.Web.Model.Requests;
using Shopaholic.Web.Model.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
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
        private readonly string clientBackApi;

        public EcPayPurchaseService(ILogger<EcPayPurchaseService> logger, ShopaholicContext dbContext, EnvirFactory envirFactory, 
            IHttpClientFactory httpClientFactory)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            orderIdCreateApi = envirFactory.GetEnvir().GetOrderIdCreateApi();
            ecpayApi = "https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut/V5";
            payConfirmApi = "/Order/api/EcPayConfirm";
            clientBackApi = "/Order/Index";
            this.httpClientFactory = httpClientFactory;
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
                var model = JsonSerializer.Deserialize<Background.Model.Responses.MessageModel<string>>(result);
                return model.Data;
            }
        }

        public async Task<PurchasePayRes> Pay(PurchasePayReq req)
        {
            var order = dbContext.OrderHeaders.SingleOrDefault(x => x.OrderId==req.OrderId);
            if (order !=null && order.StateCode==OrderStateCode.CREATE && order.StateCode==OrderFailCode.COMMON)
            {
                var reqBody = GetEcPayReqBody(order.OrderId, req.ConfirmUrl);
                var payRes = await EcPayPost(reqBody);
            }
            return new PurchasePayRes
            {
                IsSuccess = false
            };
        }

        public async Task<string> EcPayPost(Dictionary<string, string> reqBody)
        {
            using (var httpClient = httpClientFactory.CreateClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var formData = new FormUrlEncodedContent(reqBody);
                var response = await httpClient.PostAsync(ecpayApi, formData);
                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
        }

        public Dictionary<string, string> GetEcPayReqBody(string orderId, string clientBackURL)
        {
            List<OrderDetail> orderDetails = dbContext.OrderDetails.Where(x => x.OrderId == orderId).ToList();
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
                {"MerchantID", "2000132"},
                {"MerchantTradeNo", orderId},
                {"MerchantTradeDate", TimeUtil.DateTimeToFormatStr(TimeUtil.GetLocalDateTime(), TimeUtil.yyyyMMddhhmmssFormat_02)},
                {"TotalAmount", totalPrice.ToString()},
                {"ItemName", itemName},
                {"ReturnURL", clientBackURL + payConfirmApi},
                //{"ClientBackURL", clientBackURL + clientBackApi},
            };
            ecpayReq["CheckMacValue"] = GetCheckMacValue(ecpayReq);
            return ecpayReq;
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

        public bool PayConfirm(PurchaseConfirmReq req)
        {
            return true;
        }
    }
}
