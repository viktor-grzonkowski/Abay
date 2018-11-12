using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp_Abay_MVC.Controllers
{
    public class Services
    {
        ItemServiceReference.ItemServiceClient itemClient = null;
        BidServiceReference.BidServiceClient bidClient = null;

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
            return itemClient == null ? itemClient = new ItemServiceReference.ItemServiceClient("BasicHttpBinding_IItemService") : itemClient;
        }

        public BidServiceReference.BidServiceClient BidClient()
        {
            return bidClient == null ? bidClient = new BidServiceReference.BidServiceClient("BasicHttpBinding_IBidService") : bidClient;
        }
    }
}