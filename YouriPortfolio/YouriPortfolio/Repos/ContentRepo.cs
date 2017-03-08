using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PCAuthLib;
using PCDataDLL;
using YouriPortfolio.Models;

namespace YouriPortfolio.Repos
{
    public class ContentRepo
    {
        public static List<Content> GetAllContent(string category = "DEFAULT")
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"category", category}
            };
            var result = DB.PFDB.GetMultipleResultsQuery("SELECT * FROM CONTENT WHERE CATEGORY=?", parameters);

            if (result != null)
            {
                List<Content> returnList = new List<Content>();

                foreach (var row in result)
                {
                    returnList.Add(new Content(
                        row.Get("ID"),
                        row.Get("Title"),
                        row.Get("ShortContentBlock"),
                        row.Get("ContentBlock"),
                        row.Get("HeaderIMG")));
                }
                return returnList;
            }
            return null;
        }

        public static Content GetContent(int id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"id", id}
            };
            var result = DB.PFDB.GetOneResultQuery("SELECT * FROM CONTENT WHERE ID=?", parameters);

            if (result != null)
            {
                return new Content(
                    result.Get("ID"),
                    result.Get("Title"),
                    result.Get("ShortContentBlock"),
                    result.Get("ContentBlock"),
                    result.Get("HeaderIMG"));
            }
            return null;
        }

        public static bool UpdateContent(Content toEdit)
        {
            string sql = "UPDATE CONTENT SET TITLE=?, ShortContentBlock=?, ContentBlock=?, HeaderIMG=? WHERE ID=?";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"Title", toEdit.Title},
                {"ShortContentBlock", toEdit.ShortContent },
                {"ContentBlock", toEdit.ContentText },
                {"HeaderImg", toEdit.HeaderIMG },
                {"id", toEdit.ID}
            };
            var result = DB.PFDB.UpdateQuery(sql, parameters);

            return result;
        }

        public static bool InsertContent(Content toInsert)
        {
            string sql = "INSERT INTO CONTENT(Title,ShortContentBlock,ContentBlock,HeaderImg) VALUES(?,?,?,?)";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"Title", toInsert.Title},
                {"ShortContentBlock", toInsert.ShortContent },
                {"ContentBlock", toInsert.ContentText },
                {"HeaderImg", toInsert.HeaderIMG }
            };
            var result = DB.PFDB.InsertQuery(sql, parameters);

            return result;
        }
    }
}