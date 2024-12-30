using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using TelerikTreeExample.Services;
using TelerikTreeExample.ViewModels;

namespace TelerikTreeExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpSessionStorage _sessionStorage;
        public HomeController(HttpSessionStorage sessionStorage, ILogger<HomeController> logger)
        {
            _sessionStorage = sessionStorage;
            _logger = logger;
        }


        public IActionResult Index()
        {
            return View();
        }

        public JsonResult GetAllItems([DataSourceRequest] DataSourceRequest request)
        {
            var itemsResult = _sessionStorage.GetAll()
                .ToTreeDataSourceResult(request, e => e.Id, e => e.ParentId);
            return Json(itemsResult);
        }

        public JsonResult CreateItem([DataSourceRequest] DataSourceRequest request, ItemViewModel item)
        {
            if (ModelState.IsValid)
            {
                //foreach (var employee in items)
                //{
                //    //employeeDirectory.Insert(employee, ModelState);
                //}
            }

            return Json(new[] { item }.ToTreeDataSourceResult(request, ModelState));
        }


        public JsonResult UpdateItem([DataSourceRequest] DataSourceRequest request, ItemViewModel item)
        {
            if (ModelState.IsValid)
            {
                //foreach (var employee in employees)
                //{
                //    //employeeDirectory.Update(employee, ModelState);
                //}
            }

            return Json(new[] { item }.ToTreeDataSourceResult(request, ModelState));
        }


        public JsonResult DestroyItem([DataSourceRequest] DataSourceRequest request, ItemViewModel item)
        {
            if (ModelState.IsValid)
            {
                //foreach (var employee in employees)
                //{
                //    //employeeDirectory.Delete(employee, ModelState);
                //}
            }

            return Json(new[] { item }.ToTreeDataSourceResult(request, ModelState));
        }
    }
}
