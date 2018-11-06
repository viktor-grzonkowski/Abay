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

        public void CreateItem(string name, double initialPrice, int state, string token, int CategoryId)
        {
            ItemCtrl.CreateItem(name, initialPrice, state, token, CategoryId);
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

        public List<ItemCategory> GetCategories()
        {
            return CategoryCtrl.GetAllCategories();
        }
    }
}
