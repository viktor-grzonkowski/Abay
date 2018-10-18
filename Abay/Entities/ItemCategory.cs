using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class ItemCategory
    {
        private int _Id;
        private string _name;

        public ItemCategory()
        {
        }

        public ItemCategory(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get => _Id; set => _Id = value; }
        public string Name { get => _name; set => _name = value; }
        public override string ToString()
        {
            return Name;
        }
    }
}
