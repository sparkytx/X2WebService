using Microsoft.AspNetCore.Mvc;
using X2WebService.Models;

namespace X2WebService.Controllers.Main
{
    public class QueryEditController : Controller
    {
        public IActionResult Index()
        {
            var model = new QueryEditViewModel();
            return View(model);
        }
    }
}
