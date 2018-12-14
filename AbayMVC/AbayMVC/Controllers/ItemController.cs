using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using AbayMVC.BidServiceReference;
using AbayMVC.Models;
using AbayMVC.Security;
using System.Web.Routing;
using System.Threading.Tasks;

namespace AbayMVC.Controllers
{
    public class ItemController : BaseController
    {
        Services services = Services.Instance;

        [AllowAnonymous]
        public async Task<ActionResult> Listing()
        {
            string s = Request.QueryString["catId"];
            int categoryId = -1; 
            if (!string.IsNullOrEmpty(s))
            {
                categoryId = int.Parse(s);
            }

            return View(await services.ItemClient().GetAllActiveItemsByCategoryAsync(categoryId));
        }

        [AllowAnonymous]
        public async Task <ActionResult> Bid()
        {
            string s = Request.QueryString["itemId"];
            if (string.IsNullOrEmpty(s))
                return RedirectToAction("Listing", "Item");

            // Create viewmodel
            BuyItemView view = new BuyItemView();
            view.ServiceItem = await services.ItemClient().GetItemByIdAsync(Int32.Parse(s));
            view.WebItem = new Item {
                ItemId = Int32.Parse(s)
            };

            if(!string.IsNullOrEmpty(SessionPersister.Username))
                view.WebItem.BuyerName = SessionPersister.Username;
            int count = view.ServiceItem.OldBids.Count();
            return View(view);
        }

        [HttpPost]
        [CustomAuthAttribute(Roles = "User")]
        public async Task<ActionResult> Bid(BuyItemView viewCollection)
        {
            
            if (await services.UserClient().CheckTokenTimeAsync(SessionPersister.Token))
            {
                // no bid right now
                if (viewCollection.ServiceItem.WinningBid != null)
                {
                    // check if the amount is higher than the previous bid
                    if (viewCollection.WebItem.Amount <= viewCollection.ServiceItem.WinningBid.Amount)
                    {
                        Information("Your offer is to low! ");
                        return View(viewCollection);
                    }
                }
                // offer is beneath the initial price
                if (viewCollection.WebItem.Amount <= viewCollection.ServiceItem.InitialPrice)
                {
                    Information("Your offer is to low! ");
                    return View(viewCollection);
                }
                // seller and buyer is the same
                if (string.Equals(SessionPersister.Username, viewCollection.ServiceItem.SellerUser.UserName))
                {
                    Danger("You can't bid on your own item ");
                    return View(viewCollection);
                }

                // Place bid
                if (await services.BidClient().BidOnItemAsync(viewCollection.WebItem.ItemId, viewCollection.WebItem.Amount, SessionPersister.Token))
                {
                    viewCollection.ServiceItem = null;
                    viewCollection.WebItem.Amount = 0;
                    viewCollection.ServiceItem = await services.ItemClient().GetItemByIdAsync(viewCollection.WebItem.ItemId);

                    Success("Your bid was placed");
                    return View(viewCollection);
                }

                Danger("Your bid was not placed");
                return View(viewCollection);
            }
            else
            {
                SessionPersister.Logout();
                Danger("Your session timed out");
                return RedirectToAction("Login", "Account");
            }
        }

        public async Task<ActionResult> OldBids(ItemServiceReference.Item modelItem)
        {
            return View(await services.ItemClient().GetAllBidsByItemAsync(modelItem.Id));
        }


        [CustomAuthAttribute(Roles = "User")]
        public async Task<ActionResult> Sell()
        {
            return View(await services.ItemClient().GetAllCategoriesAsync());
        }

        [HttpPost]
        [CustomAuthAttribute(Roles = "User")]
        public ActionResult Sell(FormCollection collection)
        {
            string name = collection["item-name"];
            double startingPrice = Double.Parse(collection["item-startPrice"]);
            int category = Int32.Parse(collection["item-category"]); 
            string description = Request.Form["item-description"];
            int duration = Int32.Parse(collection["item-sellduration"]);

            int insertId = services.ItemClient().CreateItem(name, description, startingPrice, category, SessionPersister.Token, duration);

            if (insertId < 0)
            {
                Danger("Looks like something went wrong. Please check your form.");
            }
            else
            {
                Success("Your item is on sale now!");
                return new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "Item", action = "Bid", itemId = insertId }));
            }

            return new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "Home", action = "Index" }));
        }
    }
}