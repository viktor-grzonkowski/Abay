using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Controller;
using Entities;

namespace ServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TokenService" in both code and config file together.
    public class TokenService : ITokenService
    {
        TokenController tokenCtrl = new TokenController();

        public User GetUserByToken(string token)
        {
            return tokenCtrl.GetUserByToken(token);
        }

        public bool IsValidUser(string token)
        {
            return tokenCtrl.IsValidUser(token);
        }
    }
}
