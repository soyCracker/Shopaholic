using Microsoft.AspNetCore.Mvc;
using Shopaholic.CMS.Model.Requests;
using Shopaholic.CMS.Model.Response;
using Shopaholic.Entity.Models;
using Shopaholic.Service.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

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
            return View();
        }

        /// <summary>
        /// 新增商品類別
        /// </summary>
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<Category> Add([FromBody] CategoryAddRequest req)
        {
            Category category = new Category
            {
                Name = req.Name,
            };
            bool res = categoryService.AddCategory(category);
            return new MessageModel<Category>
            {
                Success = res,
                Msg = res ? "" : "Fail",
                Data = category
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
                Msg = res ? "" : "Fail",
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
                Msg = category != null ? "" : "Fail",
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
                Msg = resList != null ? "" : "Fail",
                Data = resList
            };
        }

        /// <summary>
        /// 修改商品類別
        /// </summary>
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<Category> Update([FromBody] CategoryUpdateRequest req)
        {
            Category category = new Category
            {
                Id = req.Id,
                Name = req.Name,
            };

            bool res = categoryService.UpdateCategory(category);
            return new MessageModel<Category>
            {
                Success = res,
                Msg = res ? "" : "Fail",
                Data = category
            };
        }
    }
}
