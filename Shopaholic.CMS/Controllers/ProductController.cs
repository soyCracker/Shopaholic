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
        private readonly ICategoryService categoryService;

        public ProductController(ILogger<ProductController> logger, IProductService productService, 
            IStorageService storageService, ICategoryService categoryService)
        {
            this.Logger = logger;
            this.productService = productService;
            this.storageService = storageService;
            this.categoryService = categoryService;
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
            List<Category> categoryList = categoryService.GetCategoryList();
            List<CategoryVM> categoryVMList = new List<CategoryVM>();
            foreach(var item in categoryList)
            {
                CategoryVM categoryVM = new CategoryVM
                {
                    Id = item.Id,
                    Name = item.Name
                };
                categoryVMList.Add(categoryVM);
            }
            return View(categoryVMList);
        }

        public IActionResult EditPage(int Id)
        {
            Product product = productService.GetProduct(Id);
            ProductVM productVM = new ProductVM
            {
                Id = product.Id,
                Name = product.Name,
                Stock = product.Stock,
                Price = product.Price,
                Content = product.Content,
                Description = product.Description,
                CategoryId = product.CategoryId,
                Image = product.Image,
            };
            List<Category> categoryList = categoryService.GetCategoryList();
            List<CategoryVM> categoryVMList = new List<CategoryVM>();
            foreach (var item in categoryList)
            {
                CategoryVM categoryVM = new CategoryVM
                {
                    Id = item.Id,
                    Name = item.Name
                };
                categoryVMList.Add(categoryVM);
            }
            ProductEditVM productEditVM = new ProductEditVM
            {
                ProductVM = productVM,
                CategoryListVM = categoryVMList
            };
            return View(productEditVM);
        }

        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<ProductEditVM> Test()
        {
            Product product = productService.GetProduct(1);
            ProductVM productVM = new ProductVM
            {
                Id = product.Id,
                Name = product.Name,
                Stock = product.Stock,
                Price = product.Price,
                Content = product.Content,
                Description = product.Description,
                CategoryId = product.CategoryId,
                Image = product.Image,
            };
            List<Category> categoryList = categoryService.GetCategoryList();
            List<CategoryVM> categoryVMList = new List<CategoryVM>();
            foreach (var item in categoryList)
            {
                CategoryVM categoryVM = new CategoryVM
                {
                    Id = item.Id,
                    Name = item.Name
                };
                categoryVMList.Add(categoryVM);
            }
            ProductEditVM productEditVM = new ProductEditVM
            {
                ProductVM = productVM,
                CategoryListVM = categoryVMList
            };
            return new MessageModel<ProductEditVM>
            {
                Success = false,
                Msg = "格式錯誤",
                Data = productEditVM
            };
        }

        /// <summary>
        /// 新增商品
        /// </summary>
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<ProductAddRequest> Add([FromBody]ProductAddRequest req)
        {
            if (ModelState.IsValid)
            {
                bool res = productService.AddProduct(req.Name, req.Description, req.CategoryId, req.Content, req.Image, req.Price,
                req.Stock);
                return new MessageModel<ProductAddRequest>
                {
                    Success = res,
                    Msg = res ? "" : "Fail",
                    Data = req
                };
            }
            return new MessageModel<ProductAddRequest>
            {
                Success = false,
                Msg = "格式錯誤",
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
            if (ModelState.IsValid)
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
            return new MessageModel<ProductUpdateRequest>
            {
                Success = false,
                Msg = "格式錯誤",
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

        /// <summary>
        /// 上傳商品內容編輯區內圖片
        /// </summary>
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public async Task<MessageModel<string>> UploadCoverImage(IFormFile file)
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
