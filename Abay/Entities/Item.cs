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
        private double _finalPrice;
        private DateTime _startDate;
        private DateTime _endDate;
        private int _state;
        private User _sellerUsername;
        private User _buyerUsername;
        private ItemCategory _categoryId;

        public Item()
        { 
        }
        public Item(string name, double initialPrice, int state, User seller, ItemCategory categoryId)
        {
            Name = name;
            InitialPrice = initialPrice;
            State = state;
            SellerUsername = seller;
            CategoryId = categoryId;
        }

        public int Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }
        public string Description { get => _description; set => _description = value; }
        public double InitialPrice { get => _initialPrice; set => _initialPrice = value; }
        public double FinalPrice { get => _finalPrice; set => _finalPrice = value; }
        public DateTime StartDate { get => _startDate; set => _startDate = value; }
        public DateTime EndDate { get => _endDate; set => _endDate = value; }
        public int State { get => _state; set => _state = value; }
        public User SellerUsername { get => _sellerUsername; set => _sellerUsername = value; }
        public User BuyerUsername { get => _buyerUsername; set => _buyerUsername = value; }
        public ItemCategory CategoryId { get => _categoryId; set => _categoryId = value; }
        
    }
}
