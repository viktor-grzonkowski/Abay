using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Token
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
    }
}
