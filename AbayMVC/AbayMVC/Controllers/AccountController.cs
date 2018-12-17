using AbayMVC.Models;
using AbayMVC.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AbayMVC.Controllers
{
    public class AccountController : BaseController
    {

        // GET: Account
        [AllowAnonymous]
        public ActionResult Login()
        {

            return View();
        }

        [AllowAnonymous]
        public ActionResult Register()
        {

            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                /// -3 : Username already in use
                /// -2 : Password to short
                /// -1 : Username to short
                ///  0 : Account couldn't be created
                ///  1 : Account created
                switch (await Services.Instance.UserClient().CreateUserAsync(model.UserName, model.FirstName, model.LastName, model.Password, model.Email))
                {
                    case -3:
                        Warning("Username is already in use!");
                        return RedirectToAction("Register", "Account");
                    case -2:
                        Warning("Password is to short!");
                        return RedirectToAction("Register", "Account");
                    case -1:
                        Warning("Username is to short!");
                        return RedirectToAction("Register", "Account");
                    case 0:
                        Warning("Account couldn't be created!");
                        return RedirectToAction("Register", "Account");
                    case 1:
                        Success("Account created!");
                        return RedirectToAction("Index", "Home");
                }

                Danger("Ooops please try again!");
                return RedirectToAction("Register", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AccountViewModel avm)
        {
            AccountModel am = new AccountModel();
            Account ac;

            if (string.IsNullOrEmpty(avm.Account.Username) || string.IsNullOrEmpty(avm.Account.Password) || (ac = am.Login(avm.Account.Username, avm.Account.Password)) == null)
            {
                Danger("Account's Invalid");
                return View("Login");
            }

            SessionPersister.Token = ac.Token;
            SessionPersister.Username = ac.Username;

            Success("Welcome "+ SessionPersister.Username);
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            SessionPersister.Logout();
            return RedirectToAction("Index", "Home");
        }
    }
}