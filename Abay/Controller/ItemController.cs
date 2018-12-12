using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public int CreateItem(string name, double initialPrice, string token, int categoryId, string description, int duration)
        {  
            return itemDB.InsertItem(new Item(name, initialPrice, tokenCtrl.GetUserByToken(token), categoryCtrl.GetItemCategory(categoryId), description, duration));
        }

        public string DeleteItem(int id, string token)
        {
            User user = tokenCtrl.GetUserByToken(token);
            Item item = GetItemById(id);

            if (string.Equals(user.UserName , item.SellerUser.UserName))
                return itemDB.DeleteItem(id);
            else
                return "Ooops something went wrong.";
        }

        public void UpdateItem(int itemId, string token, string name, string description)
        {
            User user = tokenCtrl.GetUserByToken(token);
            Item item = GetItemById(itemId);
            if (string.Equals(user.UserName, item.SellerUser.UserName))
            {
                item.Name = name;
                item.Description = description;
                itemDB.UpdateItem(item);
            }
        }

        public bool UpdateItem(Item item)
        {
            return itemDB.UpdateItem(item);
        }

        public List<Item> SearchItems(string value, int categoryId)
        {
            return itemDB.SearchItems(value, categoryId);
        }

        public List<Item> GetAllItems(int catId)
        {
            List<Item> items = new List<Item>();
            List<Item> newLst = new List<Item>();

            items = itemDB.GetAllItems(catId);
            foreach (Item item in items)
            {
                if (DateTime.Now < item.EndDate)
                {
                    newLst.Add(item);
                }
                else
                {
                    item.State = 1;
                    UpdateItem(item);
                }
            }
            return newLst;
        }

        public Item GetItemById(int id)
        {
            Item item = itemDB.GetItemById(id);
            if (DateTime.Now > item.EndDate)
            {
                item.State = 1;
                UpdateItem(item);
            }
            return item;
        }
    }
}
