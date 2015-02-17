using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTubeAnalytics.v1;
using Google.Apis.YouTubeAnalytics.v1.Data;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using LatestSightingsLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;
using System.Collections;

namespace LatestSightingsService
{
    public class YouTubeVideos
    {
        #region Properties
        private int getVideoAnalyticsErrors = 0;

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

        #region Analytics
        public void UpdateVideoAnalytics(int year, int month)
        {
            List<YouTubeVideoAnalytics> videoAnalytics = GetVideoAnalytics(year, month);
            if (videoAnalytics != null && videoAnalytics.Count > 0)
            {
                SaveVideoAnalytics(videoAnalytics, year, month);
            }
        }

        private List<YouTubeVideoAnalytics> GetVideoAnalytics(int year, int month)
        {
            List<LatestSightingsLibrary.Video> videos = GetPublishedVideos();
            List<YouTubeVideoAnalytics> videoAnalytics = null;

            if (videos != null && videos.Count > 0)
            {
                videoAnalytics = new List<YouTubeVideoAnalytics>();

                foreach (LatestSightingsLibrary.Video vid in videos)
                {
                    if (!String.IsNullOrEmpty(vid.YoutubeId) && videoAnalytics.Count < 10)
                    {
                        YouTubeVideoAnalytics vidAnalytics = GetVideoAnalytics(vid.YoutubeId, year, month);
                        if (vidAnalytics != null)
                            videoAnalytics.Add(vidAnalytics);
                    }
                }
            }

            return videoAnalytics;
        }

        private void SaveVideoAnalytics(List<YouTubeVideoAnalytics> videos, int year, int month)
        {
            if (videos != null && videos.Count > 0)
            {
                bool saved = true;

                foreach (YouTubeVideoAnalytics vid in videos)
                {
                    if (saved)
                    {
                        saved = LatestSightingsLibrary.Video.SaveYouTubeVideoAnalytics(vid, year, month);
                    }
                }
            }
        }

        private List<LatestSightingsLibrary.Video> GetPublishedVideos()
        {
            return LatestSightingsLibrary.Video.GetVideosCompact("Published");
        }

        private YouTubeVideoAnalytics GetVideoAnalytics(string id, int year, int month)
        {
            YouTubeVideoAnalytics videoAnalytics = LatestSightingsLibrary.Video.GetYouTubeVideoAnalytics(id, year, month);
            bool run = true;

            if (videoAnalytics != null && videoAnalytics.LastRun > DateTime.Now.AddHours(-12))
                run = false;

            if (run && getVideoAnalyticsErrors < 3)
            {
                try
                {
                    DateTime analyticsMonth = new DateTime(year, month, 1, 0, 0, 0);
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
                    request.Filters = "video==" + id;
                    ResultTable rTable = request.Execute();
                    if (rTable != null && rTable.Rows != null && rTable.Rows.Count > 0)
                    {
                        string earnings = rTable.Rows[0][1].ToString();
                        string views = rTable.Rows[0][2].ToString();

                        if (videoAnalytics == null)
                            videoAnalytics = new YouTubeVideoAnalytics();

                        videoAnalytics.Id = id;
                        videoAnalytics.EstimatedEarning = Convert.ToDecimal(earnings);
                        videoAnalytics.Views = Convert.ToInt64(views);
                    }

                    GetVideoAnalyticsByDay(id, year, month);
                }
                catch (Exception ex)
                {
                    ExHandler.RecordError(ex, "Error getting analytics");
                    videoAnalytics = null;
                }
            }
            else
            {
                videoAnalytics = null;
            }

            return videoAnalytics;
        }

        private void GetVideoAnalyticsByDay(string id, int year, int month)
        {
            try
            {
                DateTime analyticsMonth = new DateTime(year, month, 1, 0, 0, 0);
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
                request.Filters = "video==" + id;
                ResultTable rTable = request.Execute();
                if (rTable != null && rTable.Rows != null && rTable.Rows.Count > 0)
                {
                    List<LatestSightingsLibrary.YouTubeVideoAnalytic> days = new List<LatestSightingsLibrary.YouTubeVideoAnalytic>();

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

                    if (days.Count > 0)
                    {
                        YouTubeVideoAnalytic.SaveVideoDays(days, id);
                    }
                }
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex, "Error getting analytics");
            }
        }
        #endregion

        #region Video Details

        public void UpdateVideoDetails()
        {
            ArrayList vids = GetVideosWitoutData();
            if (vids != null && vids.Count > 0)
            {
                foreach (string vidId in vids)
                {
                    YouTubeVideo vid = GetVideoDetails(vidId);
                    if (vid != null)
                    {
                        YouTubeVideo.SaveVideo(vid);
                    }
                }
            }
        }

        private ArrayList GetVideosWitoutData()
        {
            return LatestSightingsLibrary.YouTubeVideo.GetVideosWitoutData();
        }

        public YouTubeVideo GetVideoDetails(string videoId)
        {
            YouTubeVideo vid = YouTubeVideo.GetVideo(videoId);

            try
            {
                UserCredential credential;
                using (var stream = new FileStream(OAuthfile, FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        new[] { YouTubeService.Scope.Youtube, YouTubeService.Scope.Youtubepartner, YouTubeService.Scope.YoutubepartnerChannelAudit, YouTubeService.Scope.YoutubeReadonly, YouTubeService.Scope.YoutubeUpload, YouTubeAnalyticsService.Scope.YtAnalyticsMonetaryReadonly, YouTubeAnalyticsService.Scope.YtAnalyticsReadonly },
                        "user", CancellationToken.None, new FileDataStore("Drive.Auth.Store")).Result;
                }

                var service = new YouTubeService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ProjectName,
                });

                Google.Apis.YouTube.v3.SearchResource.ListRequest results = service.Search.List("snippet");
                results.Q = videoId;
                results.MaxResults = 1;
                results.Type = "video";
                results.OnBehalfOfContentOwner = CustomerOwnerId;

                SearchListResponse resp = results.Execute();
                if (resp != null && resp.Items != null && resp.Items.Count > 0)
                {
                    foreach (SearchResult result in resp.Items)
                    {
                        if (result != null)
                        {
                            if (vid == null)
                            {
                                vid = new YouTubeVideo();
                                vid.Id = videoId;
                                vid.Created = DateTime.Now;
                            }

                            vid.Description = result.Snippet.Description;
                            vid.Title = result.Snippet.Title;
                            vid.ImageHigh = result.Snippet.Thumbnails.High != null ? result.Snippet.Thumbnails.High.Url : "";
                            vid.ImageDefault = result.Snippet.Thumbnails.Default != null ? result.Snippet.Thumbnails.Default.Url : "";
                            vid.ImageMax = result.Snippet.Thumbnails.Maxres != null ? result.Snippet.Thumbnails.Maxres.Url : "";
                            vid.ImageMedium = result.Snippet.Thumbnails.Medium != null ? result.Snippet.Thumbnails.Medium.Url : "";
                            vid.ImageStandard = result.Snippet.Thumbnails.Standard != null ? result.Snippet.Thumbnails.Standard.Url : "";
                            vid.Updated = DateTime.Now;
                        }
                    }
                }
                else
                {
                    ExHandler.RecordError("Error getting video details for video: " + videoId);
                }
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex, "Error getting video details for video: " + videoId);
                vid = null;
            }

            return vid;
        }

        #endregion
    }
}
