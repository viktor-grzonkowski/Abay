using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Controller.ServiceInterfaces
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IUserLogin" in both code and config file together.
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        User Login(string userName, string password);
        
        [OperationContract]
        int CreateUser(string userName, string firstName, string lastName, string password, string email);

        //string userName, string fName, string lName, string pw, string email, bool admin,
        [OperationContract]
        User GetUserByToken(string token);

        [OperationContract]
        bool CheckTokenTime(string token);
    }
}
