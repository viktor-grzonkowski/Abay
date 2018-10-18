using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Entities;

namespace ServiceLibrary.ServiceInterfaces
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IItemService" in both code and config file together.
    [ServiceContract]
    public interface IItemService
    {
        [OperationContract]
        List<Item> GetAllItems(int page, int quantity);

        [OperationContract]
        List<Item> SearchItems(string value, int categoryId);

        [OperationContract]
        void CreateItem(string name, double initialPrice, int state, string token, int CategoryId);

        [OperationContract]
        void UpdateItem(int itemId, string userToken, string name, string description);

        [OperationContract]
        string DeleteItem(int id, string token);

        [OperationContract]
        List<ItemCategory> GetCategories();
    }
}
