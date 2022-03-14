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
    public class CartController : Controller
    {
        private readonly ILogger Logger;
        private readonly ICartService cartService;

        public CartController(ILogger<CartController> logger, ICartService cartService)
        {
            this.Logger = logger;
            this.cartService = cartService;
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult Index()
        {
            return View();
        }

        // <summary>
        /// 加入購物車
        /// </summary>
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<bool> AddToCart([FromBody] AddToCartReq req)
        {
            var tokenInfo = HttpContext.User;
            string email = tokenInfo.FindFirst(ClaimTypes.Email).Value;
            bool res = cartService.Add(email, req.ProductId, req.Quantity);
            return new MessageModel<bool>
            {
                Success = res,
                Msg = res ? "Success" : "Fail",
                Data = res
            };
        }

        // <summary>
        /// 刪除購物車商品
        /// </summary>
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<bool> Delete([FromBody] DeleteCartProductReq req)
        {
            cartService.Delete(req.CartId);
            return new MessageModel<bool>
            {
                Success = true,
                Msg = "Success",
                Data = true
            };
        }

        // <summary>
        /// 取得購物車商品
        /// </summary>
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<List<CartWithProductDTO>> GetCart()
        {
            var tokenInfo = HttpContext.User;
            string email = tokenInfo.FindFirst(ClaimTypes.Email).Value;
            List<CartWithProductDTO> cartDTOs = cartService.GetCartWithProductList(email);
            return new MessageModel<List<CartWithProductDTO>>
            {
                Success = true,
                Msg = "Success",
                Data = cartDTOs
            };
        }

        // <summary>
        /// 取得購物車品項數量
        /// </summary>
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        [AllowAnonymous]
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<int> Count()
        {
            int count = 0;
            if(HttpContext.User.Identity.IsAuthenticated)
            {
                var tokenInfo = HttpContext.User;
                string email = tokenInfo.FindFirst(ClaimTypes.Email).Value;
                count = cartService.Count(email);
            }           
            return new MessageModel<int>
            {
                Success = true,
                Msg = "取得購物車品項數量",
                Data = count
            };
        }
    }
}
