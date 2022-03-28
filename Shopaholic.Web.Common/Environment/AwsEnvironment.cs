using Microsoft.Extensions.Configuration;
using Shopaholic.Web.Common.Interface;

namespace Shopaholic.Web.Common.Environment
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

        public string GetReddisConnStr()
        {
            return configuration.GetValue<string>("RedisConnection");
        }

        public string GetLinePayApiUrl()
        {
            return configuration.GetValue<string>("LinePay:ApiUrl");
        }

        public string GetLinePayChannelSecret()
        {
            return configuration.GetValue<string>("LinePay:ChannelSecret");
        }

        public string GetLinePayChannelId()
        {
            return configuration.GetValue<string>("LinePay:ChannelId");
        }

        public string GetLinePayBaseUrl()
        {
            return configuration.GetValue<string>("LinePay:BaseUrl");
        }

        public string GetOrderIdCreateApi()
        {
            return configuration.GetValue<string>("OrderIdCreateApi:AWS");
        }
    }
}
