using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PCDataDLL;

namespace PermacallWebApp.Repos
{
    public class LogRepo
    {
        public enum LogCategory
        {
            Request,
            Error
        }

        public static bool Log(HttpContext context)
        {
            return Log("[" + context.Request.HttpMethod + "] " + context.Request.RawUrl, LogRepo.LogCategory.Request,context.Request.UserHostAddress);
        }

        public static bool Log(string logDestription, LogCategory category, string ip)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"desc", logDestription},
                {"cate", category.ToString()},
                {"ip", ip }
            };
            var result = DB.PFDB.InsertQuery("INSERT INTO LOG(DESCRIPTION, CATEGORY, IP) VALUES (?, ?, ?)", parameters);

            return result;
        }
    }
}