using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace AbayMVC.Models
{
    public class Item
    {
        public int ItemId { get; set; }
        public string BuyerName { get; set; }
        public double Amount { get; set; }
    }
}