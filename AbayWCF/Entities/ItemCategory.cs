using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class ItemCategory : IEquatable<ItemCategory>
    {
        private int _id;
        private string _name;

        public ItemCategory()
        {
        }

        public ItemCategory(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }
        public override string ToString()
        {
            return Name;
        }
        public bool Equals(ItemCategory other)
        {
            return (this._id == other._id &&
                this._name.Equals(other._name));
        }
        public override int GetHashCode()
        {
            return _id.GetHashCode() ^ _name.GetHashCode();
        }
    }
}
