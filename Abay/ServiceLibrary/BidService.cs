using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Controller;
using Entities;
using ServiceLibrary.ServiceInterfaces;

namespace ServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "BidService" in both code and config file together.
    public class BidService : IBidService
    {
        BidController bidCtrl = new BidController();

        public bool BidOnItem(int itemId, double amount, string token)
        {
            return bidCtrl.Bid(itemId, amount, token);
        }
    }
}
