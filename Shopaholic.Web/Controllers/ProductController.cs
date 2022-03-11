using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopaholic.Service.Interfaces;
using Shopaholic.Service.Model.Moels;
using Shopaholic.Web.Model.Requests;
using Shopaholic.Web.Model.Responses;
using System.Security.Claims;

namespace Shopaholic.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger Logger;
        private readonly IProductService productService;
        private readonly ICartService cartService;
        private readonly IWebFlowService flowService;

        public ProductController(ILogger<ProductController> logger, IProductService productService,
            ICartService cartService, IWebFlowService flowService)
        {
            this.Logger = logger;
            this.productService = productService;
            this.cartService = cartService;
            this.flowService = flowService;
        }

        public IActionResult Index(int Id)
        {
            return View(Id);
        }

        public IActionResult DetailPage(int Id)
        {
            ProductDTO productDTO = productService.GetProduct(Id);
            return View(productDTO);
        }


        /// <summary>
        /// 搜索商品
        /// </summary>
        [Route("[controller]/api/[action]")]
        [HttpGet]
        public MessageModel<ProductDTO> Get(int Id)
        {
            ProductDTO res = productService.GetProduct(Id);
            return new MessageModel<ProductDTO>
            {
                Success = res != null ? true : false,
                Msg = res != null ? "" : "Fail",
                Data = res
            };
        }

        /// <summary>
        /// 搜索商品
        /// </summary>
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<ProductSearchResDTO> Search([FromBody] ProductSearchByCategoryReq req)
        {
            ProductSearchResDTO res = productService.SearchProductByCategory(req.CategoryId, req.SearchStr, req.Page, req.PageSize);
            return new MessageModel<ProductSearchResDTO>
            {
                Success = res != null ? true : false,
                Msg = res != null ? "" : "Fail",
                Data = res
            };
        }

        /// <summary>
        /// 取得商品TOP5
        /// </summary>
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<List<ProductDTO>> GetFlowTopFive()
        {
            List<ProductDTO> res = productService.GetProductByMonthFlowTop();
            return new MessageModel<List<ProductDTO>>
            {
                Success = res != null ? true : false,
                Msg = res != null ? "" : "Fail",
                Data = res
            };
        }
    }
}
