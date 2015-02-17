using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTubeAnalytics.v1;
using Google.Apis.YouTubeAnalytics.v1.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Globalization;

namespace LatestSightingsService
{
    public class Stats
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

        public void GetTopTen(int year, int month, LatestSightingsLibrary.Stat.Top10Types type)
        {
            List<LatestSightingsLibrary.Stat> stats = null;

            switch (type)
            {
                case LatestSightingsLibrary.Stat.Top10Types.Earnings:
                    stats = GetTopTenEarnings(year, month, type);
                    break;
                case LatestSightingsLibrary.Stat.Top10Types.Views:
                    stats = GetTopTenViews(year, month, type);
                    break;
                case LatestSightingsLibrary.Stat.Top10Types.CountryViews:
                    stats = GetCountryViews(year, month, type);
                    break;
                default:
                    break;
            }

            if (stats != null)
            {
                LatestSightingsLibrary.Stat.SaveStats(stats);
            }
        }

        private List<LatestSightingsLibrary.Stat> GetTopTenEarnings(int year, int month, LatestSightingsLibrary.Stat.Top10Types type)
        {
            List<LatestSightingsLibrary.Stat> stats = null;

            try
            {
                DateTime analyticsMonth = new DateTime(year, month, 1, 0, 0, 0);
                DateTime analyticsEndDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
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

                Google.Apis.YouTubeAnalytics.v1.ReportsResource.QueryRequest request = service.Reports.Query("contentOwner==" + CustomerOwnerId, analyticsMonth.ToString("yyyy-MM-dd"), analyticsEndDate.ToString("yyyy-MM-dd"), "views,estimatedMinutesWatched,earnings,monetizedPlaybacks,impressions");
                request.Dimensions = "video";
                request.Filters = "uploaderType==self";
                request.MaxResults = 10;
                request.Sort = "-earnings";
                ResultTable rTable = request.Execute();
                if (rTable != null && rTable.Rows != null && rTable.Rows.Count > 0)
                {
                    stats = new List<LatestSightingsLibrary.Stat>();

                    for (int i = 0; i < rTable.Rows.Count; i++)
                    {
                        LatestSightingsLibrary.Stat stat  = new LatestSightingsLibrary.Stat();
                        
                        stat.Day = analyticsMonth.Day;
                        stat.Month = analyticsMonth.Month;
                        stat.Year = analyticsMonth.Year;
                        stat.RunDateTime = DateTime.Now;
                        stat.Type = type;
                        stat.VideoId = rTable.Rows[i][0].ToString();
                        stat.Stat1 = rTable.Rows[i][1].ToString();
                        stat.Stat2 = rTable.Rows[i][2].ToString();
                        stat.Stat3 = rTable.Rows[i][3].ToString();
                        stat.Stat4 = rTable.Rows[i][4].ToString();
                        stat.Stat5 = rTable.Rows[i][5].ToString();
                        stat.Ordering = i + 1;
                        stat.Position = i + 1;
                        stats.Add(stat);
                    }
                }
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex, "Error retrieving Top 10 " + type.ToString());
                throw new Exception("Error retrieving Top 10 " + type.ToString());
            }

            return stats;
        }

        private List<LatestSightingsLibrary.Stat> GetTopTenViews(int year, int month, LatestSightingsLibrary.Stat.Top10Types type)
        {
            List<LatestSightingsLibrary.Stat> stats = null;

            try
            {
                DateTime analyticsMonth = new DateTime(year, month, 1, 0, 0, 0);
                DateTime analyticsEndDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
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

                Google.Apis.YouTubeAnalytics.v1.ReportsResource.QueryRequest request = service.Reports.Query("contentOwner==" + CustomerOwnerId, analyticsMonth.ToString("yyyy-MM-dd"), analyticsEndDate.ToString("yyyy-MM-dd"), "views,estimatedMinutesWatched,earnings,monetizedPlaybacks,impressions");
                request.Dimensions = "video";
                request.Filters = "uploaderType==self";
                request.MaxResults = 10;
                request.Sort = "-views";
                ResultTable rTable = request.Execute();
                if (rTable != null && rTable.Rows != null && rTable.Rows.Count > 0)
                {
                    stats = new List<LatestSightingsLibrary.Stat>();

                    for (int i = 0; i < rTable.Rows.Count; i++)
                    {
                        LatestSightingsLibrary.Stat stat = new LatestSightingsLibrary.Stat();

                        stat.Day = analyticsMonth.Day;
                        stat.Month = analyticsMonth.Month;
                        stat.Year = analyticsMonth.Year;
                        stat.RunDateTime = DateTime.Now;
                        stat.Type = type;
                        stat.VideoId = rTable.Rows[i][0].ToString();
                        stat.Stat1 = rTable.Rows[i][1].ToString();
                        stat.Stat2 = rTable.Rows[i][2].ToString();
                        stat.Stat3 = rTable.Rows[i][3].ToString();
                        stat.Stat4 = rTable.Rows[i][4].ToString();
                        stat.Stat5 = rTable.Rows[i][5].ToString();
                        stat.Ordering = i + 1;
                        stat.Position = i + 1;
                        stats.Add(stat);
                    }
                }
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex, "Error retrieving Top 10 " + type.ToString());
                throw new Exception("Error retrieving Top 10 " + type.ToString());
            }

            return stats;
        }

        private List<LatestSightingsLibrary.Stat> GetCountryViews(int year, int month, LatestSightingsLibrary.Stat.Top10Types type)
        {
            List<LatestSightingsLibrary.Stat> stats = null;

            try
            {
                DateTime analyticsMonth = new DateTime(year, month, 1, 0, 0, 0);
                DateTime analyticsEndDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
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

                Google.Apis.YouTubeAnalytics.v1.ReportsResource.QueryRequest request = service.Reports.Query("contentOwner==" + CustomerOwnerId, analyticsMonth.ToString("yyyy-MM-dd"), analyticsEndDate.ToString("yyyy-MM-dd"), "views,estimatedMinutesWatched,earnings");
                request.Dimensions = "country";
                request.Filters = "uploaderType==self";
                request.MaxResults = 10;
                request.Sort = "-views";
                ResultTable rTable = request.Execute();
                if (rTable != null && rTable.Rows != null && rTable.Rows.Count > 0)
                {
                    stats = new List<LatestSightingsLibrary.Stat>();

                    for (int i = 0; i < rTable.Rows.Count; i++)
                    {
                        LatestSightingsLibrary.Stat stat = new LatestSightingsLibrary.Stat();

                        stat.Day = analyticsMonth.Day;
                        stat.Month = analyticsMonth.Month;
                        stat.Year = analyticsMonth.Year;
                        stat.RunDateTime = DateTime.Now;
                        stat.Type = type;
                        stat.VideoId = string.Empty;
                        stat.Stat1 = rTable.Rows[i][0].ToString();
                        stat.Stat2 = rTable.Rows[i][1].ToString();
                        stat.Stat3 = rTable.Rows[i][2].ToString();
                        stat.Stat4 = rTable.Rows[i][3].ToString();
                        stat.Stat5 = string.Empty;
                        stat.Ordering = i + 1;
                        stat.Position = i + 1;
                        stats.Add(stat);
                    }
                }
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex, "Error retrieving stats for " + type.ToString());
                throw new Exception("Error retrieving stats for " + type.ToString());
            }

            return stats;
        }
    }
}
