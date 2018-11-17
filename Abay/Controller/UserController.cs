using System;
using Database;
using Entities;

namespace Controller
{
    public class UserController
    {
        // TODO: Set this from a web.config appSettting value
        public static double DefaultSecondsUntilTokenExpires = 1800;

        DBUser userDb = new DBUser();
        TokenController tokenCtrl = new TokenController();

        public Token Login(string userName, string password)
        {
            User user = userDb.Login(userName, password);
            Token token = null;

            if (user != null)
            {
                token = new Token
                {
                    SecureToken = Security.CreateToken(100),
                    UserName = user.UserName,
                    CreateDate = DateTime.Now
                };

                if (tokenCtrl.InsertToken(token))
                    return token;
                
            }

            return null;
        }

        public User CreateUser(User user, out string message)
        {
            if (userDb.CheckUserName(user.UserName))
            {
                message = "User name "+ user.UserName +" is already taken";
                return null;
            }


            if (userDb.InsertUser(user))
            {
                message = "User created!";
                return user;
            }

            message = "Ooops something bad happened";
            return null;
        }
    }
}
