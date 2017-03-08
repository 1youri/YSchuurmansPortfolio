using System.Collections.Generic;
using System.Web.Mvc;
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
                }
            }

            viewModel.ContentList = content ?? new List<Content>();

            return View(viewModel);
        }
        // GET: Project
        public ActionResult Get(int ID = 0)
        {

            ProjectViewModel viewModel = new ProjectViewModel();
            if (ID < 0) ID = ID*-1;
            Content content = ContentRepo.GetContent(ID);
            if (content != null)
            {
                BBCode.ParseContent(content);
            }
            viewModel.Project = content;

            return View(viewModel);
        }

        public ActionResult Edit(int ID = 0)
        {
            return View();
        }
    }
}