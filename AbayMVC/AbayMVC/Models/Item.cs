using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbayMVC.Models
{
    public class Item
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Starting price")]
        public double InitialPrice { get; set; }
        [Required]
        [Display(Name = "Start date")]
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string SellerUser { get; set; }
        [Display(Name = "Highest offer")]
        public Bid WinningBid { get; set; }
        [Display(Name = "Previous offers")]
        public List<Bid> OldBid {get; set; }
    }
}