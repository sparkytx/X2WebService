using Microsoft.AspNetCore.Mvc;

namespace X2WebService.Controllers.Main
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult QueryEdit()
        {
            return View();
        }
    }

   
}
