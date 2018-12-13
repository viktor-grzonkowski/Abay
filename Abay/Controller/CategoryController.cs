using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Database;

namespace Controller
{
    public class CategoryController : ValidateInput
    {
        public ItemCategory GetItemCategory(int id)
        {
            if (!CheckInt(id))
                return null;
            return DBCategory.GetItemCategory(id);
        }

        public List<ItemCategory> GetAllCategories()
        {
            return DBCategory.GetAllCategories();
        }
    }
}
