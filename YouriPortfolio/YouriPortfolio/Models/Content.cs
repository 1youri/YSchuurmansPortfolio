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
        }

        public Content(string id, string title, string shortContent, string contentText, string headerIMG)
        {
            ID = id.ToInt();
            Title = title;
            ShortContent = shortContent;
            ContentText = contentText;
            HeaderIMG = headerIMG;
        }

        public int ID { get; set; }
        public string Title { get; set; }
        public string ShortContent { get; set; }
        public string ContentText { get; set; }
        public string HeaderIMG { get; set; }
        public List<Image> Images { get; set; }

    }
}