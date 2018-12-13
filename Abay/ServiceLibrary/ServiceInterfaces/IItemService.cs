using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Entities;

namespace Controller.ServiceInterfaces
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IItemService" in both code and config file together.
    [ServiceContract]
    public interface IItemService
    {
        [OperationContract]
        List<Item> GetAllItems(int catId);

        [OperationContract]
        Item GetItemById(int itemId);

        [OperationContract]
        List<Item> SearchItems(string value, int categoryId);

        [OperationContract]
        int CreateItem(string name, string description, double initialPrice, int CategoryId, string token, int duration);

        [OperationContract]
        void UpdateItem(int itemId, string userToken, string name, string description);

        [OperationContract]
        string DeleteItem(int id, string token);

        [OperationContract]
        List<ItemCategory> GetCategories();
    }
}
