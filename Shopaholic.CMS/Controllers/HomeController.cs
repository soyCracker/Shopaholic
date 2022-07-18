using Microsoft.AspNetCore.Mvc;
using Shopaholic.Service.Interfaces;
using Shopaholic.Service.Model.Moels;

namespace Shopaholic.CMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebFlowService webFlowService;

        public HomeController(ILogger<HomeController> logger, IWebFlowService webFlowService)
        {
            _logger = logger;
            this.webFlowService = webFlowService;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Index()
        {
            List<FlowCountDTO> flows = webFlowService.GetMonthFlow();
            return View(flows);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Privacy()
        {
            return View();
        }
    }
}