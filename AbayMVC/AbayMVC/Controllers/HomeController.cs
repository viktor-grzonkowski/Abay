using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AbayMVC.Models;

namespace AbayMVC.Controllers
{
    public class HomeController : Controller
    {
        Services services = Services.Instance;

        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            return View(await services.ItemClient().GetAllCategoriesAsync());
        }
        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "Application description page";

            return View();
        }
        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.Message = "Contact page";

            return View();
        }
    }
}