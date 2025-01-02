using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using TelerikTreeExample.Services;
using TelerikTreeExample.ViewModels;

namespace TelerikTreeExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpSessionStorage _sessionStorage;

        public HomeController(HttpSessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }



        public JsonResult GetItems([DataSourceRequest] DataSourceRequest request)
        {
            var itemsResult = _sessionStorage.GetAll()
                .ToTreeDataSourceResult(request, e => e.Id, e => e.ParentId);
            return Json(itemsResult);
        }


        public JsonResult CreateItems([DataSourceRequest] DataSourceRequest request, 
            [Bind(Prefix = "models")] IEnumerable<ItemViewModel> items)
        {
            if (ModelState.IsValid)
                _sessionStorage.Create(items.ToArray());

            return Json(items.ToTreeDataSourceResult(request, ModelState));
        }


        public JsonResult UpdateItems([DataSourceRequest] DataSourceRequest request,
            [Bind(Prefix = "models")] [FromForm] IEnumerable<ItemViewModel> items)
        {
            if (ModelState.IsValid)
                _sessionStorage.Update(items.ToArray());

            return Json(items.ToTreeDataSourceResult(request, ModelState));
        }


        public JsonResult DestroyItems([DataSourceRequest] DataSourceRequest request,
            [Bind(Prefix = "models")] IEnumerable<ItemViewModel> items)
        {
            if (ModelState.IsValid)
                _sessionStorage.Delete(items.ToArray());

            return Json(items.ToTreeDataSourceResult(request, ModelState));
        }
    }
}
