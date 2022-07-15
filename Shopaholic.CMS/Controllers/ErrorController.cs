using Microsoft.AspNetCore.Mvc;
using Shopaholic.CMS.Models;
using Shopaholic.Service.Common.Filters;
using System.Diagnostics;

namespace Shopaholic.CMS.Controllers
{
    public class ErrorController : Controller
    {
        [SwaggerIgnore]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error404()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [SwaggerIgnore]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error500()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
