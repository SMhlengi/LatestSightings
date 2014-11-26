using LatestSightingsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace LatestSightings
{
    public partial class Profile : System.Web.UI.Page
    {
        public Person person = null;
        public string id = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageValues();
            person = Person.GetPerson(id);
            var master = Master as DefaultMaster;
            if (master != null)
            {
                master.SetHeader("Profile", "user");
                master.SetActiveNav("contributors");
            }

            if (person != null)
            {
                FillDetails();
                FillVideos();
            }
        }

        private void FillVideos()
        {
            List<LatestSightingsLibrary.Video> contVideos = LatestSightingsLibrary.Video.GetContributorVideos(id);
            if (contVideos != null && contVideos.Count > 0)
            {
                tableRow.Visible = true;
                alert.Visible = false;
            }
            else
            {
                tableRow.Visible = false;
                alert.Visible = true;
            }            
        }

        private void FillDetails()
        {
            // Contacts
            if (!String.IsNullOrEmpty(person.Email))
            {
                HtmlGenericControl cntrl = new HtmlGenericControl("li");
                cntrl.InnerHtml = "<i class=\"fa fa-envelope\"></i> <a href=\"mailto:" + person.Email + "\">" + person.Email + "</a>";
                contacts.Controls.Add(cntrl);
            }
            if (!String.IsNullOrEmpty(person.CellNumber))
            {
                HtmlGenericControl cntrl = new HtmlGenericControl("li");
                cntrl.InnerHtml = "<i class=\"fa fa-mobile\"></i> " + person.CellNumber;
                contacts.Controls.Add(cntrl);
            }
            if (!String.IsNullOrEmpty(person.Email))
            {
                HtmlGenericControl cntrl = new HtmlGenericControl("li");
                cntrl.InnerHtml = "<i class=\"fa fa-phone\"></i> " + person.TelephoneNumber;
                contacts.Controls.Add(cntrl);
            }
            if (!String.IsNullOrEmpty(person.Email))
            {
                HtmlGenericControl cntrl = new HtmlGenericControl("li");
                cntrl.InnerHtml = "<i class=\"fa fa-madeup\"></i> " + person.OtherContact;
                contacts.Controls.Add(cntrl);
            }

            // Social
            if (!String.IsNullOrEmpty(person.Facebook))
            {
                HtmlGenericControl cntrl = new HtmlGenericControl("li");
                cntrl.InnerHtml = "<i class=\"fa fa-facebook\"></i> <a href=\"mailto:" + person.Email + "\">" + person.Facebook + "</a>";
                connect.Controls.Add(cntrl);
            }
            if (!String.IsNullOrEmpty(person.Twitter))
            {
                HtmlGenericControl cntrl = new HtmlGenericControl("li");
                cntrl.InnerHtml = "<i class=\"fa fa-twitter\"></i> <a href=\"mailto:" + person.Email + "\">" + person.Twitter + "</a>";
                connect.Controls.Add(cntrl);
            }
            if (!String.IsNullOrEmpty(person.Skype))
            {
                HtmlGenericControl cntrl = new HtmlGenericControl("li");
                cntrl.InnerHtml = "<i class=\"fa fa-skype\"></i> <a href=\"mailto:" + person.Email + "\">" + person.Skype + "</a>";
                connect.Controls.Add(cntrl);
            }
        }

        private void SetPageValues()
        {
            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                id = Request.QueryString["id"];

            if (Page.RouteData.Values["id"] != null && !String.IsNullOrEmpty(Page.RouteData.Values["id"].ToString()))
                id = Page.RouteData.Values["id"].ToString();
        }
    }
}