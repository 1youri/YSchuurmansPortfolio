using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
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
            Content project = viewModel.Project;

            ContentRepo.UpdateContent(project);

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult UploadMultiple(IEnumerable<HttpPostedFileBase> files, ProjectViewModel viewModel)
        {
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
            VisualsRepo.InsertVisual(viewModel.Project.ID, YoutubeIDExtract.ExtractVideoIdFromUri(new Uri(viewModel.PostVideo)), Visual.ContentTypes.Video);
            return RedirectToAction("Edit", new RouteValueDictionary() { { "ID", viewModel.Project.ID } });
        }

        [HttpPost]
        public ActionResult DeleteVisuals(ProjectViewModel viewModel)
        {
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