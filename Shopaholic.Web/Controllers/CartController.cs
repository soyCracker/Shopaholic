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
            /*var tokenInfo = HttpContext.User;
            string email = tokenInfo.FindFirst(ClaimTypes.Email).Value;*/
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
    }
}
