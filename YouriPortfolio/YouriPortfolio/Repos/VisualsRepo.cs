using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PCDataDLL;
using YouriPortfolio.Models;

namespace YouriPortfolio.Repos
{
    public class VisualsRepo
    {
        public static List<Visual> GetVisuals(int contentID)
        {
            string sql = "SELECT * FROM VISUAL WHERE ContentID=? ORDER BY CONTENTTYPE DESC";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"id", contentID}
            };
            var result = DB.PFDB.GetMultipleResultsQuery(sql, parameters);

            if (result != null)
            {
                List<Visual> returnList = new List<Visual>();

                foreach (var row in result)
                {
                    returnList.Add(new Visual(row.Get("ID").ToInt(), row.Get("Location"), (Visual.ContentTypes)row.Get("ContentType").ToInt()));
                }
                return returnList;
            }
            return null;
        }
        public static bool InsertVisual(int contentID, string imageLocation, Visual.ContentTypes type)
        {
            string sql = "INSERT INTO VISUAL(ContentID, Location, ContentType) VALUES(?, ?, ?)";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"contentID", contentID } ,
                {"location", imageLocation },
                {"contentType", type }
            };
            var result = DB.PFDB.InsertQuery(sql, parameters);

            return result;
        }

        public static bool RemoveImage(int ImageID)
        {
            string sql = "DELETE FROM VISUAL WHERE ID=?";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "ImageID", ImageID }
            };
            var result = DB.PFDB.DeleteQuery(sql, parameters);

            return result;
        }

        public static bool RemoveImages(int[] ImageIDs)
        {
            string inString = "(";

            foreach (var imageID in ImageIDs)
            {
                if (inString == "(") inString += imageID;
                else inString += "," + imageID;
            }
            inString += ")";

            string sql = "DELETE FROM VISUAL WHERE ID IN "+ inString;
            var result = DB.PFDB.DeleteQuery(sql, null);

            return result;
        }
    }
}