using Microsoft.Extensions.Logging;
using Shopaholic.Web.Model.Requests;
using Shopaholic.Web.Model.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shopaholic.Base.Console.Class
{
    public class LinePayTestClass
    {
        private readonly ILogger<LinePayTestClass> logger;
        private readonly IHttpClientFactory httpClientFactory;

        public LinePayTestClass(ILogger<LinePayTestClass> logger, IHttpClientFactory httpClientFactory)
        {
            this.logger = logger;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task LinePayPost()
        {
            using(var httpClient = httpClientFactory.CreateClient())
            {
                string apiurl = "/v3/payments/request";
                string ChannelSecret = "cf9fc5f9e08b667be05c5d2774dc214d";
                string ChannelId = "1656957986";
                string nonce = Guid.NewGuid().ToString();
                var reqBody = JsonSerializer.Serialize(GetLinePayReqBody());
                string Signature = HashLinePayRequest(ChannelSecret, apiurl, reqBody, nonce, ChannelSecret);

                httpClient.DefaultRequestHeaders.Add("X-LINE-ChannelId", ChannelId);
                httpClient.DefaultRequestHeaders.Add("X-LINE-Authorization-Nonce", nonce);
                httpClient.DefaultRequestHeaders.Add("X-LINE-Authorization", Signature);
                var content = new StringContent(reqBody, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("https://sandbox-api-pay.line.me" + apiurl, content);
                var result = await response.Content.ReadAsStringAsync();

                LinePayRes linePayRes = JsonSerializer.Deserialize<LinePayRes>(result);
                logger.LogDebug("TEST RES:" + linePayRes.returnMessage);
            }
        }

        private LinePayReq GetLinePayReqBody()
        {
            var reqBody = new LinePayReq
            {
                amount = 10,
                currency = "TWD",
                orderId = Guid.NewGuid().ToString(),
                redirectUrls = new Redirecturls()
                {
                    confirmUrl = "https://tw.yahoo.com/"
                },
                packages = new List<Package>()
                {
                    new Package()
                    {
                        id = "package-1",
                        name = "測試包裹",
                        amount = 10,
                        products = new List<Product>()
                        {
                            new Product()
                            {
                                id = "prod-1",
                                name = "測試商品",
                                imageUrl = "https:////via.placeholder.com//1200//FFFFFF.png?text=1",
                                quantity = 1,
                                price = 10
                            }
                        }
                    }
                }
            };
            return reqBody;
        }

        private string HashLinePayRequest(string channelSecret, string apiUrl, string body, string nonce, string key)
        {
            var request = channelSecret + apiUrl + body + nonce;
            key = key ?? "";
            var encoding = new System.Text.UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(key);
            byte[] messageBytes = encoding.GetBytes(request);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }

        public async Task Get()
        {
            var client = httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("http://api.github.com");
            string result = await client.GetStringAsync("/");            
        }
    }
}
