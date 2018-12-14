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

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter valid price")]
        [DisplayName("Offer ammount")]
        public double Amount { get; set; }
    }
}