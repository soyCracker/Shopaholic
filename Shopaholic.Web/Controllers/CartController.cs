using Microsoft.AspNetCore.Mvc;
using Shopaholic.Service.Interfaces;
using Shopaholic.Service.Model.Moels;
using Shopaholic.Web.Model.Requests;
using Shopaholic.Web.Model.Responses;

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

        public IActionResult Index()
        {
            return View();
        }

        // <summary>
        /// 加入購物車
        /// </summary>
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<bool> AddToCart([FromBody] AddToCartReq req)
        {
            bool res = cartService.Add(req.Email, req.ProductId, req.Quantity);
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
        /// 刪除購物車商品
        /// </summary>
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<List<CartWithProductDTO>> GetCart([FromBody] GetCartListReq req)
        {
            List<CartWithProductDTO> cartDTOs = cartService.GetCartWithProductList(req.Email);
            return new MessageModel<List<CartWithProductDTO>>
            {
                Success = true,
                Msg = "Success",
                Data = cartDTOs
            };
        }
    }
}
