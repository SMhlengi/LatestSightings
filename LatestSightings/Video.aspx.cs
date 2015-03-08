using LatestSightingsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.IO;

namespace LatestSightings
{
    public partial class Video : System.Web.UI.Page
    {
        public string id = string.Empty;
        public string contributor = string.Empty;
        public int thirdPartiesCount = 0;
        public string PageLoadScript = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageValues();

            var master = Master as DefaultMaster;
            if (master != null)
            {
                string method = String.IsNullOrEmpty(id) ? "Add" : "Edit";
                master.SetHeader(method + " Video", "video-camera");
                master.SetActiveNav("videos");
            }

            if (!Page.IsPostBack)
            {
                CreateForm();

                if (!String.IsNullOrEmpty(id))
                    AddVideoDetail();
            }
        }

        private void AddVideoDetail()
        {
            LatestSightingsLibrary.Video vid = LatestSightingsLibrary.Video.GetVideo(id);
            if (vid != null)
            {
                txtTitle.Text = vid.Title;
                txtAlias.Text = vid.Alias;
                txtRecievedPicker.Text = vid.Recieved.Year > DateTime.Now.AddYears(2).Year ? "" : vid.Recieved.ToString("MM/dd/yyyy");
                txtYoutubeId.Text = vid.YoutubeId;
                txtIpPicker.Text = vid.IPDate.Year < 2000 ? "" : vid.IPDate.ToString("MM/dd/yyyy");
                if (!String.IsNullOrEmpty(vid.IPDocument))
                {
                    uploadSpan.InnerHtml = "Upload New";
                    ipLink.NavigateUrl = "/files/" + vid.IPDocument;
                    ipLink.Visible = true;
                }
                if (!String.IsNullOrEmpty(vid.RevenueShare))
                {
                    foreach (ListItem item in ddlRevenueShare.Items)
                    {
                        if (item.Value == vid.RevenueShare)
                            item.Selected = true;
                    }
                }
                txtKeywords.Text = vid.Keywords;
                txtNotes.Text = vid.Notes;
                txtRegion.Text = vid.Region;
                ddlStatus.SelectedValue = vid.Status;
                if (vid.ThirdParties != null && vid.ThirdParties.Count > 0)
                {
                    foreach (ThirdPartyVideo thirdVid in vid.ThirdParties)
                    {
                        AddThirdParty(thirdVid);
                    }
                }
                chbxStream.Checked = vid.IsLiveStream;

                divContributor.Visible = true;
                divRecalculate.Visible = true;
                btnWatch.Visible = true;
            }
        }

        private void AddThirdParty(ThirdPartyVideo thirdVid)
        {
            thirdPartiesCount++;
            HtmlGenericControl row = new HtmlGenericControl("div");
            row.Attributes.Add("class", "row");
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"form-group col-md-4\"><div style=\"margin-bottom: 4px;\"><label class=\"control-label\">Third Party</label></div>");
            sb.Append("<select id=\"selectThirdParty" + thirdPartiesCount + "\" name=\"selectThirdParty" + thirdPartiesCount + "\" style=\"width: 100%;\">");
            foreach (ListItem item in ddlSelectThirdParty.Items)
            {
                string itemChecked = string.Empty;
                if (item.Value == thirdVid.ThirdParty)
                    itemChecked = " selected";           
                sb.Append("<option value=\"" + item.Value + "\"" + itemChecked + ">" + item.Text + "</option>");
            }
            sb.Append("</select>");
            sb.Append("</div>");
            sb.Append("<div class=\"form-group col-md-4\"><label class=\"control-label\">Alias</label>");
            sb.Append("<input type=\"text\" class=\"form-control with-label\" name=\"alias" + thirdPartiesCount + "\" id=\"alias" + thirdPartiesCount + "\" value=\"" + thirdVid.Alias + "\" required>");
            sb.Append("</div>");
            sb.Append("<div class=\"form-group col-md-4\"><label class=\"control-label\">Removed Date</label>");
            string dateRemoved = thirdVid.DateRemoved.Year > DateTime.Now.AddYears(2).Year ? "" : thirdVid.DateRemoved.ToString("MM/dd/yyyy");
            sb.Append("<input type=\"text\" class=\"form-control with-label\" name=\"datepickerR" + thirdPartiesCount + "\" id=\"datepickerR" + thirdPartiesCount + "\" value=\"" + dateRemoved + "\">");
            sb.Append("</div>");
            row.InnerHtml = sb.ToString();
            plcThirdParties.Controls.Add(row);
            PageLoadScript += "jQuery('#selectThirdParty" + thirdPartiesCount + "').select2({ minimumResultsForSearch: -1 });" + Environment.NewLine;
            PageLoadScript += "jQuery('#datepickerR" + thirdPartiesCount + "').datepicker();" + Environment.NewLine;
        }

        private void SetPageValues()
        {
            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                id = Request.QueryString["id"];

            if (Page.RouteData.Values["id"] != null && !String.IsNullOrEmpty(Page.RouteData.Values["id"].ToString()))
                id = Page.RouteData.Values["id"].ToString();

            if (!String.IsNullOrEmpty(Request.QueryString["contributor"]))
                contributor = Request.QueryString["contributor"];

            if (Page.RouteData.Values["contributor"] != null && !String.IsNullOrEmpty(Page.RouteData.Values["contributor"].ToString()))
                contributor = Page.RouteData.Values["contributor"].ToString();
        }

        private void CreateForm()
        {
            FillRevenueShare();
            FillThirdParties();
            if (String.IsNullOrEmpty(id))
                txtRecievedPicker.Text = DateTime.Now.ToString("MM/dd/yyyy");
        }

        private void FillRevenueShare()
        {
            List<RevenueShare> shares = Revenue.GetRevenueShares();
            ddlRevenueShare.DataSource = shares;
            ddlRevenueShare.DataTextField = "Text";
            ddlRevenueShare.DataValueField = "Text";
            ddlRevenueShare.DataBind();
            ddlRevenueShare.Items.Insert(0, new ListItem("Select Revenue Share...", ""));
        }

        private void FillThirdParties()
        {
            List<ThirdParty> thirdParties = ThirdParty.GetThirdParties();
            ddlSelectThirdParty.DataSource = thirdParties;
            ddlSelectThirdParty.DataTextField = "Name";
            ddlSelectThirdParty.DataValueField = "Id";
            ddlSelectThirdParty.DataBind();
            ddlSelectThirdParty.Items.Insert(0, new ListItem("Select Third Party...", ""));
        }

        protected void Save(object sender, EventArgs e)
        {
            bool isUpdate = true;
            string videoStatus = string.Empty;
            LatestSightingsLibrary.Video vid = LatestSightingsLibrary.Video.GetVideo(id);
            if (vid == null)
            {
                vid = new LatestSightingsLibrary.Video();
                vid.Contributor = contributor;
                vid.Id = Guid.NewGuid().ToString();
                vid.Status = "Pending";
                vid.DateRemoved = (DateTime)System.Data.SqlTypes.SqlDateTime.MaxValue;
                vid.DateUploaded = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                isUpdate = false;
            }
            else
            {
                if (!String.IsNullOrEmpty(txtContributor.Text))
                {
                    Person cntr = Person.GetPerson(txtContributor.Text);
                    if (cntr != null)
                    {
                        vid.Contributor = cntr.Id;
                    }
                }
            }
            vid.Alias = txtAlias.Text;
            vid.IPDate = String.IsNullOrEmpty(txtIpPicker.Text) ? (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue : DateTime.Parse(txtIpPicker.Text);
            vid.IPDocument = GetIpDocument();
            vid.Keywords = txtKeywords.Text;
            vid.Notes = txtNotes.Text;
            vid.Recieved = String.IsNullOrEmpty(txtRecievedPicker.Text) ? (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue : DateTime.Parse(txtRecievedPicker.Text);
            vid.Region = txtRegion.Text;
            vid.RevenueShare = Request.Form[ddlRevenueShare.UniqueID];
            vid.Title = txtTitle.Text;
            vid.IsLiveStream = chbxStream.Checked;
            if (vid.Status != ddlStatus.SelectedValue)
            {
                videoStatus = ddlStatus.SelectedValue;
            }

            if (videoStatus == "Published")
            {
                vid.DateUploaded = DateTime.Now;
                vid.DateRemoved = (DateTime)System.Data.SqlTypes.SqlDateTime.MaxValue;
            }
            if (videoStatus == "UnPublished")
                vid.DateRemoved = DateTime.Now;

            vid.Status = ddlStatus.SelectedValue;
            vid.YoutubeId = txtYoutubeId.Text;
            vid.ThirdParties = GetThirdPartyVideos(vid);

            bool saved = LatestSightingsLibrary.Video.SaveVideo(vid);

            if (saved)
            {
                if (isUpdate)
                {
                    UserInfo.AddAlert("Video was successfully updated", GritterMessage.GritterMessageType.success);
                }
                else
                {
                    UserInfo.AddAlert("Video was successfully added", GritterMessage.GritterMessageType.success);
                }

                HttpContext.Current.Response.Redirect("/contributors");
            }
            else
            {
                if (isUpdate)
                {
                    UserInfo.AddAlert("Error updating Video", GritterMessage.GritterMessageType.error);
                }
                else
                {
                    UserInfo.AddAlert("Error adding Video", GritterMessage.GritterMessageType.error);
                }
            }
        }

        protected void SendEmail(object sender, EventArgs e)
        {
            LatestSightingsLibrary.Video vid = LatestSightingsLibrary.Video.GetVideo(id);
            LatestSightingsLibrary.Person cont = LatestSightingsLibrary.Person.GetPerson(vid.Contributor);

            try
            {
                string emailTemplate = File.ReadAllText(MapPath("email_publish.html"));

                MailMessage message = new MailMessage(ConfigurationManager.AppSettings["emailFromAddress"], cont.Email);
                //MailMessage message = new MailMessage(ConfigurationManager.AppSettings["emailFromAddress"], "soulunavailable@gmail.com");
                message.IsBodyHtml = true;
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Port = 25;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Host = ConfigurationManager.AppSettings["emailHost"];
                NetworkCredential nc = new NetworkCredential(ConfigurationManager.AppSettings["emailUser"], ConfigurationManager.AppSettings["emailPassword"]);
                smtpClient.Credentials = nc;
                message.Subject = "Your video has been published to our YouTube channel";
                message.Body = String.Format(emailTemplate, cont.FirstName, vid.Title, ConfigurationManager.AppSettings["editDetailsPage"]);
                smtpClient.Send(message);

                UserInfo.AddAlert("Published email sent", GritterMessage.GritterMessageType.success);
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex);
                UserInfo.AddAlert("Error sending email", GritterMessage.GritterMessageType.error);
            }
        }

        private string GetIpDocument()
        {
            string fileName = string.Empty;

            if (File1.PostedFile != null && File1.HasFile)
            {
                fileName = File1.FileName;
                File1.PostedFile.SaveAs(MapPath("files") + "/" + fileName);
            }

            return fileName;
        }

        private List<ThirdPartyVideo> GetThirdPartyVideos(LatestSightingsLibrary.Video vid)
        {
            List<ThirdPartyVideo> vids = ThirdPartyVideo.GetVideos(vid.Id);
            List<ThirdPartyVideo> updatedVids = new List<ThirdPartyVideo>();

            for (int i = 0; i <= 10; i++)
            {
                if (Request.Form["selectThirdParty" + i] != null && Request.Form["selectThirdParty" + i] != "")
                {
                    ThirdPartyVideo thirdPartyVid = null;
                    if (vids != null && vids.Count > 0)
                    {
                        thirdPartyVid = vids.First(x => { return x.ThirdParty == Request.Form["selectThirdParty" + i]; });
                    }
                    if (thirdPartyVid == null)
                        thirdPartyVid = new ThirdPartyVideo();
                    thirdPartyVid.ThirdParty = Request.Form["selectThirdParty" + i];
                    if (Request.Form["alias" + i] != null)
                    {
                        thirdPartyVid.Alias = Request.Form["alias" + i] != "" ? Request.Form["alias" + i] : "";
                    }
                    thirdPartyVid.DateCreated = DateTime.Now;
                    thirdPartyVid.DateRemoved = String.IsNullOrEmpty(id) || String.IsNullOrEmpty(Request.Form["datepickerR" + i]) ? (DateTime)System.Data.SqlTypes.SqlDateTime.MaxValue : Convert.ToDateTime(Request.Form["datepickerR" + i]);
                    updatedVids.Add(thirdPartyVid);
                }
            }

            if (updatedVids.Count > 0)
                vids = updatedVids;

            return vids;
        }

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public static List<RevenueShare> AddRevenueShare(string share)
        {
            Revenue.AddRevenueShare(Convert.ToDouble(share));
            return Revenue.GetRevenueShares();
        }

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public static List<ThirdParty> AddThirdParty(string name)
        {
            ThirdParty.AddThirdParty(name);
            return ThirdParty.GetThirdParties();
        }

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public static string Recalculate(string id)
        {
            return LatestSightingsLibrary.Video.SetRecalculate(id, true).ToString();
        }
    }
}