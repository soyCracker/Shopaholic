using Microsoft.Extensions.Configuration;
using Shopaholic.CMS.Common.Environment;
using Shopaholic.CMS.Common.Interface;
using Shopaholic.Service.Common.Exceptions;

namespace Shopaholic.CMS.Common.Factory
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
            string[] winPath = { "..", "..", "shopaholic.json" };
            string[] linuxPath = { "..", "shopaholic.json" };
            if (File.Exists(Path.Combine(winPath)))
            {
                configPath = Path.Combine(winPath);
            }
            //ubuntu 部署資料夾外
            else if (File.Exists(Path.Combine(linuxPath)))
            {
                configPath = Path.Combine(linuxPath);
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
