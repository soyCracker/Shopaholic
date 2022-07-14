using Microsoft.AspNetCore.Mvc;
using Shopaholic.Background.Service.Interfaces;
using Shopaholic.Background.Service.Tasks;
using Shopaholic.Service.Model.Moels;
using Shopaholic.Web.Model.Requests;

namespace Shopaholic.BackgroundWorker.Controllers.Api
{
    public class OrderTaskController : Controller
    {
        private readonly ILogger logger;
        private readonly IBackgroundTaskQueue queue;
        private readonly OrderCreateTask orderCreateTask;

        public OrderTaskController(ILogger<OrderTaskController> logger, OrderCreateTask orderCreateTask)
        {
            this.logger = logger;
            this.orderCreateTask = orderCreateTask;
        }

        /// <summary>
        /// 建立訂單編號
        /// </summary>
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<string> CreateOrderId([FromBody]PurchaseOrderCreateReq req)
        {
            string orderNum = "";
            orderNum = orderCreateTask.Start(req);
            return new MessageModel<string>
            {
                Success = true,
                Msg = "建立訂單編號",
                Data = orderNum
            };
        }

        /// <summary>
        /// 建立訂單編號
        /// </summary>
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<string> Test()
        {
            queue.QueueBackgroundWorkItem(async token =>
            {
                orderCreateTask.Test();
            });
            return new MessageModel<string>
            {
                Success = true,
                Msg = "建立訂單編號",
                Data = "123456789"
            };
        }
    }
}
