using System;
using Database;
using Entities;

namespace Controller
{
    public class UserController : ValidateInput
    {
        // TODO: Set this from a web.config appSettting value
        public static double DefaultSecondsUntilTokenExpires = 1800;

        DBUser userDb = new DBUser();
        TokenController tokenCtrl = new TokenController();
        public int CreateUser(User user)
        {
            if (!CheckString(user.UserName, 3))
                return -1;
            if (!CheckString(user.Password, 6))
                return -2;
            if (userDb.CheckUserName(user.UserName))
                return -3;
                
            if (userDb.InsertUser(user))
                return 1;

            return 0;
        }

        public User Login(string userName, string password)
        {
            if (userDb.CheckUserName(userName))
            {
                string salt = userDb.GetSalt(userName);
                string hashedPassword = Security.GetHashedPassword(password, salt);

                User user = userDb.Login(userName, hashedPassword);

                Token token = null;

                if (user != null)
                {
                    token = new Token
                    {
                        SecureToken = Security.CreateToken(100),
                        UserName = user.UserName,
                        CreateDate = DateTime.Now
                    };

                    user.LoginToken = token;

                    if (tokenCtrl.InsertToken(token))
                        return user;
                }
            }

            return null;
        }

        public User GetUserInformation(string userName)
        {
            if (userDb.CheckUserName(userName))
                return userDb.GetUserInformation(userName);
            return null;
        }
    }
}
