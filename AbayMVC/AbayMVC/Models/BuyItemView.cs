using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbayMVC.Models
{
    public class BuyItemView
    {
        private static ItemServiceReference.Item serviceItem;
        private static Item webItem;

        public ItemServiceReference.Item ServiceItem
        {
            get
            {
                return serviceItem == null ? new ItemServiceReference.Item() : serviceItem;
            }
            set
            {
                serviceItem = value;
            }
        }
        public Item WebItem
        {
            get
            {
                return webItem == null ? new Item() : webItem;
            }
            set
            {
                webItem = value;
            }
        }
    }
}