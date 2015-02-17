using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LatestSightings
{
    public partial class Videos : System.Web.UI.Page
    {
        public string status = "published";

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageValues();

            var master = Master as DefaultMaster;
            if (master != null)
            {
                TextInfo myTI = new CultureInfo("en-US", false).TextInfo;

                master.SetHeader(myTI.ToTitleCase(status) + " Videos", "video-camera");
                master.SetActiveNav("videos");
            }
        }

        private void SetPageValues()
        {
            if (!String.IsNullOrEmpty(Request.QueryString["status"]))
                status = Request.QueryString["status"];

            if (Page.RouteData.Values["status"] != null && !String.IsNullOrEmpty(Page.RouteData.Values["status"].ToString()))
                status = Page.RouteData.Values["status"].ToString();
        }
    }
}