using Microsoft.AspNetCore.Mvc;
using Shopaholic.CMS.Model.Requests;
using Shopaholic.CMS.Model.ViewModels;
using Shopaholic.Service.Interfaces;
using Shopaholic.Service.Model.Moels;

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

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Index()
        {
            return View();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult CreatePage()
        {
            List<CategoryDTO> categoryList = categoryService.GetCategoryList();
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
            return View(categoryVMList);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult EditPage(int id)
        {
            ProductWithAllCategoryDTO dataDTO = productService.GetProductWithAllCategory(id);
            ProductVM productVM = new ProductVM
            {
                Id = dataDTO.ProductDTOItem.Id,
                Name = dataDTO.ProductDTOItem.Name,
                Description = dataDTO.ProductDTOItem.Description,
                Content = dataDTO.ProductDTOItem.Content,
                Image = dataDTO.ProductDTOItem.Image,
                Stock = dataDTO.ProductDTOItem.Stock,
                Price = dataDTO.ProductDTOItem.Price,
                CategoryId = dataDTO.ProductDTOItem.CategoryId
            };
            List<CategoryVM> categoryVMList = new List<CategoryVM>();
            foreach (var categoryDTO in dataDTO.CategoryDTOList)
            {
                CategoryVM categoryVM = new CategoryVM
                {
                    Id = categoryDTO.Id,
                    Name = categoryDTO.Name
                };
                categoryVMList.Add(categoryVM);
            };
            ProductEditVM productEditVM = new ProductEditVM
            {
                ProductVM = productVM,
                CategoryListVM = categoryVMList
            };
            return View(productEditVM);
        }

        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<ProductWithAllCategoryDTO> Test()
        {
            ProductWithAllCategoryDTO dataDTO = productService.GetProductWithAllCategory(1);
            return new MessageModel<ProductWithAllCategoryDTO>
            {
                Success = false,
                Msg = "測試啦",
                Data = dataDTO
            };
        }

        /// <summary>
        /// 新增商品
        /// </summary>
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<ProductAddRequest> Add([FromBody] ProductAddRequest req)
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
        public MessageModel<ProductDTO> Get([FromBody] ProductGetRequest req)
        {
            ProductDTO productDTO = productService.GetProduct(req.Id);
            return new MessageModel<ProductDTO>
            {
                Success = productDTO != null ? true : false,
                Msg = productDTO != null ? "" : "Fail",
                Data = productDTO
            };
        }

        /// <summary>
        /// 修改商品
        /// </summary>
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<ProductUpdateRequest> Update([FromBody] ProductUpdateRequest req)
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
        public MessageModel<ProductSearchResDTO> Search([FromBody] ProductSearchRequest req)
        {
            ProductSearchResDTO res = productService.AdminSearch(req.SearchStr, req.Page, req.PageSize);
            return new MessageModel<ProductSearchResDTO>
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
        /// 上傳商品封面圖片
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
