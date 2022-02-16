using Microsoft.AspNetCore.Mvc;
using Shopaholic.CMS.Model.Requests;
using Shopaholic.CMS.Model.Response;
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

        /// <summary>
        /// 搜索商品
        /// </summary>
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<OrderSearchResDTO> Search([FromBody] OrderSearchReq req)
        {
            OrderSearchResDTO res = orderService.SearchOrder(req.SearchStr, req.Page, req.PageSize);
            return new MessageModel<OrderSearchResDTO>
            {
                Success = res != null ? true : false,
                Msg = res != null ? "" : "Fail",
                Data = res
            };
        }
    }
}
