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
        BidController BidCtrl = new BidController();
        ValidateInput Validate = new ValidateInput();

        public bool BidOnItem(int itemId, double amount, string token)
        {
            return !Validate.CheckDouble(amount) ? false : BidCtrl.Bid(itemId, amount, token);
        }
    }
}
