using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbayMVC.Controllers
{
    public class Services
    {
        ItemServiceReference.ItemServiceClient itemClient = null;
        BidServiceReference.BidServiceClient bidClient = null;
        UserServiceReference.UserServiceClient userClient = null;

        private static Services instance = null;
        private Services()
        {
            
        }

        public static Services Instance
        {
            get
            {
                if (instance == null)
                    instance = new Services();
                return instance;
            }
        }

        public ItemServiceReference.ItemServiceClient ItemClient()
        {
            return itemClient ?? (itemClient = new ItemServiceReference.ItemServiceClient("BasicHttpBinding_IItemService"));
        }

        public BidServiceReference.BidServiceClient BidClient()
        {
            return bidClient ?? (bidClient = new BidServiceReference.BidServiceClient("BasicHttpBinding_IBidService"));
        }

        public UserServiceReference.UserServiceClient UserClient()
        {
            return userClient ?? (userClient = new UserServiceReference.UserServiceClient("BasicHttpBinding_IUserService"));
        }
    }
}