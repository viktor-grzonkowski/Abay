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
        ItemServiceReference.ItemServiceClient itemClient = new ItemServiceReference.ItemServiceClient("BasicHttpBinding_IItemService");
        [AllowAnonymous]
        public ActionResult Index()
        {
            List<Category> cats = new List<Category>();
            var categorys = itemClient.GetCategories();
            foreach (var category in categorys)
            {
                Category cat = new Category
                {
                    Id = category.Id,
                    Name = category.Name
                };
                cats.Add(cat);
            }
            return View(cats);
        }
        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}