using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopaholic.CMS.Model.Requests;
using Shopaholic.CMS.Model.Response;
using Shopaholic.CMS.Model.ViewModels;
using Shopaholic.Entity.Models;
using Shopaholic.Service.Interfaces;

namespace Shopaholic.CMS.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILogger Logger;
        private readonly ICategoryService categoryService;

        public CategoryController(ILogger<ProductController> logger, ICategoryService categoryService)
        {
            this.Logger = logger;
            this.categoryService = categoryService;
        }

        public IActionResult Index()
        {
            List<Category> resList = categoryService.GetCategoryList();
            List<CategoryVM> categoryVMList = new List<CategoryVM>();
            foreach(var item in resList)
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

        public IActionResult CreatePage()
        {           
            return View();
        }

        /// <summary>
        /// 新增商品類別
        /// </summary>
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<CategoryAddRequest> Add([FromBody] CategoryAddRequest req)
        {           
            bool res = categoryService.AddCategory(req.Name);
            return new MessageModel<CategoryAddRequest>
            {
                Success = res,
                Msg = res ? "Success" : "Fail",
                Data = req
            };
        }

        /// <summary>
        /// 刪除商品類別
        /// </summary>
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<CategoryDeleteRequest> Delete([FromBody] CategoryDeleteRequest req)
        {
            bool res = categoryService.DeleteCategory(req.Id);
            return new MessageModel<CategoryDeleteRequest>
            {
                Success = res,
                Msg = res ? "Success" : "Fail",
                Data = req
            };        
        }

        /// <summary>
        /// 取得單一商品類別
        /// </summary>
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<Category> Get([FromBody] CategoryGetRequest req)
        {
            Category category = categoryService.GetCategory(req.Id);
            return new MessageModel<Category>
            {
                Success = category != null ? true : false,
                Msg = category != null ? "Success" : "Fail",
                Data = category
            };
        }

        /// <summary>
        /// 取得商品類別清單
        /// </summary>
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<List<Category>> GetList()
        {
            List<Category> resList = categoryService.GetCategoryList();
            return new MessageModel<List<Category>>
            {
                Success = resList != null ? true : false,
                Msg = resList != null ? "Success" : "Fail",
                Data = resList
            };
        }

        /// <summary>
        /// 修改商品類別
        /// </summary>
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<CategoryUpdateRequest> Update([FromBody] CategoryUpdateRequest req)
        {
            bool res = categoryService.UpdateCategory(req.Id, req.Name);
            return new MessageModel<CategoryUpdateRequest>
            {
                Success = res,
                Msg = res ? "Success" : "Fail",
                Data = req
            };
        }
    }
}
