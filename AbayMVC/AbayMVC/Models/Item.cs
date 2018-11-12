using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp_Abay_MVC.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double InitialPrice { get; set; }
        public double FinalPrice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string SellerUser { get; set; }
        public string BuyerUser { get; set; }
    }
}