using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ServiceLibrary.ServiceInterfaces
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IUserLogin" in both code and config file together.
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        string Login(string userName, string password);
        
        [OperationContract]
        User CreateUser(User user, out string message);

        //string userName, string fName, string lName, string pw, string email, bool admin,
    }
}
