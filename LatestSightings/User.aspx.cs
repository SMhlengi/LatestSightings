using LatestSightingsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace LatestSightings
{
    public partial class User : System.Web.UI.Page
    {
        public Person person = null;
        public string currencyScripts = string.Empty;
        public int year = DateTime.Now.Year;
        public int month = DateTime.Now.Month;
        public string contributor;

        protected void Page_Load(object sender, EventArgs e)
        {
            Person peep = (Person)Session["user"];
            person = Person.GetPerson(peep.Id);

            if (person != null)
            {
                contributor = person.Id;
                FillDetails();
                FillVideos();
                SetCurrencies();
                FillStats();
            }      
        }

        private void FillStats()
        {
            Month mnth = Month.GetMonth(DateTime.Now.Year, DateTime.Now.Month);
            if (mnth == null || (mnth != null && mnth.EstimatedEarnings <= 0))
            {
                mnth = Month.GetMonth(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month);
            }

            if (mnth != null)
            {
                List<YouTubeVideoAnalytics> anals = LatestSightingsLibrary.Video.GetYouTubeVideoAnalyticsByContributor(year, month, contributor);
                decimal totalEarnings = 0;
                long totalViews = 0;

                foreach (YouTubeVideoAnalytics anal in anals)
                {
                    if (anal.EstimatedEarning > 0)
                        totalEarnings += anal.EstimatedEarning;
                    if (anal.Views > 0)
                        totalViews += anal.Views;
                }

                ltlEarnings.Text = "$" + totalEarnings.ToString();
                ltlViews.Text = totalViews.ToString();

                List<LatestSightingsLibrary.Video> videos = LatestSightingsLibrary.Video.GetContributorVideos(person.Id);

                List<YouTubeVideoAnalytics> stats = LatestSightingsLibrary.Video.GetYouTubeVideoAnalyticsByContributor(year, month, person.Id);
                if (stats != null && videos != null)
                {
                    stats = stats.OrderByDescending(x => x.EstimatedEarning).ToList();
                    StringBuilder sb = new StringBuilder();
                    int count = 1;
                    foreach (YouTubeVideoAnalytics stat in stats)
                    {
                        LatestSightingsLibrary.Video video = videos.FirstOrDefault(x => { return x.YoutubeId == stat.Id; });
                        if (video != null)
                        {
                            sb.Append("<tr>");
                            sb.Append("<td>" + count.ToString() + "</td>");
                            sb.Append("<td>" + video.Title + "</td>");
                            sb.Append("<td>$" + stat.EstimatedEarning + "</td>");
                            sb.Append("</tr>");
                            count++;
                        }
                    }
                    ltlTop10Earnings.Text = sb.ToString();

                    stats = stats.OrderByDescending(x => x.Views).ToList();
                    sb = new StringBuilder();
                    count = 1;
                    foreach (YouTubeVideoAnalytics stat in stats)
                    {
                        LatestSightingsLibrary.Video video = videos.FirstOrDefault(x => { return x.YoutubeId == stat.Id; });
                        if (video != null)
                        {
                            sb.Append("<tr>");
                            sb.Append("<td>" + count.ToString() + "</td>");
                            sb.Append("<td>" + video.Title + "</td>");
                            sb.Append("<td>" + stat.Views + "</td>");
                            sb.Append("</tr>");
                            count++;
                        }
                    }
                    ltlTop10Views.Text = sb.ToString();
                }
            }     
        }

        private void FillVideos()
        {
            List<LatestSightingsLibrary.Video> contVideos = LatestSightingsLibrary.Video.GetContributorVideos(person.Id);
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

        private void SetCurrencies()
        {
            List<Currency> currencies = Currency.GetCurrencies();
            if (currencies != null)
            {
                int count = 1;
                StringBuilder sb = new StringBuilder();
                foreach (Currency currency in currencies)
                {
                    currencyScripts += "{ \"data\": \"Currency" + count.ToString() + "\", orderable: false },";
                    sb.Append("<th style=\"background-color:#554337; color: #FFFFFF; border-bottom: 0px;\">" + currency.Description + "</th>" + Environment.NewLine);
                    count++;
                }
                ltlCurrencies.Text = sb.ToString();
            }
        }
    }
}