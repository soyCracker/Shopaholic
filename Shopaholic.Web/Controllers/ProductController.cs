using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopaholic.Service.Interfaces;
using Shopaholic.Service.Model.Moels;
using Shopaholic.Web.Model.Requests;
using Shopaholic.Web.Model.Responses;
using Shopaholic.Web.Model.ViewModels;
using System;
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

        public IActionResult Index(ProductSearchVM vm)
        {
            return View(vm);
        }

        public IActionResult DetailPage(int Id)
        {
            ProductDTO productDTO = productService.GetProduct(Id);
            return View(productDTO);
        }


        /// <summary>
        /// 取得商品
        /// </summary>
        [Route("[controller]/api/[action]")]
        [HttpGet]
        public MessageModel<ProductDTO> Get(int Id)
        {
            ProductDTO res = productService.GetProduct(Id);
            return new MessageModel<ProductDTO>
            {
                Success = res != null ? true : false,
                Msg = res != null ? "取得商品" : "Fail",
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
            ProductSearchResDTO res = productService.Search(Convert.ToInt32(req.CategoryId), req.SearchStr, req.Page, req.PageSize);
            return new MessageModel<ProductSearchResDTO>
            {
                Success = res != null ? true : false,
                Msg = res != null ? "搜索商品" : "Fail",
                Data = res
            };
        }

        /// <summary>
        /// 取得商品瀏覽TOP5
        /// </summary>
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<List<ProductTopDTO>> GetFlowTopFive()
        {
            List<ProductTopDTO> res = productService.GetProductByMonthFlowTop();
            return new MessageModel<List<ProductTopDTO>>
            {
                Success = res != null ? true : false,
                Msg = res != null ? "取得商品瀏覽TOP5" : "Fail",
                Data = res
            };
        }

        /// <summary>
        /// 取得商品購買TOP5
        /// </summary>
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<List<ProductTopDTO>> GetOrderTopFive()
        {
            List<ProductTopDTO> res = productService.GetProductByMonthOrderTop();
            return new MessageModel<List<ProductTopDTO>>
            {
                Success = res != null ? true : false,
                Msg = res != null ? "取得商品購買TOP5" : "Fail",
                Data = res
            };
        }
    }
}
