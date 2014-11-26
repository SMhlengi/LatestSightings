using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LatestSightings
{
    public partial class UserAjax : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public static List<GritterMessage> GetGritterMessages()
        {
            List<GritterMessage> grits = UserInfo.GetAlerts();
            UserInfo.ClearAlerts();

            return grits;
        }
    }
}