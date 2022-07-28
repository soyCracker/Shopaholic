using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopaholic.Service.Interfaces;
using Shopaholic.Service.Model.Moels;
using Shopaholic.Web.Model.Requests;
using System.Security.Claims;

namespace Shopaholic.Web.Controllers.Api
{
    public class FlowController : Controller
    {
        private readonly ILogger Logger;
        private readonly IWebFlowService flowService;

        public FlowController(ILogger<AuthController> logger, IWebFlowService flowService)
        {
            this.Logger = logger;
            this.flowService = flowService;
        }

        /// <summary>
        /// 增加瀏覽紀錄
        /// </summary>
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [AllowAnonymous]
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<bool> Add([FromBody]FlowAddReq req)
        {
            string remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4()==null
                ? "" : Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            string email = "";
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var tokenInfo = HttpContext.User;
                email = tokenInfo.FindFirst(ClaimTypes.Email).Value;
            }

            flowService.AddFlow(remoteIpAddress, req.Enter, email);
            return new MessageModel<bool>
            {
                Success = true,
                Msg = "增加瀏覽紀錄",
                Data = true
            };
        }
    }
}
