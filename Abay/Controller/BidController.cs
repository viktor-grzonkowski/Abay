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
                if (bid == null)
                {
                    if (amount >= item.InitialPrice && amount > item.FinalPrice)
                        if (CreateBid(itemId, amount, token))
                            return itemCtrl.UpdateItem(itemId, amount);

                    return false;
                }
                else
                {
                    if (amount > bid.Amount && amount >= item.InitialPrice && amount > item.FinalPrice)
                        if(bidDb.Bid(itemId, amount, buyer))
                            return itemCtrl.UpdateItem(itemId, amount);
                    return false;
                }
            }
            else
                return false;
        }

        public Bid GetBid(int itemId)
        {
            return bidDb.GetBid(itemId);
        }

        public bool CreateBid(int itemId, double amount, string token)
        {
            Bid bid = new Bid
            {
                UserName = userCtrl.GetUserByToken(token).UserName,
                ItemId = itemId,
                Amount = amount,
                Timestamp = DateTime.Now
            };

            return bidDb.InsertBid(bid);
        }
    }
}
