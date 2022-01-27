using Microsoft.AspNetCore.Mvc;
using Shopaholic.CMS.Models;
using Shopaholic.Service.Interfaces;
using Shopaholic.Service.Model.Moels;
using System.Diagnostics;

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

        public IActionResult Index()
        {
            List<FlowCountDTO> flows = webFlowService.GetMonthFlow();
            return View(flows);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}