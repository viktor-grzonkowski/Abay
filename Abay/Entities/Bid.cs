using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Bid
    {
        private string _userName;
        private int _itemId;
        private double _amount;
        private DateTime _timestamp;

        public Bid()
        {

        }

        public string UserName { get => _userName; set => _userName = value; }
        public int ItemId { get => _itemId; set => _itemId = value; }
        public double Amount { get => _amount; set => _amount = value; }
        public DateTime Timestamp { get => _timestamp; set => _timestamp = value; }
    }
}
