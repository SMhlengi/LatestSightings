using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LatestSightings
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["user"] != null)
                HttpContext.Current.Session["user"] = null;
            if (HttpContext.Current.Request.Cookies["user"] != null)
            {
                HttpContext.Current.Response.Cookies["user"].Value = string.Empty;
                HttpContext.Current.Response.Cookies["user"].Expires = DateTime.Now.AddDays(-1);
            }
            HttpContext.Current.Response.Redirect("/login");
        }
    }
}