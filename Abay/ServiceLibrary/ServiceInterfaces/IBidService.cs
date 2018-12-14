using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Controller.ServiceInterfaces
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IBidService" in both code and config file together.
    [ServiceContract]
    public interface IBidService
    {
        /// <summary>
        /// Heavy shit is going on here
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="amount"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [OperationContract]
        bool BidOnItem(int itemId, double amount, string token);
    }
}
