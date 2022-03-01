using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopaholic.Service.Interfaces;
using Shopaholic.Web.Model.Requests;
using Shopaholic.Web.Model.Responses;

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

        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<bool> Login([FromBody]AuthLoginReq req)
        {
            authService.UpdateUser(req.AccessToken, req.Uid, req.DisplayName, req.Email, 
                req.EmailVerified, req.PhotoURL, req.IsAnonymous);
            return new MessageModel<bool>
            {
                Success = true,
                Msg = "測試啦",
                Data = true
            };
        }
    }
}
