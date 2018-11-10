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

        public bool Bid(int itemId, double amount, string token)
        {
            Bid bid = GetBid(itemId);
            User buyer = userCtrl.GetUserByToken(token);
            Item item = itemCtrl.GetItemById(itemId);

            if (!string.Equals(buyer.UserName,item.SellerUser.UserName))
            {
                if (DateTime.Now < item.EndDate)
                {
                    if (amount >= item.InitialPrice && amount > item.FinalPrice)
                    {
                        item.BuyerUser = buyer;
                        item.FinalPrice = amount;

                        if (bid == null)
                        {
                            if (CreateBid(item))
                                return itemCtrl.UpdateItem(item);
                            return false;
                        }
                        else
                        {
                            if (amount > bid.Amount)
                                if (Bid(item))
                                    return itemCtrl.UpdateItem(item);
                            return false;
                        }
                    }
                }
            }
            return false;
        }

        public Bid GetBid(int itemId)
        {
            return bidDb.GetBid(itemId);
        }

        public bool CreateBid(Item item)
        {
            Bid bid = new Bid
            {
                UserName = item.BuyerUser.UserName,
                ItemId = item.Id,
                Amount = item.FinalPrice,
                Timestamp = DateTime.Now
            };

            return bidDb.InsertBid(bid);
        }

        public bool Bid(Item item)
        {
            return bidDb.Bid(item);
        }
    }
}
