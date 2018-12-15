using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class User : IEquatable<User>
    {
        private string _userName;
        private string _firstName;
        private string _lastName;
        private string _email;
        private bool _admin;
        private string _password;
        private Token _loginToken;

        public User()
        {

        }

        public User(string username, string firstname, string lastname, string passwrod, string email)
        {
            UserName = username;
            FirstName = firstname;
            LastName = lastname;
            Password = passwrod;
            Email = email;
        }

        public string UserName { get => _userName; set => _userName = value; }
        public string FirstName { get => _firstName; set => _firstName = value; }
        public string LastName { get => _lastName; set => _lastName = value; }
        public string Email { get => _email; set => _email = value; }
        public bool Admin { get => _admin; set => _admin = value; }
        public string Password { get => _password; set => _password = value; }
        public Token LoginToken { get => _loginToken; set => _loginToken = value; }

        public bool Equals(User other)
        {
            return (this._userName.Equals(other._userName) &&
                this._firstName.Equals(other._firstName) &&
                this._lastName.Equals(other._lastName) &&
                this._email.Equals(other._email) &&
                this._admin == other._admin &&
                this._loginToken.Equals(other._loginToken));
        }
        public override int GetHashCode()
        {
            return _userName.GetHashCode() ^ _firstName.GetHashCode() ^ _lastName.GetHashCode() ^ _email.GetHashCode() ^
                _admin.GetHashCode() ^ _loginToken.GetHashCode();
        }
    }
}
