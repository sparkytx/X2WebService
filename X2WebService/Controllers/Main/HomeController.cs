using ComTech.X2.Common.Config;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using X2WebService.Models;

namespace X2WebService.Controllers.Main
{
    public class HomeController : Controller
    {
        private readonly IGetterInfoReadOnlyAsync _infoReadOnlyAsync;

        public HomeController(IGetterInfoReadOnlyAsync infoReadOnlyAsync)
        {
            _infoReadOnlyAsync = infoReadOnlyAsync;
        }
        public IActionResult Index()
        {
            var view=new HomeViewModel(_infoReadOnlyAsync);
            var uri = new Uri(Request.GetDisplayUrl());
            view.BaseUrl = uri.Scheme + "://" + uri.Authority;
            return View(view);
        }
        
    }

   
}
