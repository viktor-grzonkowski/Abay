using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double InitialPrice { get; set; }
        public double FinalPrice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int State { get; set; }
        public User Seller_Username { get; set; }
        public User Buyer_Username { get; set; }


        public Item(string name, double initialPrice, int state, User seller)
        {
            this.Name = name;
            this.InitialPrice = initialPrice;
            this.State = state;
            this.Seller_Username = seller;
        }
    }

    public enum Category
    {
        electronics,
        fashion,
        collectiblesArt,
        homeGarden,
        sportingGoods,
        motors
    }
}
