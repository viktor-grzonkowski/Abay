using Controller;
using Entities;
using ServiceLibrary.ServiceInterfaces;

namespace ServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UserLogin" in both code and config file together.
    public class UserService : IUserService
    {
        TokenController TokenCtrl = new TokenController();
        UserController UserCtrl = new UserController();
        ValidateInput Validate = new ValidateInput();

        //Method to validate user
        public string Login(string userName, string password)
        {
            Token token = UserCtrl.Login(userName, password);
            return token != null ? token.SecureToken : "";
        }

        /// <summary>
        /// CreateUser return type:
        /// -2 : Username already in use
        /// -1 : Username to short
        ///  0 : Account couldn't be created
        ///  1 : Account created
        /// </summary>
        public int CreateUser(string userName, string firstName, string lastName, string password, string email)
        {
            return !Validate.CheckString(userName,3) ? -1 : UserCtrl.CreateUser(new User(userName, firstName, lastName, password, email));
        }

        public User GetUserByToken(string token)
        {
            return TokenCtrl.GetUserByToken(token);
        }

        public bool IsValidUser(string token)
        {
            return TokenCtrl.IsValidUser(token);
        }

        public bool CheckToken(string token)
        {
            return TokenCtrl.CheckTokenTime(token);
        }
    }
}
