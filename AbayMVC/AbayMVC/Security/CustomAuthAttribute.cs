using AbayMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AbayMVC.Security
{
    public class CustomAuthAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (string.IsNullOrEmpty(SessionPersister.Token))
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { Controller = "Account", action = "Login" }));
            else
            {
                AccountModel am = new AccountModel();
                CustomPrincipal cp = new CustomPrincipal(am.Find(SessionPersister.Token));
                if (!cp.IsInRole(Roles))
                    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { Controller = "Home", action = "Index" }));
            }
        }
    }
}