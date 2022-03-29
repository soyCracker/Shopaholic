using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shopaholic.Background.Service.Tasks;
using Shopaholic.Util.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Background.Service.Servicces
{
    public class OrderHostedService : BackgroundService
    {
        private readonly ILogger logger;
        private readonly IServiceScopeFactory scopeFactory;
        private DateTime orderShipSimulateTask_lasttime = DateTime.MinValue;
        private DateTime orderOverdueTask_lasttime = DateTime.MinValue;

        public OrderHostedService(ILogger<OrderHostedService> logger, IServiceScopeFactory scopeFactory)
        {
            this.logger = logger;
            this.scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    logger.LogInformation("OrderHostedService ExecuteAsync()");

                    //RunOrderShipSimulateTask();

                    //RunOrderOverdueTask();

                    await Task.Delay(10 * 1000, cancellationToken);
                }
                catch (Exception ex)
                {
                    logger.LogError("OrderHostedService ExecuteAsync:" + ex.Message);
                }
            }
        }

        private void RunOrderShipSimulateTask()
        {
            try
            {
                logger.LogInformation("OrderHostedService RunOrderShipSimulateTask() orderShipSimulateTask_lasttime:" + orderShipSimulateTask_lasttime);
                if (TimeUtil.GetUtcDateTime().AddHours(-8)>orderShipSimulateTask_lasttime)
                {
                    logger.LogInformation("OrderHostedService RunOrderShipSimulateTask() Start()");
                    OrderShipSimulateTask orderShipSimulateTask = new OrderShipSimulateTask(scopeFactory);
                    orderShipSimulateTask.Start();
                    orderShipSimulateTask_lasttime = TimeUtil.GetUtcDateTime().UtcDateTime;
                    logger.LogInformation("OrderHostedService RunOrderShipSimulateTask() Finish");
                }                
            }
            catch(Exception ex)
            {
                logger.LogError("OrderHostedService RunOrderShipSimulateTask() ex:" + ex.Message);
            }
        }

        private void RunOrderOverdueTask()
        {
            try
            {
                logger.LogInformation("OrderHostedService RunOrderOverdueTask() orderOverdueTask_lasttime:" + orderOverdueTask_lasttime);
                if (TimeUtil.GetUtcDateTime().AddHours(-8)>orderOverdueTask_lasttime)
                {
                    logger.LogInformation("OrderHostedService RunOrderOverdueTask() Start()");
                    OrderOverdueTask task = new OrderOverdueTask(scopeFactory);
                    task.Start();
                    orderOverdueTask_lasttime = TimeUtil.GetUtcDateTime().UtcDateTime;
                    logger.LogInformation("OrderHostedService RunOrderOverdueTask() Finish");
                }
            }
            catch (Exception ex)
            {
                logger.LogError("OrderHostedService RunOrderOverdueTask() ex:" + ex.Message);
            }
        }
    }
}
