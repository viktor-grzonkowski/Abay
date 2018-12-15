using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Token : IEquatable<Token>
    {
        private string _secureToken;
        private string _userName;
        private DateTime _createDate;

        public Token()
        {

        }

        public string SecureToken { get => _secureToken; set => _secureToken = value; }
        public string UserName { get => _userName; set => _userName = value; }
        public DateTime CreateDate { get => _createDate; set => _createDate = value; }

        public bool Equals(Token other)
        {
            return (this._secureToken.Equals(other._secureToken) &&
                this._userName.Equals(other._userName) &&
                this._createDate.Equals(other._createDate));
        }
        public override int GetHashCode()
        {
            return _secureToken.GetHashCode() ^ _userName.GetHashCode() ^ _createDate.GetHashCode();
        }
    }
}
