using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopaholic.Service.Interfaces;
using Shopaholic.Web.Model.Requests;
using Shopaholic.Web.Model.Responses;
using System.Security.Claims;

namespace Shopaholic.Web.Controllers.Api
{
    
    public class AuthController : Controller
    {
        private readonly ILogger Logger;
        private readonly IAuthService authService;

        public AuthController(ILogger<AuthController> logger, IAuthService authService)
        {
            this.Logger = logger;   
            this.authService = authService;
        }

        [Authorize]
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public async Task<MessageModel<bool>> Login([FromBody]AuthLoginReq req)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, req.DisplayName),
                new Claim(ClaimTypes.Email, req.Email),
                //new Claim(ClaimTypes.Role, "Administrator"),
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            var isAuth = HttpContext.User.Identity.IsAuthenticated;

            authService.UpdateUser(req.AccessToken, req.Uid, req.DisplayName, req.Email, 
                req.EmailVerified, req.PhotoURL, req.IsAnonymous);

            return new MessageModel<bool>
            {
                Success = true,
                Msg = "登入",
                Data = true
            };
        }

        [Route("[controller]/api/[action]")]
        [HttpPost]
        public async Task<MessageModel<bool>> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return new MessageModel<bool>
            {
                Success = true,
                Msg = "登出",
                Data = true
            };
        }

        /// <summary>
        /// 確認帳號有效性
        /// </summary>
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [AllowAnonymous]
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public async Task<MessageModel<bool>> ChkExist()
        {
            bool res = false;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var tokenInfo = HttpContext.User;
                string email = tokenInfo.FindFirst(ClaimTypes.Email).Value;
                res = authService.ChkExist("", email);
            }
            if(!res)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            return new MessageModel<bool>
            {
                Success = true,
                Msg = "確認帳號有效性",
                Data = res
            };
        }
    }
}
