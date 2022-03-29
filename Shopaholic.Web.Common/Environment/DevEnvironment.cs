using Microsoft.Extensions.Configuration;
using Shopaholic.Web.Common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Web.Common.Environment
{
    public class DevEnvironment : IEnvironment
    {
        private readonly IConfiguration configuration;

        public DevEnvironment(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GetDbConnStr()
        {
            return configuration.GetConnectionString("DEV");
        }

        public string GetReddisConnStr()
        {
            return configuration.GetValue<string>("RedisConnection");
        }

        public string GetLinePayApiUrl()
        {
            return configuration.GetValue<string>("LinePay:AWS:ApiUrl");
        }

        public string GetLinePayChannelSecret()
        {
            return configuration.GetValue<string>("LinePay:AWS:ChannelSecret");
        }

        public string GetLinePayChannelId()
        {
            return configuration.GetValue<string>("LinePay:AWS:ChannelId");
        }

        public string GetLinePayBaseUrl()
        {
            return configuration.GetValue<string>("LinePay:AWS:BaseUrl");
        }

        public string GetLinePayConfirmApi()
        {
            return configuration.GetValue<string>("LinePay:AWS:PayConfirmApi");
        }

        public string GetOrderIdCreateApi()
        {
            return configuration.GetValue<string>("OrderIdCreateApi:DEV");
        }

        public string GetEcPayApi()
        {
            return configuration.GetValue<string>("EcPay:AWS:EcPayApi");
        }

        public string GetEcPayConfirmApi()
        {
            return configuration.GetValue<string>("EcPay:AWS:PayConfirmApi");
        }

        public string GetPayConfirmReturnPage()
        {
            return configuration.GetValue<string>("PayConfirmReturnUrl");
        }

        public string GetEcPayMerchantID()
        {
            return configuration.GetValue<string>("EcPay:AWS:MerchantID");
        }
    }
}
