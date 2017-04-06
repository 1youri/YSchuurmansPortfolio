using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YouriPortfolio.Models
{
    public class Content
    {
        public Content()
        {
            Visuals = new List<Visual>();
        }

        public Content(string id, string title, string shortContent, string contentText)
        {
            ID = id.ToInt();
            Title = title;
            ShortContent = shortContent;
            ContentText = contentText;
            Visuals = new List<Visual>();
        }

        public int ID { get; set; }
        public string Title { get; set; }
        public string ShortContent { get; set; }
        public string ContentText { get; set; }
        public Visual HeaderImg { get; set; }
        public List<Visual> Visuals { get; set; }

    }
}