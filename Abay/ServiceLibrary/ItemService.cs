using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Controller;
using Entities;
using Controller.ServiceInterfaces;

namespace Controller
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ItemService" in both code and config file together.
    public class ItemService : IItemService
    {
        ItemController ItemCtrl = new ItemController();
        CategoryController CategoryCtrl = new CategoryController();

        /// <summary>
        /// CreateItem returns the itemId from the database or -1 if it fails to create it
        /// </summary>
        public int CreateItem(string name, string description, double initialPrice, int CategoryId, string token, int duration)
        {
            return ItemCtrl.CreateItem(name, description, initialPrice, CategoryId, token, duration);
        }

        public string DeleteItem(int id, string token)
        {
            return ItemCtrl.DeleteItem(id, token);
        }
        public void UpdateItem(int itemId, string token, string name, string description, int catId)
        {
            ItemCtrl.UpdateItem(itemId, token, name, description, catId);
        }

        /// <summary>
        /// <param>
        ///     value can be item name, description or username
        /// </param>
        /// <param>
        ///     set category id to -1 to search in every category
        /// </param>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public List<Item> SearchItems(string value, int categoryId)
        {
            return ItemCtrl.SearchItems(value, categoryId);
        }

        /// <summary>
        /// Get the item by his id
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public Item GetItemById(int itemId)
        {
            return ItemCtrl.GetItemById(itemId);
        }

        /// <summary>
        /// Get all existing categorys
        /// </summary>
        /// <returns></returns>
        public List<ItemCategory> GetAllCategories()
        {
            return CategoryCtrl.GetAllCategories();
        }

        /// <summary>
        /// Provide -1 as category id to get all items
        /// </summary>
        /// <param name="catId"></param>
        /// <returns></returns>
        public List<Item> GetAllActiveItemsByCategory(int catId)
        {
            return ItemCtrl.GetAllActiveItemsByCategory(catId);
        }

        /// <summary>
        /// Get all items from the system ! HEAVY LOAD !
        /// </summary>
        /// <returns></returns>
        public List<Item> GetAllItems()
        {
            return ItemCtrl.GetAllItems();
        }
    }
}
