using LatestSightingsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LatestSightings
{
    public partial class Contributors : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var master = Master as DefaultMaster;
            if (master != null)
            {
                master.SetHeader("Contributors", "user");
                master.SetActiveNav("contributors");
            }
        }
    }
}