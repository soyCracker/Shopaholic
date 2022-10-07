using Microsoft.AspNetCore.Mvc;
using Shopaholic.Service.Common.Environment;
using Shopaholic.Service.Interfaces;
using Shopaholic.Service.Model.Moels;

namespace Shopaholic.CMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebFlowService webFlowService;
        private readonly string frontUrl;

        public HomeController(ILogger<HomeController> logger, IWebFlowService webFlowService, IEnvironment envir)
        {
            _logger = logger;
            this.webFlowService = webFlowService;
            frontUrl = envir.FrontWebUrl();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Index()
        {
            return View();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Privacy()
        {
            return View();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult GoToFrontWeb()
        {
            return Redirect(frontUrl);
        }

        [Route("[controller]/api/[action]")]
        [HttpPost]
        public MessageModel<List<FlowCountDTO>> GetMonthFlow()
        {
            List<FlowCountDTO> flows = webFlowService.GetMonthFlow();
            return new MessageModel<List<FlowCountDTO>>
            {
                Success = true,
                Msg = "網站月流量",
                Data = flows
            };
        }
    }
}