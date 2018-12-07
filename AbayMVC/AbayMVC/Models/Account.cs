using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AbayMVC.Models
{
    public class Account
    {
        [Display(Name = "Username")]
        public string Username
        {
            get;
            set;
        }
        [Display(Name = "Password")]
        public string Password
        {
            get;
            set;
        }

        public string Token
        {
            get;
            set;
        }

        public string[] Roles
        {
            get;
            set;
        }
    }
}