using AbayMVC.ItemServiceReference;
using AbayMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbayMVC.Models
{
    public class SellItemView
    {
        private static ItemCategory[] allCategorys;
        private static SellItem sellItem;

        public ItemCategory[] AllCategorys { get => allCategorys; set => allCategorys = value; }

        public SellItem SellItem
        {
            get
            {
                return sellItem == null ? new SellItem() : sellItem;
            }
            set
            {
                sellItem = value;
            }
        }
    }
}