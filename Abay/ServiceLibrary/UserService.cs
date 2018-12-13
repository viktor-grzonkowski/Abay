using Controller;
using Entities;
using Controller.ServiceInterfaces;

namespace Controller
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UserLogin" in both code and config file together.
    public class UserService : IUserService
    {
        TokenController TokenCtrl = new TokenController();
        UserController UserCtrl = new UserController();

        /// <summary>
        /// <para>
        /// Validates the user and returns a token
        /// </para>
        /// <para>
        /// Returns an empty string if thecredentials are wrong
        /// </para>
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string Login(string userName, string password)
        {
            Token token = UserCtrl.Login(userName, password);
            return token != null ? token.SecureToken : "";
        }

        /// <summary>
        /// CreateUser return type:
        /// -3 : Username already in use
        /// -2 : Password to short
        /// -1 : Username to short
        ///  0 : Account couldn't be created
        ///  1 : Account created
        /// </summary>
        public int CreateUser(string userName, string firstName, string lastName, string password, string email)
        {
            return UserCtrl.CreateUser(new User(userName, firstName, lastName, password, email));
        }

        /// <summary>
        /// Gets the user details
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public User GetUserByToken(string token)
        {
            return TokenCtrl.GetUserByToken(token);
        }

        /// <summary>
        /// Checks if the token has expired
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool CheckTokenTime(string token)
        {
            return TokenCtrl.CheckTokenTime(token);
        }
    }
}
