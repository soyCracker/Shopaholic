using Microsoft.Extensions.Configuration;
using Shopaholic.Service.Common.Environment;

namespace Shopaholic.CMS.Common.Environment
{
    public class AwsEnvironment : IEnvironment
    {
        private readonly IConfiguration configuration;

        public AwsEnvironment(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GetDbConnStr()
        {
            return configuration.GetConnectionString("AWS");
        }

        public string GetEcPayApi()
        {
            throw new NotImplementedException();
        }

        public string GetEcPayConfirmApi()
        {
            throw new NotImplementedException();
        }

        public string GetEcPayMerchantID()
        {
            throw new NotImplementedException();
        }

        public string GetFirebaseID()
        {
            throw new NotImplementedException();
        }

        public string GetFirebaseUrl()
        {
            throw new NotImplementedException();
        }

        public string GetImgurClientID()
        {
            return configuration.GetValue<string>("Imgur:ClientID");
        }

        public string GetImgurClientSecret()
        {
            return configuration.GetValue<string>("Imgur:ClientSecret");
        }

        public string GetLinePayApiUrl()
        {
            throw new NotImplementedException();
        }

        public string GetLinePayBaseUrl()
        {
            throw new NotImplementedException();
        }

        public string GetLinePayChannelId()
        {
            throw new NotImplementedException();
        }

        public string GetLinePayChannelSecret()
        {
            throw new NotImplementedException();
        }

        public string GetLinePayConfirmApi()
        {
            throw new NotImplementedException();
        }

        public string GetLoginUrl()
        {
            throw new NotImplementedException();
        }

        public string GetOrderIdCreateApi()
        {
            throw new NotImplementedException();
        }

        public string GetPayConfirmReturnPage()
        {
            throw new NotImplementedException();
        }

        public string GetReddisConnStr()
        {
            return configuration.GetValue<string>("RedisConnection");
        }
    }
}
