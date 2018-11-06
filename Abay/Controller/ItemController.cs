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
        CategoryController CategoryCtrl = new CategoryController();

        public string CreateItem(string name, double initialPrice, int state, string token, int categoryId)
        {  
            return itemDB.InsertItem(new Item(name, initialPrice, state, userCtrl.GetUserByToken(token), CategoryCtrl.GetItemCategory(categoryId)));
        }

        public string DeleteItem(int id, string token)
        {
            User user = userCtrl.GetUserByToken(token);
            Item item = GetItemById(id);

            if (string.Equals(user.UserName , item.SellerUser.UserName))
                return itemDB.DeleteItem(id);
            else
                return "Ooops something went wrong.";
        }

        public void UpdateItem(int itemId, string token, string name, string description)
        {
            User user = userCtrl.GetUserByToken(token);
            Item item = GetItemById(itemId);
            if (string.Equals(user.UserName, item.SellerUser.UserName))
            {
                item.Name = name;
                item.Description = description;
                itemDB.UpdateItem(item);
            }
        }

        public List<Item> SearchItems(string value, int categoryId)
        {
            return itemDB.SearchItems(value, categoryId);
        }

        public List<Item> GetAllItems(int catId)
        {
            return itemDB.GetAllItems(catId);
        }

        public Item GetItemById(int id)
        {
            return itemDB.GetItemById(id);
        }
    }
}
