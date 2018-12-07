using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbayMVC.Security
{
    public static class SessionPersister
    {
        static string tokenSession = "token";

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
    }
}