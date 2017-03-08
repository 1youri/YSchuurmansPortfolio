using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YouriPortfolio.Models
{
    public class Content
    {
        public string Title { get; set; }
        public string ShortContent { get; set; }
        public string ContentText { get; set; }
        public string HeaderIMG { get; set; }
        public List<string> Images { get; set; }

    }
}