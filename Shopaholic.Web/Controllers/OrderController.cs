using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopaholic.Service.Interfaces;
using Shopaholic.Service.Model.Moels;
using Shopaholic.Web.Model.Requests;
using Shopaholic.Web.Model.Responses;
using System.Security.Claims;

namespace Shopaholic.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly ILogger Logger;
        private readonly IPurchaseService purchaseService;
        private readonly IOrderService orderService;

        public OrderController(ILogger<ProductController> logger, IPurchaseService purchaseService,
            IOrderService orderService)
        {
            this.Logger = logger;
            this.purchaseService = purchaseService;
            this.orderService = orderService;
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// 搜索商品
        /// </summary>
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<OrderSearchResDTO> Search([FromBody] UserOrderSearchReq req)
        {
            var tokenInfo = HttpContext.User;
            string email = tokenInfo.FindFirst(ClaimTypes.Email).Value;
            OrderSearchResDTO res = orderService.SearchUserOrder(email, req.SearchStr, req.Page, req.PageSize, req.BeginTime, req.EndTime);
            return new MessageModel<OrderSearchResDTO>
            {
                Success = res != null ? true : false,
                Msg = res != null ? "" : "Fail",
                Data = res
            };
        }

        /// <summary>
        /// 建立訂單
        /// </summary>
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<string> CreateOrder([FromBody] PurchaseReq req)
        {
            var tokenInfo = HttpContext.User;
            req.Email = tokenInfo.FindFirst(ClaimTypes.Email).Value;            
            var orderId = purchaseService.CreateOrder(req);
            return new MessageModel<string>
            {
                Success = orderId != null ? true : false,
                Msg = orderId != null ? "" : "Fail",
                Data = orderId
            };
        }

        /// <summary>
        /// 付款
        /// </summary>
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<bool> Pay([FromBody] PayReq req)
        {
            var tokenInfo = HttpContext.User;
            req.Email = tokenInfo.FindFirst(ClaimTypes.Email).Value;
            var res = purchaseService.Pay(req);
            return new MessageModel<bool>
            {
                Success = res,
                Msg = res != null ? "" : "Fail",
                Data = res
            };
        }

        /// <summary>
        /// 申請退貨
        /// </summary>
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<bool> ApplyReturn([FromBody] OrderApplyReturnReq req)
        {
            var res = orderService.ApplyReturn(req.OrderId);
            return new MessageModel<bool>
            {
                Success = res,
                Msg = res != null ? "" : "Fail",
                Data = res
            };
        }
    }
}
