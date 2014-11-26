using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTubeAnalytics.v1;
using Google.Apis.YouTubeAnalytics.v1.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LatestSightingsService
{
    public class Month
    {
        #region Properties
        private string _projectName = string.Empty;
        private string ProjectName
        {
            get
            {
                if (String.IsNullOrEmpty(_projectName))
                {
                    _projectName = ConfigurationManager.AppSettings["YoutubeProjectName"].ToString();
                }

                return _projectName;
            }
            set
            {
                _projectName = value;
            }
        }

        private string _customerOwnerId = string.Empty;
        private string CustomerOwnerId
        {
            get
            {
                if (String.IsNullOrEmpty(_customerOwnerId))
                {
                    _customerOwnerId = ConfigurationManager.AppSettings["YoutubeCustomerOwnerId"].ToString();
                }

                return _customerOwnerId;
            }
            set
            {
                _customerOwnerId = value;
            }
        }

        private string _oAuthfile = string.Empty;
        private string OAuthfile
        {
            get
            {
                if (String.IsNullOrEmpty(_oAuthfile))
                {
                    _oAuthfile = ConfigurationManager.AppSettings["YoutubeJsonOAuthFile"].ToString();
                }

                return _oAuthfile;
            }
            set
            {
                _oAuthfile = value;
            }
        }
        #endregion

        public void UpdateMonthAnalytics(int year, int month)
        {
            LatestSightingsLibrary.Month mnth = null;
            try
            {
                mnth = LatestSightingsLibrary.Month.GetMonth(year, month);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving month");
            }

            if (mnth == null)
                mnth = LatestSightingsLibrary.Month.GetNewMonth(year, month);

            if (mnth.Earnings <= 0 && mnth.EstimatedEarningsSupplied <= 0 && mnth.EstimatedDate < DateTime.Now.AddHours(-11))
            {
                try
                {
                    GetMonthAnalytics(ref mnth);
                    SaveMonthAnalytics(mnth);
                    if (mnth != null)
                    {
                        UpdateMonthAnalyticsByDay(mnth.YearNumber, mnth.MonthNumber);
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        public void UpdateMonthAnalyticsByDay(int year, int month)
        {
            LatestSightingsLibrary.Month mnth = null;
            try
            {
                mnth = LatestSightingsLibrary.Month.GetMonth(year, month);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving month");
            }

            if (mnth == null)
                mnth = LatestSightingsLibrary.Month.GetNewMonth(year, month);
            try
            {
                List<LatestSightingsLibrary.YouTubeVideoAnalytic> days = GetMonthAnalyticsByDay(ref mnth);
                SaveMonthAnalyticsByDay(days);
            }
            catch (Exception ex)
            {

            }
        }

        private void SaveMonthAnalytics(LatestSightingsLibrary.Month mnth)
        {
            if (mnth != null && mnth.EstimatedEarnings > 0)
            {
                LatestSightingsLibrary.Month.SaveMonth(mnth);
            }
        }

        private void SaveMonthAnalyticsByDay(List<LatestSightingsLibrary.YouTubeVideoAnalytic> days)
        {
            if (days != null && days.Count > 0)
            {
                LatestSightingsLibrary.YouTubeVideoAnalytic.SaveDays(days);
            }
        }

        private void GetMonthAnalytics(ref LatestSightingsLibrary.Month mnth)
        {
           try
            {
                DateTime analyticsMonth = new DateTime(mnth.YearNumber, mnth.MonthNumber, 1, 0, 0, 0);
                UserCredential credential;
                using (var stream = new FileStream(OAuthfile, FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        new[] { YouTubeAnalyticsService.Scope.YtAnalyticsMonetaryReadonly, YouTubeAnalyticsService.Scope.YtAnalyticsReadonly },
                        "user", CancellationToken.None, new FileDataStore("Drive.Auth.Store")).Result;
                }

                var service = new YouTubeAnalyticsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ProjectName,
                });

                Google.Apis.YouTubeAnalytics.v1.ReportsResource.QueryRequest request = service.Reports.Query("contentOwner==" + CustomerOwnerId, analyticsMonth.ToString("yyyy-MM-dd"), analyticsMonth.ToString("yyyy-MM-dd"), "earnings,views");
                request.Dimensions = "month";
                request.Filters = "claimedStatus==claimed";
                ResultTable rTable = request.Execute();
                if (rTable != null && rTable.Rows.Count > 0)
                {
                    string earnings = rTable.Rows[0][1].ToString();
                    string views = rTable.Rows[0][2].ToString();

                    mnth.Views = Convert.ToInt64(views);
                    mnth.EstimatedEarnings = Convert.ToDecimal(earnings);
                    mnth.EstimatedDate = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex, "Error retrieving month analytics");
                throw new Exception("Error retrieving month analytics ");
            }
        }

        private List<LatestSightingsLibrary.YouTubeVideoAnalytic> GetMonthAnalyticsByDay(ref LatestSightingsLibrary.Month mnth)
        {
            List<LatestSightingsLibrary.YouTubeVideoAnalytic> days = null;

            try
            {
                DateTime analyticsMonth = new DateTime(mnth.YearNumber, mnth.MonthNumber, 1, 0, 0, 0);
                UserCredential credential;
                using (var stream = new FileStream(OAuthfile, FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        new[] { YouTubeAnalyticsService.Scope.YtAnalyticsMonetaryReadonly, YouTubeAnalyticsService.Scope.YtAnalyticsReadonly },
                        "user", CancellationToken.None, new FileDataStore("Drive.Auth.Store")).Result;
                }

                var service = new YouTubeAnalyticsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ProjectName,
                });

                DateTime startDate = analyticsMonth;
                DateTime endDate = new DateTime(analyticsMonth.Year, analyticsMonth.Month, DateTime.DaysInMonth(analyticsMonth.Year, analyticsMonth.Month));
                Google.Apis.YouTubeAnalytics.v1.ReportsResource.QueryRequest request = service.Reports.Query("contentOwner==" + CustomerOwnerId, startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"), "earnings,views");
                request.Dimensions = "day";
                request.Filters = "claimedStatus==claimed";
                ResultTable rTable = request.Execute();
                if (rTable != null && rTable.Rows.Count > 0)
                {
                    days = new List<LatestSightingsLibrary.YouTubeVideoAnalytic>();

                    for (int i = 0; i < rTable.Rows.Count; i++)
                    {
                        LatestSightingsLibrary.YouTubeVideoAnalytic day = new LatestSightingsLibrary.YouTubeVideoAnalytic();
                        DateTime date = DateTime.ParseExact(rTable.Rows[i][0].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        string earnings = rTable.Rows[i][1].ToString();
                        string views = rTable.Rows[i][2].ToString();

                        day.Views = Convert.ToInt64(views);
                        day.EstimatedEarning = Convert.ToDecimal(earnings);
                        day.Year = date.Year;
                        day.Month = date.Month;
                        day.Day = date.Day;

                        days.Add(day);
                    }
                }
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex, "Error retrieving month analytics");
                throw new Exception("Error retrieving month analytics ");
            }

            return days;
        }
    }
}
