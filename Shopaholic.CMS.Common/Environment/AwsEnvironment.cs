using Microsoft.Extensions.Configuration;
using Shopaholic.CMS.Common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public string GetImgurClientID()
        {
            return configuration.GetValue<string>("Imgur:ClientID");
        }

        public string GetImgurClientSecret()
        {
            return configuration.GetValue<string>("Imgur:ClientSecret");
        }

        public string GetReddisConnStr()
        {
            return configuration.GetValue<string>("RedisConnection");
        }
    }
}
