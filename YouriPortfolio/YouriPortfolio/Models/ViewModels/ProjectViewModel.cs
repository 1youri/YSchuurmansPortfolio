using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YouriPortfolio.Models.ViewModels
{
    public class ProjectViewModel
    {
        public ProjectViewModel()
        {
            Project = new Content();
        }
        public Content Project { get; set; }
        public string PostVideo { get; set; }
    }
}