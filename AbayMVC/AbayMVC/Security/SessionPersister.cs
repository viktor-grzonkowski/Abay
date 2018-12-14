using AbayMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbayMVC.Security
{
    public static class SessionPersister
    {
        static string tokenSession = "tk";
        static string usernameSession = "us";

        public static string Token
        {
            get
            {
                if (HttpContext.Current == null)
                    return string.Empty;

                var sessionVar = HttpContext.Current.Session[tokenSession];

                if (sessionVar != null)
                    return sessionVar as string;

                return null;
            }

            set
            {
                HttpContext.Current.Session[tokenSession] = value;
            }
        }

        public static string Username
        {
            get
            {
                if (HttpContext.Current == null)
                    return string.Empty;

                var sessionVar = HttpContext.Current.Session[usernameSession];

                if (sessionVar != null)
                    return sessionVar as string;

                return null;
            }

            set
            {
                HttpContext.Current.Session[usernameSession] = value;
            }
        }

        public static void Logout()
        {
            Token = string.Empty;
            Username = string.Empty;
            new BuyItemView().ServiceItem = null;
            new BuyItemView().WebItem = null;
        }
    }
}