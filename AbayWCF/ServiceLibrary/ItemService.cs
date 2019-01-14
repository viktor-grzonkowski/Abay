using System.Collections.Generic;
using Entities;
using ServiceLibrary.ServiceInterfaces;
using Controller;

namespace ServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ItemService" in both code and config file together.
    public class ItemService : IItemService
    {
        ItemController ItemCtrl = new ItemController();
        BidController BidCtrl = new BidController();
        CategoryController CategoryCtrl = new CategoryController();

        public int CreateItem(string name, string description, double initialPrice, int categoryId, string token, int duration, string imagePath)
        {
            return ItemCtrl.CreateItem(name, description, initialPrice, categoryId, token, duration, imagePath);
        }

        public bool DeleteItem(int id, string token)
        {
            return ItemCtrl.DeleteItem(id, token);
        }
        public bool UpdateItem(int itemId, string token, string name, string description, int categoryId)
        {
            return ItemCtrl.UpdateItem(itemId, token, name, description, categoryId);
        }

        public List<Item> SearchItems(string value, int categoryId)
        {
            return ItemCtrl.SearchItems(value, categoryId);
        }

        public Item GetItemById(int itemId)
        {
            return ItemCtrl.GetItemById(itemId);
        }

        public List<ItemCategory> GetAllCategories()
        {
            return CategoryCtrl.GetAllCategories();
        }

        public List<Item> GetAllActiveItemsByCategory(int catId)
        {
            return ItemCtrl.GetAllActiveItemsByCategory(catId);
        }

        public List<Item> GetAllItems()
        {
            return ItemCtrl.GetAllItems();
        }

        public List<Bid> GetAllBidsByItem(int itemId)
        {
            return BidCtrl.GetAllBidsByItem(itemId);
        }
    }
}
