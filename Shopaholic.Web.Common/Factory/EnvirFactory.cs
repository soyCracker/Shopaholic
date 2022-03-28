using Microsoft.Extensions.Configuration;
using Shopaholic.Web.Common.Environment;
using Shopaholic.Web.Common.Interface;

namespace Shopaholic.Web.Common.Factory
{
    public class EnvirFactory
    {
        private readonly IEnvironment envir;

        public EnvirFactory(IConfiguration configuration, string mode)
        {
            envir = SetEnvir(configuration, mode);
        }

        private IEnvironment SetEnvir(IConfiguration configuration, string mode)
        {
            switch (mode)
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
