using LatestSightingsLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace LatestSightings
{
    public partial class DefaultMaster : System.Web.UI.MasterPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] != null)
            {
                Person person = (Person)Session["user"];
                ltlUserName.Text = person.FirstName + " " + person.LastName;
                ltlUserRole.Text = Person.RoleName(person.Role);
                if (person.Role > 2)
                    HttpContext.Current.Response.Redirect("~/user");
            }
            else
            {
                HttpContext.Current.Response.Redirect("/login");
            }
        }

        public void SetHeader(string Text, string icon)
        {
            HtmlGenericControl headerText = new HtmlGenericControl("li");
            headerText.InnerHtml = Text;
            breadcrumbs.Controls.Add(headerText);
            pageName.InnerHtml = Text;
            pageIcon.Attributes.Add("class", "fa fa-" + icon);

            string fileName = Path.GetFileName(Page.AppRelativeVirtualPath);

            if (fileName.ToLower() == "financials.aspx")
                pageName.InnerHtml += "<span id=\"ttPI\" class=\"glyphicon glyphicon-question-sign popovers\" title=\"\" data-original-title=\"\"  style=\"margin-left: 5px;\" data-container=\"body\" data-toggle=\"popover\" data-placement=\"top\" data-content=\"This is the page where we record payments made to us from Youtube and Third Parties in a specific month.\"></span>";
        }
        
        public void SetActiveNav(string text)
        {
            HtmlGenericControl cntrl = (HtmlGenericControl)FindControl("nav" + text.ToLower());
            if (cntrl != null)
            {
                string newClass = "active";
                if (cntrl.Attributes != null && cntrl.Attributes["class"] != null)
                {
                    newClass = cntrl.Attributes["class"] + " " + newClass;
                    cntrl.Attributes.Remove("class");
                }
                cntrl.Attributes.Add("class", newClass);
            }
        }
    }
}