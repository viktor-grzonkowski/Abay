using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Bid : IEquatable<Bid>
    {
        private string _buyerName;
        private int _itemId;
        private double _amount;
        private DateTime _timestamp;
        private bool _winning;

        public Bid()
        {

        }

        public string BuyerName { get => _buyerName; set => _buyerName = value; }
        public int ItemId { get => _itemId; set => _itemId = value; }
        public double Amount { get => _amount; set => _amount = value; }
        public DateTime Timestamp { get => _timestamp; set => _timestamp = value; }
        public bool Winning { get => _winning; set => _winning = value; }

        public bool Equals(Bid other)
        {
            return (this._buyerName.Equals(other._buyerName) &&
                this._itemId == other._itemId &&
                this._amount == other._amount &&
                this._timestamp.Equals(other._timestamp) &&
                this._winning == other._winning);
        }
        public override int GetHashCode()
        {
            return _buyerName.GetHashCode() ^ _itemId.GetHashCode() ^ _amount.GetHashCode() ^
                _timestamp.GetHashCode() ^ _winning.GetHashCode();
        }
    }
}
