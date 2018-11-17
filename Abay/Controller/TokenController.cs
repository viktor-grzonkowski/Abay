using Entities;
using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public class TokenController
    {
        DBToken tokenDb = new DBToken();

        public User GetUserByToken(string token)
        {
            return tokenDb.GetUserByToken(token);
        }

        public bool IsValidUser(string token)
        {
            return tokenDb.GetToken(token) != null ? true : false;
        }

        public bool InsertToken(Token token)
        {
            return tokenDb.InsertToken(token);
        }
    }
}
