using Microsoft.AspNetCore.Mvc;

namespace Shopaholic.CMS.Controllers
{
    public class VueController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
