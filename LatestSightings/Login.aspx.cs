using LatestSightingsLibrary;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LatestSightings
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void LogIn(object sender, EventArgs e)
        {
            bool error = false;

            if (!String.IsNullOrEmpty(txtEmail.Text) && !String.IsNullOrEmpty(txtPassword.Text))
            {
                Person person = Person.GetPerson(txtEmail.Text.Trim(), txtPassword.Text.Trim());
                if (person != null)
                {
                    SetUser(person, chbxRemember.Checked);
                }
                else
                {
                    error = true;
                }
            }

            if (error)
            {
                alert.Visible = true;
            }
            else
            {
                HttpContext.Current.Response.Redirect("/");
            }
        }

        private void SetUser(Person person, bool remember)
        {
            if (remember)
            {
                HttpContext.Current.Response.Cookies["user"].Value = person.Id;
                HttpContext.Current.Response.Cookies["user"].Expires = DateTime.Now.AddDays(7);
            }

            Session["user"] = person;
        }
    }
}