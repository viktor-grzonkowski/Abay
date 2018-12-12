using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AbayMVC.Models;

namespace AbayMVC.Controllers
{
    public class HomeController : Controller
    {
        Services services = Services.Instance;

        [AllowAnonymous]
        public ActionResult Index()
        {
            List<Category> cats = new List<Category>();
            try {
                var categorys = services.ItemClient().GetCategories();
                foreach (var category in categorys)
                {
                    Category cat = new Category
                    {
                        Id = category.Id,
                        Name = category.Name
                    };
                    cats.Add(cat);
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Index");
            }

            return View(cats);
        }
        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "Application description page.";

            return View();
        }
        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.Message = "Contact page.";

            return View();
        }
    }
}