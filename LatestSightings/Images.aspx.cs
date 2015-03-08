using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LatestSightings
{
    public partial class Images : System.Web.UI.Page
    {
        public string startDate = DateTime.Now.ToString("MMMM yyyy");

        protected void Page_Load(object sender, EventArgs e)
        {
            var master = Master as DefaultMaster;
            if (master != null)
            {
                master.SetHeader("Images", "file-image-o");
                master.SetActiveNav("siteimages");
            }

            txtMonthPicker.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("MMMM yyyy");
        }
    }
}