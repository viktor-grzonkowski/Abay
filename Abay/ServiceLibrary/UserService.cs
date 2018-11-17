using Controller;
using Entities;
using ServiceLibrary.ServiceInterfaces;

namespace ServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UserLogin" in both code and config file together.
    public class UserService : IUserService
    {
        UserController userCtrl = new UserController();

        //Method to validate user
        public string Login(string userName, string password)
        {
            Token token = userCtrl.Login(userName, password);
            if(token != null)
            {
                return token.SecureToken;
            }

            return "";
        }

        public User CreateUser(User user, out string message)
        {
            /*
            User user = new User()
            {
                UserName = userName,
                FirstName = fName,
                LastName = lName,
                Password = pw,
                Email = email,
                Admin = admin
            };
            */
            User insertedUser = userCtrl.CreateUser(user, out string messa);
            message = messa;

            return insertedUser;
        }
    }
}
