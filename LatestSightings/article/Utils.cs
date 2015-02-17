using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace LatestSightings
{
    public class Utils
    {
        [WebMethod]
        public static string escapeApostrophe(string data)
        {
            data = data.Replace("'", "''");
            return data;
        }

        public static string escapeDoubleQuotation(string data)
        {
            data = data.Replace("\"", "''");
            return data;
        }
    }
}