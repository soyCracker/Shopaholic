using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shopaholic.DI.Configuration.DI
{
    public static class ServiceCollectionExtension
    {
        public static void ConfigureBusinessService(this IServiceCollection service, IConfiguration configuration)
        {
            if(service!=null)
            {
                /*service.AddScoped<IProductService, ProductService>();
                service.AddScoped<ICategoryService, CategoryService>();

                service.AddScoped<IWebFlowService, WebFlowService>();
                service.AddScoped<IOrderService, OrderService>();*/
            }
        }
    }
}
