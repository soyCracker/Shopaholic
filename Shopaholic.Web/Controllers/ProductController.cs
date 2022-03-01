using Microsoft.AspNetCore.Mvc;
using Shopaholic.Service.Interfaces;
using Shopaholic.Service.Model.Moels;
using Shopaholic.Web.Model.Requests;
using Shopaholic.Web.Model.Responses;

namespace Shopaholic.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger Logger;
        private readonly IProductService productService;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            this.Logger = logger;
            this.productService = productService;
        }

        public IActionResult Index(int Id)
        {

            return View(Id);
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
    }
}
