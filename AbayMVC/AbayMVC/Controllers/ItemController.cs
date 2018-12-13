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

namespace AbayMVC.Controllers
{
    public class ItemController : BaseController
    {
        Services services = Services.Instance;

        [AllowAnonymous]
        public ActionResult Listing()
        {
            string s = Request.QueryString["catId"];
            int categoryId = -1;
            
            if (!string.IsNullOrEmpty(s))
            {
                categoryId = int.Parse(s);
            }
            List<Item> lst = new List<Item>();

            var items = services.ItemClient().GetAllItems(categoryId);

            // Build items 
            foreach (var item in items)
            {
                Item itm = new Item
                {
                    Id = item.Id,
                    Name = item.Name,
                    InitialPrice = item.InitialPrice,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    SellerUser = item.SellerUser.UserName
                };

                // Check if there is a winning bid
                if (item.WinningBid != null)
                {
                    Bid bid = new Bid
                    {
                        BuyerName = item.WinningBid.BuyerName,
                        ItemId = item.WinningBid.ItemId,
                        Amount = item.WinningBid.Amount,
                        Timestamp = item.WinningBid.Timestamp,
                        Winning = item.WinningBid.Winning
                    };

                    itm.WinningBid = bid;
                }

                lst.Add(itm);
            }
            return View(lst);
        }

        [AllowAnonymous]
        public ActionResult Bid()
        {
            string s = Request.QueryString["itemId"];

            if (string.IsNullOrEmpty(s))
            {
                return new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "Item", action = "Listing" }));
            }

            var item = services.ItemClient().GetItemById(Int32.Parse(s));

            Item itm = new Item
            {
                Id = item.Id,
                Name = item.Name,
                InitialPrice = item.InitialPrice,
                StartDate = item.StartDate,
                EndDate = item.EndDate,
                SellerUser = item.SellerUser.UserName
            };

            // Check for winning bid
            if (item.WinningBid != null)
            {
                Bid bid = new Bid {
                    BuyerName = item.WinningBid.BuyerName,
                    ItemId = item.WinningBid.ItemId,
                    Amount = item.WinningBid.Amount,
                    Timestamp = item.WinningBid.Timestamp,
                    Winning = item.WinningBid.Winning
                };

                itm.WinningBid = bid;
            }

            // Check if it has old bids
            if (item.OldBids != null)
            {
                itm.OldBid = new List<Bid>();
                foreach (var oldBid in item.OldBids)
                {
                    Bid bid = new Bid
                    {
                        BuyerName = oldBid.BuyerName,
                        ItemId = oldBid.ItemId,
                        Amount = oldBid.Amount,
                        Timestamp = oldBid.Timestamp,
                        Winning = oldBid.Winning
                    };

                    itm.OldBid.Add(bid);
                }
            }
            return View(itm);
        }

        [HttpPost]
        [CustomAuthAttribute(Roles = "User")]
        public ActionResult Bid(FormCollection collection)
        {
            int itemId = Convert.ToInt32(collection["itemId"]);

            var item = services.ItemClient().GetItemById(itemId);

            double startingPrice = Convert.ToDouble(collection["startingPriceNumber"]);
            double highestOffer = string.IsNullOrEmpty(collection["highestOfferNumber"]) ? 0 : Convert.ToDouble(collection["highestOfferNumber"]);
            double offer = Convert.ToDouble(collection["offerAmount"]);

            string token = SessionPersister.Token;

            Item itmOld = new Item
            {
                Id = item.Id,
                Name = item.Name,
                InitialPrice = item.InitialPrice,
                StartDate = item.StartDate,
                EndDate = item.EndDate,
                SellerUser = item.SellerUser.UserName
            };

            if (services.UserClient().CheckTokenTime(token))
            {
                if (offer <= startingPrice || offer <= highestOffer)
                {
                    Information("Your offer is to low! ");
                    return View(itmOld);
                }
                else if (string.Compare(SessionPersister.Username, collection["sellerName"]) == 0)
                {
                    Danger("Your can't bid on your own item ");
                    return View(itmOld);
                }
                else
                {

                    try
                    {
                        services.BidClient().BidOnItem(itemId, offer, token);
                        var newItem = services.ItemClient().GetItemById(itemId);

                        Item itmNew = new Item
                        {
                            Id = newItem.Id,
                            Name = newItem.Name,
                            InitialPrice = newItem.InitialPrice,
                            StartDate = newItem.StartDate,
                            EndDate = newItem.EndDate,
                            SellerUser = newItem.SellerUser.UserName
                        };

                        // Check for winning bid
                        if (newItem.WinningBid != null)
                        {
                            Bid bid = new Bid
                            {
                                BuyerName = newItem.WinningBid.BuyerName,
                                ItemId = newItem.WinningBid.ItemId,
                                Amount = newItem.WinningBid.Amount,
                                Timestamp = newItem.WinningBid.Timestamp,
                                Winning = newItem.WinningBid.Winning
                            };

                            itmNew.WinningBid = bid;
                        }

                        // Check if it has old bids
                        if (newItem.OldBids != null)
                        {
                            itmNew.OldBid = new List<Bid>();

                            foreach (var oldBid in newItem.OldBids)
                            {
                                Bid bid = new Bid
                                {
                                    BuyerName = oldBid.BuyerName,
                                    ItemId = oldBid.ItemId,
                                    Amount = oldBid.Amount,
                                    Timestamp = oldBid.Timestamp,
                                    Winning = oldBid.Winning
                                };

                                itmNew.OldBid.Add(bid);
                            }
                        }

                        Success("Your bid was placed");
                        return View(itmNew);
                    }
                    catch
                    {
                        Danger("Your bid wasn't placed");
                        return View(itmOld);
                    }
                }
            }
            else
            {
                SessionPersister.Logout();
                Information("Your session timed out");
                return new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "Account", action = "Login" }));
            }
        }

        [CustomAuthAttribute(Roles = "User")]
        public ActionResult Sell()
        {
            List<Category> cats = new List<Category>();
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

            return View(cats);
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

        public ActionResult OldBids(Item modelItem)
        {
            List<Bid> oldBids = new List<Bid>();
            var item = services.ItemClient().GetItemById(modelItem.Id);

            // Check if it has old bids
            if (item.OldBids != null)
            {
                foreach (var oldBid in item.OldBids)
                {
                    Bid bid = new Bid
                    {
                        BuyerName = oldBid.BuyerName,
                        ItemId = oldBid.ItemId,
                        Amount = oldBid.Amount,
                        Timestamp = oldBid.Timestamp,
                        Winning = oldBid.Winning
                    };

                    oldBids.Add(bid);
                }
            }

            return View(oldBids);
        }
    }
}