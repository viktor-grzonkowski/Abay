using AbayMVC.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbayMVC.Models
{
    public class AccountModel
    {
        public Account Find(string token)
        {
            //Get user with the token
            Account ac = new Account();

            UserServiceReference.User user = Services.Instance.UserClient().GetUserByToken(token);
            ac.Username = user.UserName;
            ac.Password = user.Password;
            ac.Token = token;

            if (user.Admin)
            {
                ac.Roles = new string[] { "Admin", "User" };
            }
            else
            {
                ac.Roles = new string[] { "User" };
            }

            return ac;
        }

        public Account Login(string username, string password)
        {
            UserServiceReference.User user = Services.Instance.UserClient().Login(username, password);
            if (string.IsNullOrEmpty(user.LoginToken.SecureToken))
                return null;

            return Find(user.LoginToken.SecureToken);
        }
    }
}