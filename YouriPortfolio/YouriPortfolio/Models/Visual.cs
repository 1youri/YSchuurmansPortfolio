using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YouriPortfolio.Models
{
    public class Visual
    {
        public Visual() { }
        public Visual(int id, string location, ContentTypes contentType)
        {
            ID = id;
            Location = location;
            Selected = false;
            ContentType = contentType;
        }

        public enum ContentTypes
        {
            Photo = 0 ,
            Video = 1
        }

        public int ID { get; set; }
        public string Location { get; set; }
        public bool Selected { get; set; }
        public ContentTypes ContentType { get; set; }
    }
}