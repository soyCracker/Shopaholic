using Microsoft.AspNetCore.Mvc;

namespace Shopaholic.CMS.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
