using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shopaholic.Entity.Models;
using Shopaholic.Service.Common.Constants;
using Shopaholic.Util.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Background.Service.Tasks
{
    public class OrderShipSimulateTask
    {
        private readonly IServiceScopeFactory scopeFactory;

        public OrderShipSimulateTask(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        public void Start()
        {
            using(var scope = scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ShopaholicContext>();
                DateTime now = TimeUtil.GetLocalDateTime();
                var prepareShip = dbContext.OrderHeaders.Where(x => x.StateCode==OrderStateCode.PICKUP && x.FailCode == OrderFailCode.COMMON &&
                    x.UpdateTime<now.AddDays(-2) && x.IsDelete!=true && x.IsSent!=true && x.IsFinish!=true);
                foreach(var order in prepareShip)
                {
                    order.StateCode = OrderStateCode.SENT;
                    order.IsSent = true;
                }
                dbContext.SaveChanges();
            }
        }
    }
}
