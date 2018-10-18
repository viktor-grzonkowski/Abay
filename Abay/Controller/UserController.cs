using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;
using Entities;

namespace Controller
{
    public class UserController
    {
        DBUser userDb = new DBUser();

        public bool Login(string userName, string password)
        {
            return userDb.Login(userName, password);
        }

        public User GetUserByToken(string token)
        {
            return userDb.GetUserByToken(token);
        }
    }
}
