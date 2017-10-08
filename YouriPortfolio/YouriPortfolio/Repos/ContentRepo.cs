using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using PCAuthLib;
using PCDataDLL;
using YouriPortfolio.Models;

namespace YouriPortfolio.Repos
{
    public class ContentRepo
    {
        public static List<Content> GetAllContent(string category = "DEFAULT", bool isAdmin = false)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"category", category}
            };
            var result = DB.PFDB.GetMultipleResultsQuery("SELECT * FROM CONTENT WHERE CATEGORY=? ORDER BY PRIORITY ASC,ID DESC", parameters);

            if (result != null)
            {
                List<Content> returnList = new List<Content>();

                foreach (var row in result)
                {
                    if(!isAdmin && row.Get("Shown") != "1") continue;
                    returnList.Add(new Content(
                        row.Get("ID"),
                        row.Get("Title"),
                        row.Get("ShortContentBlock"),
                        row.Get("ContentBlock"),
                        row.Get("Priority").ToInt(),
                        row.Get("Date"),
                        row.Get("Shown") == "1"));
                }
                return returnList;
            }
            return null;
        }

        public static Content GetContent(int id, bool isAdmin = false)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"id", id}
            };
            var result = DB.PFDB.GetOneResultQuery("SELECT * FROM CONTENT WHERE ID=?", parameters);

            if (result != null)
            {
                if (!isAdmin && result.Get("Shown") != "1") return null;
                return new Content(
                    result.Get("ID"),
                    result.Get("Title"),
                    result.Get("ShortContentBlock"),
                    result.Get("ContentBlock"),
                    result.Get("Priority").ToInt(),
                    result.Get("Date"),
                    result.Get("Shown") == "1");
            }
            return null;
        }

        public static Content GetLastContent()
        {
            var result = DB.PFDB.GetOneResultQuery("SELECT * FROM CONTENT WHERE CATEGORY='DEFAULT' ORDER BY ID DESC LIMIT 1", null);

            if (result != null)
            {
                return new Content(
                    result.Get("ID"),
                    result.Get("Title"),
                    result.Get("ShortContentBlock"),
                    result.Get("ContentBlock"),
                    result.Get("Priority").ToInt(),
                    result.Get("Date"),
                    Convert.ToBoolean(result.Get("Shown")));
            }
            return null;
        }

        public static bool UpdateOrder(string[] newIDOrder)
        {
            List<string> sqlStrings = new List<string>();
            List<Dictionary<string, object>> parameterList = new List<Dictionary<string, object>>();

            for (int i = 0; i < newIDOrder.Length; i++)
            {
                sqlStrings.Add("UPDATE CONTENT SET PRIORITY=? WHERE ID=? AND CATEGORY='DEFAULT'");
                parameterList.Add(new Dictionary<string, object>()
                {
                    {"priority", i},
                    {"id", newIDOrder[i]}
                });
            }
            
            var result = DB.PFDB.UpdateMultiQuery(sqlStrings, parameterList);

            return result;
        }

        public static bool InsertContent(Content toInsert)
        {
            string sql = "INSERT INTO CONTENT(Title,ShortContentBlock,ContentBlock,Date) VALUES(?,?,?,?)";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"Title", toInsert.Title},
                {"ShortContentBlock", toInsert.ShortContent },
                {"ContentBlock", toInsert.ContentText },
                {"date",toInsert.Date }
            };
            var result = DB.PFDB.InsertQuery(sql, parameters);

            return result;
        }

        public static bool DeleteContent(int ID)
        {
            string sql = "DELETE FROM CONTENT WHERE ID=?";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"id", ID}
            };
            var result = DB.PFDB.DeleteQuery(sql, parameters);

            return result;
        }

        public static CVViewModel GetCV()
        {
            var result = DB.PFDB.GetOneResultQuery("SELECT * FROM CONTENT WHERE CATEGORY='CV'", null);

            if (result != null)
            {
                if (result.KeyCount() > 0 && result.Get("ContentBlock") != null)
                {
                    return JsonConvert.DeserializeObject<CVViewModel>(result.Get("ContentBlock"));
                }
                else
                {
                    if (!DB.PFDB.CheckExist("SELECT * FROM CONTENT WHERE CATEGORY = 'CV'", null))
                    {
                        DB.PFDB.InsertQuery("INSERT INTO CONTENT(CATEGORY) VALUES('CV')", null);
                        return new CVViewModel();
                    }
                }
            }
            return null;
        }

        public static bool UpdateCV(string toUpdate)
        {
            string sql = "UPDATE CONTENT SET ContentBlock=? WHERE CATEGORY='CV'";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"Title", toUpdate}
            };
            var result = DB.PFDB.UpdateQuery(sql, parameters);

            return result;
        }

        public static bool UpdateContent(Content toEdit)
        {
            string sql = "UPDATE CONTENT SET TITLE=?, ShortContentBlock=?, ContentBlock=?, Priority=?, Date=? WHERE ID=? AND CATEGORY='DEFAULT'";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"Title", toEdit.Title},
                {"ShortContentBlock", toEdit.ShortContent },
                {"ContentBlock", toEdit.ContentText },
                {"priority", toEdit.Priority},
                {"date", toEdit.Date + "" },
                {"id", toEdit.ID}
            };
            var result = DB.PFDB.UpdateQuery(sql, parameters);

            return result;
        }
    }
}