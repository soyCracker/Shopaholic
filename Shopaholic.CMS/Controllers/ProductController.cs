using Microsoft.AspNetCore.Mvc;
using Shopaholic.CMS.Model.Requests;
using Shopaholic.CMS.Model.Response;
using Shopaholic.Entity.Models;
using Shopaholic.Service.Interfaces;


namespace Shopaholic.CMS.Controllers
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

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 新增商品
        /// </summary>
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<Product> Add([FromBody]ProductAddRequest req)
        {
            Product product = new Product
            {
                Name = req.Name,
                Description = req.Description,
                CategoryId = req.CategoryId,
                Content = req.Content,
                Image = req.Image,
                Price = req.Price,
                Stock = req.Stock
            };
            bool res = productService.AddProduct(product);
            return new MessageModel<Product>
            {
                Success = res,
                Msg = res ? "" : "Fail",
                Data = product
            };
        }

        /// <summary>
        /// 刪除商品
        /// </summary>
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<ProductDeleteRequest> Delete([FromBody] ProductDeleteRequest req)
        {          
            bool res = productService.DeleteProduct(req.Id);
            return new MessageModel<ProductDeleteRequest>
            {
                Success = res,
                Msg = res ? "" : "Fail",
                Data = req
            };
        }

        /// <summary>
        /// 取得單一商品
        /// </summary>
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<Product> Get([FromBody] ProductGetRequest req)
        {
            Product product = productService.GetProduct(req.Id);
            return new MessageModel<Product>
            {
                Success = product != null ? true : false,
                Msg = product != null ? "" : "Fail",
                Data = product
            };
        }

        /// <summary>
        /// 取得全部商品清單
        /// </summary>
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<List<Product>> GetList()
        {
            List<Product> productList = productService.GetProductList();
            return new MessageModel<List<Product>>
            {
                Success = productList != null ? true : false,
                Msg = productList != null ? "" : "Fail",
                Data = productList
            };
        }

        /// <summary>
        /// 修改商品
        /// </summary>
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<Product> Update([FromBody] ProductUpdateRequest req)
        {
            Product product = new Product
            {
                Id = req.Id,
                Description = req.Description,
                Name = req.Name,
                CategoryId = req.CategoryId,
                Content = req.Content,
                Image = req.Image,
                Price = req.Price,
                Stock = req.Stock
            };

            bool res = productService.UpdateProduct(product);
            return new MessageModel<Product>
            {
                Success = res,
                Msg = res ? "" : "Fail",
                Data = product
            };
        }
    }
}
