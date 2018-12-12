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
        private static readonly int EXPIRYTIME = 30;

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
            if (tokenDb.UserExisting(token.UserName))
                tokenDb.DeleteToken(token.UserName);

            return tokenDb.InsertToken(token);
        }

        public Token GetToken(string token)
        {
            return tokenDb.GetToken(token);
        }

        public bool CheckTokenTime(string token)
        {
            Token tokenObj = GetToken(token);

            DateTime created = tokenObj.CreateDate;

            if (DateTime.Now > created.AddMinutes(EXPIRYTIME))
            {
                tokenDb.DeleteToken(tokenObj.UserName);
                return false;
            }   

            return true;
        }
    }
}
