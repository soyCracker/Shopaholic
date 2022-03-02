using Microsoft.AspNetCore.Mvc;

namespace Shopaholic.Web.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Cart()
        {
            return View();
        }
    }
}
