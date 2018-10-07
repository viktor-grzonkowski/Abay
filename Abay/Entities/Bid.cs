using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    class Bid
    {
        public User Username { get; set; }
        public Item Item { get; set; }
        public double Amount { get; set; }
        public DateTime Timestamp { get; set; }

        public Bid()
        {

        }
    }
}
