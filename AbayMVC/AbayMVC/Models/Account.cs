using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AbayMVC.Models
{
    public class Account
    {
        [Required]
        [Display(Name = "Username")]
        [DataType(DataType.Text)]
        public string Username
        {
            get;
            set;
        }
        [Required]
        [DataType(DataType.Password)]
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