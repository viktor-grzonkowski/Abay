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
        /// <summary>
        /// Validates the user and returns a usr object with his token inside
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password">Returns null if credentials are wrong</param>
        /// <returns></returns>
        [OperationContract]
        User Login(string userName, string password);

        /// <summary>
        /// CreateUser return type:
        /// -3 : Username already in use
        /// -2 : Password to short
        /// -1 : Username to short
        ///  0 : Account couldn't be created
        ///  1 : Account created
        /// </summary>
        [OperationContract]
        int CreateUser(string userName, string firstName, string lastName, string password, string email);

        /// <summary>
        /// Gets the user details
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [OperationContract]
        User GetUserByToken(string token);

        /// <summary>
        /// Checks if the token has expired
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [OperationContract]
        bool CheckTokenTime(string token);
    }
}
