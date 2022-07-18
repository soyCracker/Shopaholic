using Microsoft.Extensions.Configuration;
using Shopaholic.Service.Common.Exceptions;
using Shopaholic.Web.Common.Environment;
using Shopaholic.Web.Common.Interface;

namespace Shopaholic.Web.Common.Factory
{
    public class EnvirFactory
    {
        private readonly IEnvironment envir;

        public EnvirFactory()
        {
            envir = SetEnvir(GetConfig());
        }

        private IConfiguration GetConfig()
        {
            string configPath = "";
            //windows 專案資料夾外，ex:GitRepo:/shopaholic.json
            if (File.Exists(@"..\..\shopaholic.json"))
            {
                configPath = @"..\..\shopaholic.json";
            }
            //ubuntu 部署資料夾外
            else if (File.Exists(@"..\shopaholic.json"))
            {
                configPath = @"..\shopaholic.json";
            }
            else
            {
                throw new ShopaholicConfigNotFoundException();
            }

            var builder = new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile(Path.GetFullPath(configPath));
            var shopaholicConfig = builder.Build();
            return shopaholicConfig;
        }

        private IEnvironment SetEnvir(IConfiguration configuration)
        {
            switch (configuration.GetValue<string>("EnvirMode"))
            {
                case "AWS":
                    return new AwsEnvironment(configuration);
                case "DEV":
                    return new DevEnvironment(configuration);
                default:
                    return new AwsEnvironment(configuration);
            }
        }

        public IEnvironment GetEnvir()
        {
            return envir;
        }
    }
}
