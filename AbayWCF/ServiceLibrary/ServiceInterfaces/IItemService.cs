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
        /// <summary>
        /// CreateItem returns the itemId from the database or -1 if it fails to create it
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="initialPrice"></param>
        /// <param name="CategoryId"></param>
        /// <param name="token"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        [OperationContract]
        int CreateItem(string name, string description, double initialPrice, int CategoryId, string token, int duration, string imagePath);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">itemId</param>
        /// <param name="token">User secure token</param>
        /// <returns>boolean</returns>
        [OperationContract]
        bool DeleteItem(int id, string token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="userToken"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="catId"></param>
        /// <returns>boolean</returns>
        [OperationContract]
        bool UpdateItem(int itemId, string userToken, string name, string description, int catId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">can be item name, description or username</param>
        /// <param name="categoryId">set to -1 to search in every category</param>
        /// <returns></returns>
        [OperationContract]
        List<Item> SearchItems(string value, int categoryId);

        /// <summary>
        /// Get the item by his id
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [OperationContract]
        Item GetItemById(int itemId);

        /// <summary>
        /// Get all existing categorys
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<ItemCategory> GetAllCategories();

        /// <summary>
        /// Provide -1 as category id to get all items
        /// </summary>
        /// <param name="catId"></param>
        /// <returns></returns>
        [OperationContract]
        List<Item> GetAllActiveItemsByCategory(int catId);


        /// <summary>
        /// Get all items from the system ! HEAVY LOAD !
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Item> GetAllItems();

        /// <summary>
        /// Provide an valid itemId to get all bids
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [OperationContract]
        List<Bid> GetAllBidsByItem(int itemId);
    }
}
