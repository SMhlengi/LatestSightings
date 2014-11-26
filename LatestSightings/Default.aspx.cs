using LatestSightingsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LatestSightings
{
    public partial class Default : System.Web.UI.Page
    {
        public int year = DateTime.Now.Year;
        public int month = DateTime.Now.Month;

        protected void Page_Load(object sender, EventArgs e)
        {
            var master = Master as DefaultMaster;
            if (master != null)
            {
                master.SetHeader("Dashboard", "home");
                master.SetActiveNav("dashboard");
            }

            Month mnth = Month.GetMonth(DateTime.Now.Year, DateTime.Now.Month);
            if (mnth == null || (mnth != null && mnth.EstimatedEarnings <= 0))
            {
                mnth = Month.GetMonth(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month);
            }

            if (mnth != null)
            {
                year = mnth.YearNumber;
                month = mnth.MonthNumber;

                ltlEarnings.Text = "$" + mnth.EstimatedEarnings.ToString("G29");
                ltlViews.Text = mnth.Views.ToString();

                DateTime curMonth = new DateTime(year, month, 1);
                earMnth.InnerHtml = curMonth.ToString("MMMM") + " Earnings";
                earMnthTot.InnerHtml = mnth.EstimatedEarnings.ToString("G29");

                viewMnth.InnerHtml = curMonth.ToString("MMMM") + " Views";
                viewMnthTot.InnerHtml = mnth.Views.ToString();

                for (int i = 1; i < 3; i++)
                {
                    curMonth = curMonth.AddMonths(-1);

                    Month tmpMnth = Month.GetMonth(curMonth.Year, curMonth.Month);

                    switch (i)
                    {
                        case 1:
                            earMnth1.InnerHtml = curMonth.ToString("MMMM");
                            viewMnth1.InnerHtml = curMonth.ToString("MMMM");
                            if (tmpMnth != null)
                            {
                                earMnth1Tot.InnerHtml = "$" + tmpMnth.EstimatedEarnings.ToString("G29");
                                viewMnthTot1.InnerHtml = tmpMnth.Views.ToString();
                            }
                            break;
                        case 2:
                            earMnth2.InnerHtml = curMonth.ToString("MMMM");
                            viewMnth2.InnerHtml = curMonth.ToString("MMMM");
                            if (tmpMnth != null)
                            {
                                earMnth2Tot.InnerHtml = "$" + tmpMnth.EstimatedEarnings.ToString("G29");
                                viewMnthTot2.InnerHtml = tmpMnth.Views.ToString();
                            }
                            break;
                        default:
                            break;
                    }
                }

                List<LatestSightingsLibrary.Video> pending = LatestSightingsLibrary.Video.GetVideosCompact("Pending");
                List<LatestSightingsLibrary.Video> nonPending = LatestSightingsLibrary.Video.GetVideosCompact("");

                int pendingCount = pending == null ? 0 : pending.Count;
                int nonPendingCount = nonPending == null ? 0 : nonPending.Count;

                vidPen.InnerHtml = pendingCount.ToString();
                vidPub.InnerHtml = nonPendingCount.ToString();
                vidTot.InnerHtml = (nonPendingCount + pendingCount).ToString();

                List<Stat> stats = Stat.GetVideoStats(year, month, 0, Stat.Top10Types.Earnings);
                if (stats != null && stats.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    int count = 1;
                    foreach (Stat stat in stats)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td>" + count.ToString() + "</td>");
                        sb.Append("<td>" + stat.VideoTitle + "</td>");
                        sb.Append("<td>" + stat.Stat3 + "</td>");
                        sb.Append("</tr>");
                        count++;
                    }
                    ltlTop10Earnings.Text = sb.ToString();
                }

                stats = Stat.GetVideoStats(year, month, 0, Stat.Top10Types.Views);
                if (stats != null && stats.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    int count = 1;
                    foreach (Stat stat in stats)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td>" + count.ToString() + "</td>");
                        sb.Append("<td>" + stat.VideoTitle + "</td>");
                        sb.Append("<td>" + stat.Stat1 + "</td>");
                        sb.Append("</tr>");
                        count++;
                    }
                    ltlTop10Views.Text = sb.ToString();
                }

                stats = Stat.GetStats(year, month, 0, Stat.Top10Types.CountryViews);
                if (stats != null && stats.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    int count = 1;
                    foreach (Stat stat in stats)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td>" + count.ToString() + "</td>");
                        sb.Append("<td>" + stat.Stat1 + "</td>");
                        sb.Append("<td>" + stat.Stat2 + "</td>");
                        sb.Append("</tr>");
                        count++;
                    }
                    ltlTop10Countries.Text = sb.ToString();
                }
            }
        }
    }
}