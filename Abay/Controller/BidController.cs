using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Database;
using Controller;

namespace Controller
{
    public class BidController
    {
        DBBid bidDb = new DBBid();
        UserController userCtrl = new UserController();
        ItemController itemCtrl = new ItemController();
        TokenController tokenCtrl = new TokenController();

        public bool Bid(int itemId, double amount, string token)
        {
            // Get the item on which the bid goes
            Item item = itemCtrl.GetItemById(itemId);
            // Get the buyer with his token
            User buyer = tokenCtrl.GetUserByToken(token);
            // Get the current winning bid
            Bid currentWinning = GetWinningBid(itemId);

            // The seller can't bid on his own items
            if (!string.Equals(buyer.UserName, item.SellerUser.UserName))
            {
                // Check if item bidding ended
                if (item.EndDate > DateTime.Now)
                {
                    // Check if the amount is higher than the initial price
                    if (amount > item.InitialPrice)
                    {
                        // If there is no bid place the first one 
                        if (currentWinning == null)
                        {
                            Bid newWinning = new Bid
                            {
                                BuyerName = buyer.UserName,
                                ItemId = item.Id,
                                Amount = amount,
                                Timestamp = DateTime.Now,
                                Winning = true
                            };
                            // Insert the bid into the database
                            return bidDb.UpdateBid(null, newWinning) ? true : false;
                        }
                        else
                        {
                            // just get it again to be sure nothing happen
                            currentWinning = GetWinningBid(itemId);

                            // check if the amount is enough
                            if (amount > currentWinning.Amount)
                            {
                                currentWinning.Winning = false;

                                Bid newWinning = new Bid
                                {
                                    BuyerName = buyer.UserName,
                                    ItemId = item.Id,
                                    Amount = amount,
                                    Timestamp = DateTime.Now,
                                    Winning = true
                                };

                                bidDb.UpdateBid(currentWinning, newWinning);
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        public Bid GetWinningBid(int itemId)
        {
            List<Bid> currentWinning = bidDb.GetBids(itemId, true);

            if (currentWinning.Count == 0)
            {
                return null;
            }
            else
            {
                // check if there are more than there should be (it should not happen) ... hopefully
                if (currentWinning.Count > 1)
                {
                    // the first bid on index 0 should be the highest bid so update the rest
                    for (int i = 1; i < currentWinning.Count - 1; i++)
                    {
                        currentWinning[i].Winning = false;
                        bidDb.UpdateBid(currentWinning[i], null);
                    }
                }
            }

            return currentWinning[0];
        }

        public List<Bid> GetAllPrevBids(int itemId)
        {
            return bidDb.GetBids(itemId, false);
        }
    }
}
