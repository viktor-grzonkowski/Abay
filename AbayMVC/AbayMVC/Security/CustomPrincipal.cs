using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using AbayMVC.Models;

namespace AbayMVC.Security
{
    public class CustomPrincipal : IPrincipal
    {
        private Account Account;

        public CustomPrincipal(Account account)
        {
            this.Account = account;
            this.Identity = new GenericIdentity(account.Token);
        }

        public IIdentity Identity
        {
            get;
            set;
        }

        public bool IsInRole(string role)
        {
            var roles = role.Split(new char[] { ',' });
            return roles.Any(r => this.Account.Roles.Contains(r));
        }
    }
}