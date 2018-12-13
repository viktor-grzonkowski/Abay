using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Bid
    {
        private int _id;
        private string _userName;
        private double _amount;
        private DateTime _timestamp;

        public Bid()
        {

        }

        public int Id { get => _id; set => _id = value; }
        public string UserName { get => _userName; set => _userName = value; }
        public double Amount { get => _amount; set => _amount = value; }
        public DateTime Timestamp { get => _timestamp; set => _timestamp = value; }
        
    }
}
