using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PCAuthLib;
using YouriPortfolio.Models;

namespace YouriPortfolio.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            if (!Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return new EmptyResult();

            Account viewModel = new Account();

            User currentUser = Login.GetCurrentUser(System.Web.HttpContext.Current);
            if (currentUser.ID > 0)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(viewModel);
        }

        //POST: Login
        [HttpPost]
        public ActionResult Index(Account account)
        {
            if (!Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return new EmptyResult();

            if (Login.GetCurrentUser(System.Web.HttpContext.Current).ID > 0) return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                var loginRe = Login.AuthorizeUser(System.Web.HttpContext.Current, account.Username, account.Password);
                if (loginRe.Item1)
                {
                    User currentUser = Login.GetCurrentUser(System.Web.HttpContext.Current);
                    if (currentUser.ID > 0)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                account.ErrorMessage = loginRe.Item2;
            }


            return View(account);
        }

        public ActionResult Logout()
        {
            if (!Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return new EmptyResult();

            Account viewModel = new Account();

            User currentUser = Login.GetCurrentUser(System.Web.HttpContext.Current);
            if (currentUser.ID > 0)
            {
                Login.Logout(System.Web.HttpContext.Current, Request.UserHostAddress);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}