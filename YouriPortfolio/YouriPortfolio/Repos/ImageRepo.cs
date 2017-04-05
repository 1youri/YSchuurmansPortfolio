using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PCDataDLL;

namespace YouriPortfolio.Repos
{
    public class ImageRepo
    {
        public static bool InsertImage(int contentID, )
        {
            string sql = "UPDATE CONTENT SET TITLE=?, ShortContentBlock=?, ContentBlock=?, HeaderIMG=? WHERE ID=?";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                //{"Title", toEdit.Title},
                //{"ShortContentBlock", toEdit.ShortContent },
                //{"ContentBlock", toEdit.ContentText },
                //{"HeaderImg", toEdit.HeaderIMG },
                //{"id", toEdit.ID}
            };
            var result = DB.PFDB.UpdateQuery(sql, parameters);

            return result;
        }
    }
}