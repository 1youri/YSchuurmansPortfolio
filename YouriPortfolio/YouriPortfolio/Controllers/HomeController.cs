using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PCAuthLib;
using PCDataDLL;
using PermacallWebApp.Repos;

namespace YouriPortfolio.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            LogRepo.Log(System.Web.HttpContext.Current);
            return RedirectToAction("Index", "CV");
        }
    }
}