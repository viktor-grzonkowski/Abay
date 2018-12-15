using System;
using System.Collections.Generic;
using Database;
using Entities;

namespace Controller
{
    public class ItemController
    {
        DBItem itemDB = new DBItem();
        UserController userCtrl = new UserController();
        CategoryController categoryCtrl = new CategoryController();
        TokenController tokenCtrl = new TokenController();

        public int CreateItem(string name, string description, double initialPrice, int categoryId, string token, int duration, string imagePath)
        {  
            return itemDB.InsertItem(new Item(name, initialPrice, tokenCtrl.GetUserByToken(token), categoryCtrl.GetItemCategory(categoryId), description, duration, imagePath));
        }

        public bool DeleteItem(int id, string token)
        {
            User user = tokenCtrl.GetUserByToken(token);
            Item item = GetItemById(id);

            return string.Equals(user.UserName , item.SellerUser.UserName) || user.Admin ? itemDB.DeleteItem(id) : false;
        }

        public bool UpdateItem(int itemId, string token, string name, string description, int catId)
        {
            User user = tokenCtrl.GetUserByToken(token);
            Item item = GetItemById(itemId);
            if (string.Equals(user.UserName, item.SellerUser.UserName) || user.Admin)
            {
                item.Name = name;
                item.Description = description;
                item.Category = categoryCtrl.GetItemCategory(catId);
                return itemDB.UpdateItem(item);
            }
            return false;
        }

        public List<Item> SearchItems(string value, int categoryId)
        {
            return itemDB.SearchItems(value, categoryId);
        }

        public List<Item> GetAllActiveItemsByCategory(int catId)
        {
            List<Item> items = new List<Item>();
            List<Item> newLst = new List<Item>();

            items = itemDB.GetAllActiveItemsByCategory(catId);

            foreach (Item item in items)
            {
                if (DateTime.Now < item.EndDate )
                {
                    newLst.Add(item);
                }
                else
                {
                    item.State = 1;
                    itemDB.UpdateItem(item);
                }
            }
            return newLst;
        }

        public Item GetItemById(int id)
        {
            Item item = itemDB.GetItemById(id);
            if (item != null)
            {
                if (DateTime.Now > item.EndDate)
                {
                    item.State = 1;
                    itemDB.UpdateItem(item);
                }
            }
            return item;
        }

        public List<Item> GetAllItems()
        {
            return itemDB.GetAllItems();
        }
    }
}
