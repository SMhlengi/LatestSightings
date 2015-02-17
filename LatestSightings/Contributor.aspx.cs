using LatestSightingsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LatestSightings
{
    public partial class Contributor : System.Web.UI.Page
    {
        public string id = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageValues();

            var master = Master as DefaultMaster;
            if (master != null)
            {
                string method = String.IsNullOrEmpty(id) ? "Add" : "Edit";
                master.SetHeader(method + " Contributor", "user");
                master.SetActiveNav("contributors");
            }

            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(id))
                {
                    AddUserDetail();
                }
                else
                {
                    ClearForm();
                }
            }
        }

        private void SetPageValues()
        {
            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                id = Request.QueryString["id"];

            if (Page.RouteData.Values["id"] != null && !String.IsNullOrEmpty(Page.RouteData.Values["id"].ToString()))
                id = Page.RouteData.Values["id"].ToString();
        }

        private void ClearForm()
        {
            rdbContributor.Checked = true;
        }

        private void AddUserDetail()
        {
            Person person = Person.GetPerson(id);
            if (person != null)
            {
                txtAddress.Text = person.Address;
                txtBanking.Text = person.Banking;
                txtPaypal.Text = person.Paypal;
                txtCell.Text = person.CellNumber;
                txtEmail.Text = person.Email;
                txtFacebook.Text = person.Facebook;
                txtFirstName.Text = person.FirstName;
                txtOther.Text = person.OtherContact;
                txtPassword.Text = person.Password;
                txtSkype.Text = person.Skype;
                txtSurname.Text = person.LastName;
                txtTelNumber.Text = person.TelephoneNumber;
                txtTwitter.Text = person.Twitter;

                switch (person.Role)
                {
                    case 1:
                        rdbAdmin.Checked = true;
                        break;
                    case 2:
                        rdbFinance.Checked = true;
                        break;
                    case 3:
                        rdbContributor.Checked = true;
                        break;
                    default:
                        break;
                }

                chbxActive.Checked = person.Active;
            }
        }

        protected void Save(object sender, EventArgs e)
        {
            bool isUpdate = String.IsNullOrEmpty(id) ? false : true;
            Person person = new Person();
            person.Id = id;
            person.Email = txtEmail.Text;
            person.FirstName = txtFirstName.Text;
            person.LastName = txtSurname.Text;
            person.Address = txtAddress.Text;
            person.Banking = txtBanking.Text;
            person.Paypal = txtPaypal.Text;
            person.CellNumber = txtCell.Text;
            person.TelephoneNumber = txtTelNumber.Text;
            person.OtherContact = txtOther.Text;
            person.Facebook = txtFacebook.Text;
            person.Skype = txtSkype.Text;
            person.Twitter = txtTwitter.Text;
            person.Active = chbxActive.Checked;
            person.Password = txtPassword.Text;

            int role = 3;
            if (rdbAdmin.Checked)
                role = 1;
            if (rdbFinance.Checked)
                role = 2;
            person.Role = role;

            bool saved = Person.SavePerson(person);

            if (saved)
            {
                if (isUpdate)
                {
                    UserInfo.AddAlert("Contributor " + person.FirstName + " " + person.LastName + " was successfully updated", GritterMessage.GritterMessageType.success);
                }
                else
                {
                    UserInfo.AddAlert("Contributor " + person.FirstName + " " + person.LastName + " was successfully added", GritterMessage.GritterMessageType.success);
                }

                HttpContext.Current.Response.Redirect("/contributors");
            }
            else
            {
                if (isUpdate)
                {
                    UserInfo.AddAlert("Error updating " + person.FirstName + " " + person.LastName, GritterMessage.GritterMessageType.error);
                }
                else
                {
                    UserInfo.AddAlert("Error adding " + person.FirstName + " " + person.LastName, GritterMessage.GritterMessageType.error);
                }
            }
        }
    }
}