using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using PCAuthLib;
using YouriPortfolio.Logic;
using YouriPortfolio.Models;
using YouriPortfolio.Repos;

namespace YouriPortfolio.Controllers
{
    public class CVController : Controller
    {
        // GET: CV
        public ActionResult Index()
        {
            if (!Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return new EmptyResult();

            CVViewModel viewModel = ContentRepo.GetCV();
            if(viewModel == null) return View(new CVViewModel());

            viewModel.ExperienceText = BBCode.ParseBBCode(viewModel.ExperienceText);
            viewModel.AboutMeText = BBCode.ParseBBCode(viewModel.AboutMeText);
            viewModel.EducationText = BBCode.ParseBBCode(viewModel.EducationText);
            return View(viewModel);
        }

        public ActionResult Edit()
        {
            if (!Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return new EmptyResult();
            var currentUser = Login.GetCurrentUser(System.Web.HttpContext.Current);
            if (currentUser.Permission < PCAuthLib.User.PermissionGroup.ADMIN) return RedirectToAction("Index", "Login");

            CVViewModel viewModel = ContentRepo.GetCV();
            if (viewModel == null) viewModel = new CVViewModel();

            if (viewModel.SkillKeyValues.Count > 0)
            {
                viewModel.SkillKeyValuesJson = JsonConvert.SerializeObject(viewModel.SkillKeyValues, Formatting.Indented);
            }
            else
            {
                Dictionary<string,int> toJson = new Dictionary<string, int>();
                toJson.Add("C#", 0);
                toJson.Add("ASP.net", 0);
                viewModel.SkillKeyValuesJson = JsonConvert.SerializeObject(toJson, Formatting.Indented);
            }
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(CVViewModel viewModel)
        {
            if (!Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return new EmptyResult();
            var currentUser = Login.GetCurrentUser(System.Web.HttpContext.Current);
            if (currentUser.Permission < PCAuthLib.User.PermissionGroup.ADMIN) return RedirectToAction("Index", "Login");


            if (viewModel != null)
            {
                viewModel.SkillKeyValues =
                    JsonConvert.DeserializeObject<Dictionary<string, int>>(viewModel.SkillKeyValuesJson);
                ContentRepo.UpdateCV(JsonConvert.SerializeObject(viewModel));
            }
            return RedirectToAction("Index");
        }
    }
}