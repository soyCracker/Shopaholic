using Microsoft.AspNetCore.Mvc;
using Shopaholic.CMS.Model.Requests;
using Shopaholic.CMS.Model.Response;
using Shopaholic.CMS.Model.ViewModels;
using Shopaholic.Service.Interfaces;
using Shopaholic.Service.Model.Moels;

namespace Shopaholic.CMS.Controllers
{
    public class OrderController : Controller
    {
        private readonly ILogger Logger;
        private readonly IOrderService orderService;

        public OrderController(ILogger<ProductController> logger, IOrderService orderService)
        {
            this.Logger = logger;
            this.orderService = orderService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("[controller]/[action]/{orderId}")]
        public IActionResult EditPage(string orderId)
        {
            OrderIdVM vm = new OrderIdVM
            {
                OrderId = orderId,
            };
            return View(vm);
        }

        /// <summary>
        /// 搜索訂單
        /// </summary>
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<OrderSearchResDTO> Search([FromBody] OrderSearchReq req)
        {
            OrderSearchResDTO res = orderService.SearchOrder(req.SearchStr, req.Page, req.PageSize, req.BeginTime, req.EndTime);
            return new MessageModel<OrderSearchResDTO>
            {
                Success = res != null ? true : false,
                Msg = res != null ? "搜索訂單" : "Fail",
                Data = res
            };
        }

        /// <summary>
        /// 取得訂單資料
        /// </summary>
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<OrderAllDataDTO> GetOrderData([FromBody] OrderGetDataReq req)
        {
            var res = orderService.GetOrderData(req.OrderId);
            return new MessageModel<OrderAllDataDTO>
            {
                Success = res != null ? true : false,
                Msg = res != null ? "取得訂單資料" : "Fail",
                Data = res
            };
        }

        /// <summary>
        /// 撿貨確認
        /// </summary>
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<bool> PickupConfirm([FromBody] OrderGetDataReq req)
        {
            var res = orderService.PickupConfirm(req.OrderId);
            return new MessageModel<bool>
            {
                Success = res,
                Msg = res? "撿貨確認" : "Fail",
                Data = res
            };
        }

        /// <summary>
        /// 退貨確認
        /// </summary>
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<bool> ReturnConfirm([FromBody] OrderGetDataReq req)
        {
            var res = orderService.ReturnConfirm(req.OrderId);
            return new MessageModel<bool>
            {
                Success = res,
                Msg = res? "退貨確認" : "Fail",
                Data = res
            };
        }

        /// <summary>
        /// 訂單手動確認
        /// </summary>
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<bool> ManunlFinish([FromBody] OrderGetDataReq req)
        {
            var res = orderService.ForceFinish(req.OrderId);
            return new MessageModel<bool>
            {
                Success = res,
                Msg = res? "訂單手動確認" : "Fail",
                Data = res
            };
        }
    }
}
