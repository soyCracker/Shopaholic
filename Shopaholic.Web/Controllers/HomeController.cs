using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopaholic.Service.Interfaces;
using Shopaholic.Service.Model.Moels;
using Shopaholic.Service.Services;
using Shopaholic.Web.Model.Responses;
using Shopaholic.Web.Models;
using System.Diagnostics;

namespace Shopaholic.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly ICategoryService categoryService;

        public HomeController(ILogger<HomeController> logger, ICategoryService categoryService)
        {
            logger = logger;
            this.categoryService = categoryService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }


        /// <summary>
        /// 取得商品類別清單
        /// </summary>
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<List<CategoryDTO>> GetCategoryList()
        {
            List<CategoryDTO> resList = categoryService.GetCategoryList();
            return new MessageModel<List<CategoryDTO>>
            {
                Success = resList != null ? true : false,
                Msg = resList != null ? "Success" : "Fail",
                Data = resList
            };
        }

        [HttpPost]
        [Authorize]
        [Route("[controller]/api/[action]")]
        public IActionResult Test()
        {

            return Ok(new { Value = true, ErrorCode = 0, Res = "Good Auth" });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}