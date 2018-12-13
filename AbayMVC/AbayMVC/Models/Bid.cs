using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AbayMVC.Models
{
    public class Bid
    {
        [Display(Name = "Buyer name")]
        public string BuyerName { get; set; }
        public int ItemId { get; set; }
        [Display(Name = "Highest offer")]
        public double Amount { get; set; }
        [Display(Name = "Bid time")]
        public DateTime Timestamp { get; set; }
        public bool Winning { get; set; }
    }
}