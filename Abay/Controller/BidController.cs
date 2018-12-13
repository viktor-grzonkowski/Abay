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
            Item item = itemCtrl.GetItemById(itemId);
            User buyer = tokenCtrl.GetUserByToken(token);
            
            if (!string.Equals(buyer.UserName,item.SellerUser.UserName))
            {
                if (DateTime.Now < item.EndDate)
                {
                    if (amount > item.InitialPrice)
                    {
                        if (item.Bid != null)
                        {
                            if (amount > item.Bid.Amount)
                            {
                                Bid bid = new Bid
                                {
                                    Id = item.Bid.Id,
                                    Amount = amount,
                                    UserName = buyer.UserName,
                                    Timestamp = DateTime.Now
                                };

                                return bidDb.UpdateBid(bid);
                            }
                        }
                        else
                        {
                            Bid bid = new Bid
                            {
                                Amount = amount,
                                UserName = buyer.UserName,
                                Timestamp = DateTime.Now
                            };

                            int rowId = bidDb.InsertBid(bid);
                            if (rowId >= 0)
                            {
                                bid.Id = rowId;
                                item.Bid = bid;
                                return itemCtrl.UpdateItem(item);
                            }
                        }
                    }
                }
            }
            return false;
        }

        private Bid GetBid(int bidId)
        {
            return bidDb.GetBid(bidId);
        }
    }
}
