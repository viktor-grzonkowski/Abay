using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Controller;
using Entities;
using ServiceLibrary.ServiceInterfaces;

namespace ServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ItemService" in both code and config file together.
    public class ItemService : IItemService
    {
        ItemController ItemCtrl = new ItemController();
        CategoryController CategoryCtrl = new CategoryController();
        ValidateInput Validate = new ValidateInput();

        /// <summary>
        /// CreateItem returns the itemId from the database or -1 if it fails to create it
        /// </summary>
        public int CreateItem(string name, string description, double initialPrice, int CategoryId, string token, int duration)
        {
            return !Validate.CheckDouble(initialPrice) || !Validate.CheckString(name, 3) || !Validate.CheckInt(CategoryId)
                ? -1
                : ItemCtrl.CreateItem(name, description, initialPrice, CategoryId, token, duration);
        }

        public string DeleteItem(int id, string token)
        {
            return ItemCtrl.DeleteItem(id, token);
        }
        public void UpdateItem(int itemId, string token, string name, string description)
        {
            ItemCtrl.UpdateItem(itemId, token, name, description);
        }

        public List<Item> SearchItems(string value, int categoryId)
        {
            return ItemCtrl.SearchItems(value, categoryId);
        }

        public List<Item> GetAllItems(int catId)
        {
            return ItemCtrl.GetAllItems(catId);
        }
        public Item GetItemById(int itemId)
        {
            return ItemCtrl.GetItemById(itemId);
        }

        public List<ItemCategory> GetCategories()
        {
            return CategoryCtrl.GetAllCategories();
        }
    }
}
