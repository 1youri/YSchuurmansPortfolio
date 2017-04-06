using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using PCAuthLib;
using YouriPortfolio.Logic;
using YouriPortfolio.Models;
using YouriPortfolio.Models.ViewModels;
using YouriPortfolio.Repos;

namespace YouriPortfolio.Controllers
{
    public class ProjectsController : Controller
    {
        // GET: Project
        public ActionResult Index()
        {
            if (!Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return new HttpUnauthorizedResult();

            ProjectListViewModel viewModel = new ProjectListViewModel();

            List<Content> content = ContentRepo.GetAllContent();

            if (content != null)
            {
                foreach (Content contentItem in content)
                {
                    BBCode.ParseContent(contentItem);

                    contentItem.HeaderImg = VisualsRepo.RandomVisual(contentItem.ID);
                }
            }

            viewModel.ContentList = content ?? new List<Content>();

            return View(viewModel);
        }
        // GET: Project
        public ActionResult Get(int ID = 0)
        {
            if (!Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return new HttpUnauthorizedResult();

            ProjectViewModel viewModel = new ProjectViewModel();
            if (ID < 0) ID = ID * -1;
            Content content = ContentRepo.GetContent(ID);
            if (content != null)
            {
                content.Visuals = VisualsRepo.GetVisuals(content.ID);
                BBCode.ParseContent(content);
            }
            viewModel.Project = content;

            return View(viewModel);
        }

        public ActionResult Edit(int ID = 0)
        {
            if (!Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return new HttpUnauthorizedResult();
            var currentUser = Login.GetCurrentUser(System.Web.HttpContext.Current);
            if (currentUser.Permission < PCAuthLib.User.PermissionGroup.ADMIN) return RedirectToAction("Index","Login");

            ProjectViewModel viewModel = new ProjectViewModel();
            Content content = ContentRepo.GetContent(ID);
            if (content != null)
            {
                content.Visuals = VisualsRepo.GetVisuals(content.ID);
                viewModel.Project = content;
            }
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult Edit(ProjectViewModel viewModel)
        {
            if (!Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return new HttpUnauthorizedResult();
            var currentUser = Login.GetCurrentUser(System.Web.HttpContext.Current);
            if (currentUser.Permission < PCAuthLib.User.PermissionGroup.ADMIN) return RedirectToAction("Index", "Login");

            if (viewModel.Project.ID > 0)
            {
                Content project = viewModel.Project;
                ContentRepo.UpdateContent(project);
            }
            else if (viewModel.Project.ID == 0)
            {
                if (ContentRepo.InsertContent(viewModel.Project))
                {
                    viewModel.Project = ContentRepo.GetLastContent();
                }
            }

            return RedirectToAction("Edit", new RouteValueDictionary() {{"ID", viewModel.Project.ID}});
        }

        [HttpPost]
        public ActionResult UploadMultiple(IEnumerable<HttpPostedFileBase> files, ProjectViewModel viewModel)
        {
            if (!Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return new HttpUnauthorizedResult();
            var currentUser = Login.GetCurrentUser(System.Web.HttpContext.Current);
            if (currentUser.Permission < PCAuthLib.User.PermissionGroup.ADMIN) return RedirectToAction("Index", "Login");

            if (!Directory.Exists(Server.MapPath("/uploads")))
                Directory.CreateDirectory(Server.MapPath("/uploads"));
            foreach (var file in files)
            {
                if (file != null && file.ContentLength > 0)
                {
                    string filename = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    string path = Path.Combine(Server.MapPath("/uploads"), filename);
                    file.SaveAs(path);
                    VisualsRepo.InsertVisual(viewModel.Project.ID, filename, Visual.ContentTypes.Photo);
                }
            }
            return RedirectToAction("Edit", new RouteValueDictionary() { { "ID", viewModel.Project.ID } });
        }

        [HttpPost]
        public ActionResult NewVideo(ProjectViewModel viewModel)
        {
            if (!Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return new HttpUnauthorizedResult();
            var currentUser = Login.GetCurrentUser(System.Web.HttpContext.Current);
            if (currentUser.Permission < PCAuthLib.User.PermissionGroup.ADMIN) return RedirectToAction("Index", "Login");

            VisualsRepo.InsertVisual(viewModel.Project.ID, YoutubeIDExtract.ExtractVideoIdFromUri(new Uri(viewModel.PostVideo)), Visual.ContentTypes.Video);
            return RedirectToAction("Edit", new RouteValueDictionary() { { "ID", viewModel.Project.ID } });
        }

        public ActionResult DeleteProject(int ID = 0)
        {
            if (!Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return new HttpUnauthorizedResult();

            Content content = ContentRepo.GetContent(ID);
            ProjectViewModel viewModel = new ProjectViewModel();
            viewModel.Project = content;
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult DeleteProject(ProjectViewModel viewModel)
        {
            if (!Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return new HttpUnauthorizedResult();
            var currentUser = Login.GetCurrentUser(System.Web.HttpContext.Current);
            if (currentUser.Permission < PCAuthLib.User.PermissionGroup.ADMIN) return RedirectToAction("Index", "Login");

            if (viewModel == null) return RedirectToAction("Index");
            Content content = ContentRepo.GetContent(viewModel.Project.ID);
            if (content != null)
            {
                if (!string.IsNullOrEmpty(viewModel.DeleteConfirmationName) && viewModel.DeleteConfirmationName == content.Title)
                {
                    ContentRepo.DeleteContent(viewModel.Project.ID);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Error = "Entered name does not match Project name";
                    return View(viewModel);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteVisuals(ProjectViewModel viewModel)
        {
            if (!Login.ForceHTTPSConnection(System.Web.HttpContext.Current, true)) return new HttpUnauthorizedResult();
            var currentUser = Login.GetCurrentUser(System.Web.HttpContext.Current);
            if (currentUser.Permission < PCAuthLib.User.PermissionGroup.ADMIN) return RedirectToAction("Index", "Login");

            List<int> toRemoveIDs = new List<int>();

            foreach (Visual visual in viewModel.Project.Visuals)
            {
                if (visual.Selected)
                {
                    toRemoveIDs.Add(visual.ID);
                }
            }

            var succes = VisualsRepo.RemoveImages(toRemoveIDs.ToArray());

            if (succes)
            {
                foreach (var visual in viewModel.Project.Visuals)
                {
                    if (visual.Selected && visual.ContentType == Visual.ContentTypes.Photo)
                    {
                        string folder = Server.MapPath("/uploads");
                        string path = Path.Combine(folder, visual.Location);
                        System.IO.File.Delete(path);
                    }
                }
            }

            return RedirectToAction("Edit", new RouteValueDictionary() { { "ID", viewModel.Project.ID } });
        }
    }
}