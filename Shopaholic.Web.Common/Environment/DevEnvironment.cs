using Microsoft.Extensions.Configuration;
using Shopaholic.Service.Common.Environment;

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
            return configuration.GetValue<string>("LinePay:PRD:ApiUrl");
        }

        public string GetLinePayChannelSecret()
        {
            return configuration.GetValue<string>("LinePay:PRD:ChannelSecret");
        }

        public string GetLinePayChannelId()
        {
            return configuration.GetValue<string>("LinePay:PRD:ChannelId");
        }

        public string GetLinePayBaseUrl()
        {
            return configuration.GetValue<string>("LinePay:PRD:BaseUrl");
        }

        public string GetLinePayConfirmApi()
        {
            return configuration.GetValue<string>("LinePay:PRD:PayConfirmApi");
        }

        public string GetOrderIdCreateApi()
        {
            return configuration.GetValue<string>("OrderIdCreateApi:DEV");
        }

        public string GetEcPayApi()
        {
            return configuration.GetValue<string>("EcPay:PRD:EcPayApi");
        }

        public string GetEcPayConfirmApi()
        {
            return configuration.GetValue<string>("EcPay:PRD:PayConfirmApi");
        }

        public string GetPayConfirmReturnPage()
        {
            return configuration.GetValue<string>("PayConfirmReturnUrl");
        }

        public string GetEcPayMerchantID()
        {
            return configuration.GetValue<string>("EcPay:PRD:MerchantID");
        }

        public string GetFirebaseID()
        {
            return configuration.GetValue<string>("Firebase:PRD:Id");
        }

        public string GetFirebaseUrl()
        {
            return configuration.GetValue<string>("Firebase:PRD:Url");
        }

        public string GetLoginUrl()
        {
            return configuration.GetValue<string>("LoginUrl");
        }

        public string GetImgurClientID()
        {
            throw new NotImplementedException();
        }

        public string GetImgurClientSecret()
        {
            throw new NotImplementedException();
        }

        public string FrontWebUrl()
        {
            return configuration.GetValue<string>("Shopaholic_Url:Front");
        }

        public string CMSWebUrl()
        {
            return configuration.GetValue<string>("Shopaholic_Url:CMS");
        }
    }
}
