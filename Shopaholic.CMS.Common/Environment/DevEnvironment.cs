﻿using Microsoft.Extensions.Configuration;
using Shopaholic.Service.Common.Environment;

namespace Shopaholic.CMS.Common.Environment
{
    public class DevEnvironment : IEnvironment
    {
        private readonly IConfiguration configuration;

        public DevEnvironment(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string CMSWebUrl()
        {
            return configuration.GetValue<string>("Shopaholic_Url:CMS");
        }

        public string FrontWebUrl()
        {
            return configuration.GetValue<string>("Shopaholic_Url:Front");
        }

        public string GetDbConnStr()
        {
            return configuration.GetConnectionString("DEV");
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

        public string GetMsClientId()
        {
            throw new NotImplementedException();
        }

        public string GetMsClientSecret()
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
