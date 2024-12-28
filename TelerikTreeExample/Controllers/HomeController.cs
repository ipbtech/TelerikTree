using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using TelerikTreeExample.Services;

namespace TelerikTreeExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpSessionService _sessionService;
        public HomeController(HttpSessionService sessionService, ILogger<HomeController> logger)
        {
            _sessionService = sessionService;
            _logger = logger;
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Index([DataSourceRequest] DataSourceRequest request)
        {
            return View();
        }

        public JsonResult GetItems([DataSourceRequest] DataSourceRequest request)
        {
            var itemsResult = _sessionService.GetItems()
                .ToTreeDataSourceResult(request, e => e.Id, e => e.ParentId);
            return Json(itemsResult);
        }
    }
}
