using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Item : IEquatable<Item>
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
        private string _imagePath;

        public Item()
        { 

        }

        public Item(string name, double initialPrice, User seller, ItemCategory categoryId, string description, int duration, string imagePath)
        {
            Name = name;
            InitialPrice = initialPrice;
            State = 0;
            SellerUser = seller;
            Category = categoryId;
            Description = description;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now.AddDays(duration);
            ImagePath = imagePath;
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
        public string ImagePath { get => _imagePath; set => _imagePath = value; }

        public bool Equals(Item other)
        {
            return (this._category.Equals(other._category) &&
                this._description.Equals(other._description) &&
                this._endDate.Equals(other._endDate) &&
                this._id == other._id &&
                this._initialPrice == other._initialPrice &&
                this._name.Equals(other._name) &&
                this._sellerUser.UserName.Equals(other._sellerUser.UserName) &&
                this._startDate.Equals(other._startDate) &&
                this._state == other._state);
    }
        public override int GetHashCode()
        {
            return _category.GetHashCode() ^ _description.GetHashCode() ^ _endDate.GetHashCode() ^ _id.GetHashCode() ^
                _initialPrice.GetHashCode() ^ _name.GetHashCode() ^ _sellerUser.GetHashCode() ^ _startDate.GetHashCode() ^
                _state.GetHashCode();
        }
    }
}
