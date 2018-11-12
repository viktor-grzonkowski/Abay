using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp_Abay_MVC.BidServiceReference;
using WebApp_Abay_MVC.Models;

namespace WebApp_Abay_MVC.Controllers
{
    public class ItemController : Controller
    {
        Services services = Services.Instance;

        public ActionResult Showcase()
        {
            string s = Request.QueryString["catId"];
            int categoryId = -1;
            
            if (s != null)
            {
                categoryId = int.Parse(s);
            }
            List<Item> lst = new List<Item>();

            var items = services.ItemClient().GetAllItems(categoryId);

            foreach (var item in items)
            {
                Item itm = new Item
                {
                    Id = item.Id,
                    Name = item.Name,
                    InitialPrice = item.InitialPrice,
                    FinalPrice = item.FinalPrice,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    SellerUser = item.SellerUser.UserName,
                    BuyerUser = item.BuyerUser.UserName
                };

                lst.Add(itm);
            }
            return View(lst);
        }
        
        public ActionResult Bid(int itemId)
        {
            var item = services.ItemClient().GetItemById(itemId);

            Item itm = new Item
            {
                Id = item.Id,
                Name = item.Name,
                InitialPrice = item.InitialPrice,
                FinalPrice = item.FinalPrice,
                StartDate = item.StartDate,
                EndDate = item.EndDate,
                SellerUser = item.SellerUser.UserName
            };
            return View(itm);
        }
        [HttpPost]
        public ActionResult Bid(FormCollection collection)
        {
            Item itm = null;

            try
            {
                
                int itemId = Convert.ToInt32(collection["itemId"]);
                double finalPrice = Convert.ToDouble(collection["finalPrice"]);
                string bidder = "bob";

                services.BidClient().BidOnItem(itemId, finalPrice, bidder);
                
                var item = services.ItemClient().GetItemById(itemId);

                itm = new Item
                {
                    Id = item.Id,
                    Name = item.Name,
                    InitialPrice = item.InitialPrice,
                    FinalPrice = item.FinalPrice,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    SellerUser = item.SellerUser.UserName
                };

                return View(itm);
            }
            catch
            {
                return View(itm);
            }
            
        }
    }
}