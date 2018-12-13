using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Item
    {
        private int _id;
        private string _name;
        private string _description;
        private double _initialPrice;
        private DateTime _startDate;
        private DateTime _endDate;
        private int _state;
        private User _sellerUser;
        private ItemCategory _category;
        private Bid _winningBid;
        private List<Bid> _oldBids;

        public Item()
        { 

        }

        public Item(string name, double initialPrice, User seller, ItemCategory categoryId, string description, int duration)
        {
            Name = name;
            InitialPrice = initialPrice;
            State = 0;
            SellerUser = seller;
            Category = categoryId;
            Description = description;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now.AddDays(duration);
        }

        public int Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }
        public string Description { get => _description; set => _description = value; }
        public double InitialPrice { get => _initialPrice; set => _initialPrice = value; }
        public DateTime StartDate { get => _startDate; set => _startDate = value; }
        public DateTime EndDate { get => _endDate; set => _endDate = value; }
        public int State { get => _state; set => _state = value; }
        public User SellerUser { get => _sellerUser; set => _sellerUser = value; }
        public ItemCategory Category { get => _category; set => _category = value; }
        public Bid WinningBid { get => _winningBid; set => _winningBid = value; }
        public List<Bid> OldBids { get => _oldBids; set => _oldBids = value; }
    }
}
