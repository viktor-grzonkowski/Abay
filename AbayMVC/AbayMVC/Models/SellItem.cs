using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace AbayMVC.Models
{
    public class SellItem
    {
        public string Titel { get; set; }
        public double InitialPrice { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public WebImage ItemImage { get; set; }
        public string newFileName { get; set; }
        public string imagePath { get; set; }
    }
}