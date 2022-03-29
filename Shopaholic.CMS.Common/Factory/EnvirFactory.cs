using Microsoft.Extensions.Configuration;
using Shopaholic.CMS.Common.Environment;
using Shopaholic.CMS.Common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.CMS.Common.Factory
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
