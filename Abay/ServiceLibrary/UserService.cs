using Entities;
using ServiceLibrary.ServiceInterfaces;
using Controller;

namespace ServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UserLogin" in both code and config file together.
    public class UserService : IUserService
    {
        TokenController TokenCtrl = new TokenController();
        UserController UserCtrl = new UserController();

        
        public User Login(string userName, string password)
        {
            return UserCtrl.Login(userName, password);
        }
  
        public int CreateUser(string userName, string firstName, string lastName, string password, string email)
        {
            return UserCtrl.CreateUser(new User(userName, firstName, lastName, password, email));
        }

        public User GetUserByToken(string token)
        {
            return TokenCtrl.GetUserByToken(token);
        }
        
        public bool CheckTokenTime(string token)
        {
            return TokenCtrl.CheckTokenTime(token);
        }
    }
}
