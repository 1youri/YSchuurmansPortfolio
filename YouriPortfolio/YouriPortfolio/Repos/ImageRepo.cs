using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PCDataDLL;

namespace YouriPortfolio.Repos
{
    public class ImageRepo
    {
        public static bool InsertImage(int contentID, string imageLocation)
        {
            string sql = "INSERT INTO IMAGES(ContentID, imageLocation) VALUES(?, ?)";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "contentID", contentID } ,
                {"imageLocation", imageLocation }
            };
            var result = DB.PFDB.InsertQuery(sql, parameters);

            return result;
        }

        public static bool RemoveImage(int ImageID) 
        {
            string sql = "DELETE FROM IMAGES WHERE ID=?";
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

            string sql = "DELETE FROM IMAGES WHERE ID IN ?";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "imageIn", inString }
            };
            var result = DB.PFDB.DeleteQuery(sql, parameters);

            return result;
        }
    }
}