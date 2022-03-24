using Microsoft.Extensions.DependencyInjection;
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
    public class OrderOverdueTask
    {
        private readonly IServiceScopeFactory scopeFactory;

        public OrderOverdueTask(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        public void Start()
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ShopaholicContext>();
                DateTime now = TimeUtil.GetLocalDateTime();
                var prepareShip = dbContext.OrderHeaders.Where(x => x.StateCode==OrderStateCode.CREATE && x.FailCode == OrderFailCode.COMMON &&
                    x.UpdateTime<now.AddDays(-5) && x.IsDelete!=true && x.IsCancel!=true && x.IsFinish!=true);
                foreach (var order in prepareShip)
                {
                    order.IsCancel = true;
                    order.IsFinish = true;
                    order.FailCode = OrderFailCode.OVERDUE;
                }
                dbContext.SaveChanges();
            }
        }
    }
}
