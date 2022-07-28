using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopaholic.Service.Interfaces;
using Shopaholic.Service.Model.Moels;
using Shopaholic.Service.Services;
using Shopaholic.Web.Model.Requests;
using Shopaholic.Web.Model.Responses;
using System.Security.Claims;

namespace Shopaholic.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly ILogger Logger;
        private readonly IPurchaseService linePayPurchaseService;
        private readonly IPurchaseService ecPayPurchaseService;
        private readonly IOrderService orderService;

        public OrderController(ILogger<ProductController> logger, LinePayPurchaseService linePayPurchaseService,
            IOrderService orderService, EcPayPurchaseService ecPayPurchaseService, IEnumerable<IPurchaseService> purchaseServices)
        {
            this.Logger = logger;
            this.linePayPurchaseService = purchaseServices.Single(x => x.PType == PurchaseType.LinePay);           
            this.ecPayPurchaseService = purchaseServices.Single(x => x.PType == PurchaseType.EcPay);
            this.orderService = orderService;
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult CreatePage()
        {
            return View();
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> EcPayPage(string Id, string baseurl)
        {
            PurchasePayReq req = new PurchasePayReq
            {
                OrderId = Id,
                ConfirmUrl = baseurl
            };
            Dictionary<string, string> res = await ecPayPurchaseService.Pay<Dictionary<string, string>>(req);
            return View(res);
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
        public async Task<MessageModel<string>> LinePayCreateOrder([FromBody] PurchaseOrderCreateReq req)
        {
            var tokenInfo = HttpContext.User;
            req.Email = tokenInfo.FindFirst(ClaimTypes.Email).Value;            
            var orderId = await linePayPurchaseService.CreateOrder(req);
            return new MessageModel<string>
            {
                Success = string.IsNullOrEmpty(orderId) ? false : true,
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
        public async Task<MessageModel<LinePayPurchaseRes>> LinePay([FromBody] PurchasePayReq req)
        {
            LinePayPurchaseRes res = await linePayPurchaseService.Pay<LinePayPurchaseRes>(req);
            return new MessageModel<LinePayPurchaseRes>
            {
                Success = res.IsSuccess,
                Msg = res.Msg,
                Data = res
            };
        }

        /// <summary>
        /// LinePay付款確認
        /// </summary>
        [Route("[controller]/api/[action]")]
        [HttpGet]
        public IActionResult LinePayConfirm([FromQuery] string orderId, [FromQuery]string transactionId)
        {
            LinePayConfirmReq req = new LinePayConfirmReq
            {
                OtherSysOrderId = orderId,
                TransactionId = transactionId
            };
            var res = linePayPurchaseService.PayConfirm(req);

            return RedirectToAction("Index", "Order");
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

        /// <summary>
        /// EcPay建立訂單
        /// </summary>
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public async Task<MessageModel<string>> EcpayCreateOrder([FromBody] PurchaseOrderCreateReq req)
        {
            var tokenInfo = HttpContext.User;
            req.Email = tokenInfo.FindFirst(ClaimTypes.Email).Value;
            var orderId = await ecPayPurchaseService.CreateOrder(req);
            return new MessageModel<string>
            {
                Success = string.IsNullOrEmpty(orderId) ? false : true,
                Msg = orderId != null ? "" : "Fail",
                Data = orderId
            };
        }

        /// <summary>
        /// EcPay付款確認
        /// </summary>
        [Route("[controller]/api/[action]")]
        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        public IActionResult EcPayConfirm([FromForm] EcPayConfirmReq req)
        {
            bool res = ecPayPurchaseService.PayConfirm(req);
            if(res)
            {
                return Ok("1|OK");
            }
            return Ok("0|Error");
        }
    }
}
