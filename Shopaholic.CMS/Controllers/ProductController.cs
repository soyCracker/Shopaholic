using Microsoft.AspNetCore.Mvc;
using Shopaholic.CMS.Model.Requests;
using Shopaholic.CMS.Model.Response;
using Shopaholic.CMS.Model.ViewModels;
using Shopaholic.Entity.Models;
using Shopaholic.Service.Interfaces;
using System.Text.Json;

namespace Shopaholic.CMS.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger Logger;
        private readonly IProductService productService;
        private readonly IStorageService storageService;

        public ProductController(ILogger<ProductController> logger, IProductService productService, IStorageService storageService)
        {
            this.Logger = logger;
            this.productService = productService;
            this.storageService = storageService;
        }

        public IActionResult Index()
        {
            List<Product> resList = productService.GetProductList();
            List<ProductVM> productVMList = new List<ProductVM>();
            foreach (var item in resList)
            {
                ProductVM productVM = new ProductVM
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Content = item.Content,
                    Price = item.Price,
                    Stock = item.Stock,
                    Image = item.Image,
                    CategoryId = item.CategoryId,
                };
                productVMList.Add(productVM);
            }
            return View(productVMList);
        }

        public IActionResult CreatePage()
        {
            return View();
        }

        public IActionResult EditPage(ProductVM vm)
        {
            return View(vm);
        }

        /// <summary>
        /// 新增商品
        /// </summary>
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<ProductAddRequest> Add([FromBody]ProductAddRequest req)
        {
            bool res = productService.AddProduct(req.Name, req.Description, req.CategoryId, req.Content, req.Image, req.Price,
                req.Stock );
            return new MessageModel<ProductAddRequest>
            {
                Success = res,
                Msg = res ? "" : "Fail",
                Data = req
            };
        }

        /// <summary>
        /// 刪除商品
        /// </summary>
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<ProductDeleteRequest> Delete([FromBody]ProductDeleteRequest req)
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
        public MessageModel<Product> Get([FromBody]ProductGetRequest req)
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
        public MessageModel<ProductUpdateRequest> Update([FromBody]ProductUpdateRequest req)
        {
            bool res = productService.UpdateProduct(req.Id, req.Name, req.Description, req.CategoryId, req.Content, req.Image, 
                req.Price, req.Stock);
            return new MessageModel<ProductUpdateRequest>
            {
                Success = res,
                Msg = res ? "" : "Fail",
                Data = req
            };
        }

        /// <summary>
        /// 搜索商品
        /// </summary>
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<List<Product>> Search([FromBody] ProductSearchRequest req)
        {
            List<Product> res = productService.SearchProduct(req.Name, req.Description, req.Content);
            return new MessageModel<List<Product>>
            {
                Success = res != null ? true : false,
                Msg = res != null ? "" : "Fail",
                Data = res
            };
        }

        /// <summary>
        /// 上傳商品內容編輯區內圖片
        /// </summary>
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public async Task<MessageModel<string>> UploadEditorImage(IFormFile file)
        {
            string url = await storageService.UploadFile(ControllerContext.ActionDescriptor.ControllerName, file);
            return new MessageModel<string>
            {
                Success = true,
                Msg = url != null ? "" : "Fail",
                Data = url
            };
        }
    }
}
