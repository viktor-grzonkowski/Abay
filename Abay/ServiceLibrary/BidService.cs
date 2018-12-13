using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Controller;
using Entities;
using Controller.ServiceInterfaces;

namespace Controller
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "BidService" in both code and config file together.
    public class BidService : IBidService
    {
        BidController BidCtrl = new BidController();

        public bool BidOnItem(int itemId, double amount, string token)
        {
            return BidCtrl.Bid(itemId, amount, token);
        }
    }
}
