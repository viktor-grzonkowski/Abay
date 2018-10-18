using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Controller;
using Entities;
using ServiceLibrary.ServiceInterfaces;

namespace ServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UserLogin" in both code and config file together.
    public class UserLogin : IUserLogin
    {
        UserController userCtrl = new UserController();

        public User GetUserByToken(string token)
        {
            return userCtrl.GetUserByToken(token);
        }

        public bool Login(string userName, string password)
        {
            return userCtrl.Login(userName, password);
        }
    }
}
