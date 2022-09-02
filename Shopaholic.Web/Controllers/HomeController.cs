﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shopaholic.Service.Common.Environment;
using Shopaholic.Service.Interfaces;
using Shopaholic.Service.Model.Moels;
using System.Security.Claims;

namespace Shopaholic.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly ICategoryService categoryService;
        private readonly IPopularService popularService;
        private readonly IEnvironment envir;
        private readonly SignInManager<HomeController> signInManager;

        public HomeController(ILogger<HomeController> logger, ICategoryService categoryService, IPopularService popularService, 
            IEnvironment envir)
        {
            logger = logger;
            this.categoryService = categoryService;
            this.popularService = popularService;
            this.envir = envir;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Firebase auth只針對api，view的auth是依靠Cookie-based Authentication，要在action指定
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public IActionResult Privacy()
        {
            var tokenInfo = HttpContext.User;
            string email = tokenInfo.FindFirst(ClaimTypes.Email).Value;
            var isAuth = HttpContext.User.Identity.IsAuthenticated;
            return View();
        }

        public IActionResult LoginPage()
        {
            return View();
        }

        public IActionResult GoToCMS()
        {
            return Redirect(envir.CMSWebUrl());
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

        /// <summary>
        /// 取得商品瀏覽TOP5
        /// </summary>
        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<List<ProductTopDTO>> GetFlowTopFive()
        {
            List<ProductTopDTO> res = popularService.GetProductByMonthFlowTop();
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
            List<ProductTopDTO> res = popularService.GetProductByMonthOrderTop();
            return new MessageModel<List<ProductTopDTO>>
            {
                Success = res != null ? true : false,
                Msg = res != null ? "取得商品購買TOP5" : "Fail",
                Data = res
            };
        }

        public ActionResult MsLogin()
        {
            //var props = new AuthenticationProperties();
            //props.RedirectUri = "https://localhost:44347/Auth";
            return Challenge("Microsoft");
            //string redirectUrl = Url.Action("signin-google");
            //var properties = signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            //var p = new AuthenticationProperties { RedirectUri = redirectUrl };
            //return new ChallengeResult("Google");
        }
    }
}