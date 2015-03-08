using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Google.Apis.YouTubeAnalytics.v1;
using System.Threading;
using Google.Apis.Util.Store;
using Google.Apis.YouTubeAnalytics.v1.Data;
using System.Collections;
using System.Timers;
using System.Configuration;
using LatestSightingsLibrary;

namespace LatestSightingsService
{
    public partial class Service1 : ServiceBase
    {

        public System.Timers.Timer sysTimer;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            sysTimer = new System.Timers.Timer(5000);
            sysTimer.Elapsed += new ElapsedEventHandler(timerElapsed);
            sysTimer.AutoReset = false;
            sysTimer.Enabled = true;
        }

        protected override void OnStop()
        {
            
        }

        private void timerElapsed(object sender, ElapsedEventArgs e)
        {
            Thread t = new Thread(Run);
            t.IsBackground = true;
            t.Start();
        }

        public void Run1()
        {
            ExHandler.RecordErrorToFile("1");
            DateTime analyticsMonth = new DateTime(2014, 9, 1, 0, 0, 0);
            UserCredential credential;
            using (var stream = new FileStream(@"c:/LatestSightingsService/client_secret_5349507435512.json", FileMode.Open, FileAccess.Read))
            {
                ExHandler.RecordErrorToFile("2");
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { YouTubeAnalyticsService.Scope.YtAnalyticsMonetaryReadonly, YouTubeAnalyticsService.Scope.YtAnalyticsReadonly },
                    "user", CancellationToken.None, new FileDataStore("Drive.Auth.Store")).Result;
                ExHandler.RecordErrorToFile("3");
            }

            ExHandler.RecordErrorToFile("4");

            var service = new YouTubeAnalyticsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Earnings API",
            });

            ExHandler.RecordErrorToFile("5");

            Google.Apis.YouTubeAnalytics.v1.ReportsResource.QueryRequest request = service.Reports.Query("contentOwner==mruLyFmPbuFncZmkTtmBMg", analyticsMonth.ToString("yyyy-MM-dd"), analyticsMonth.ToString("yyyy-MM-dd"), "earnings,views");
            request.Dimensions = "month";
            request.Filters = "claimedStatus==claimed";
            ResultTable rTable = request.Execute();

            ExHandler.RecordErrorToFile("6");

            if (rTable != null && rTable.Rows.Count > 0)
            {
                ExHandler.RecordErrorToFile("7");
                string earnings = rTable.Rows[0][1].ToString();
                string views = rTable.Rows[0][2].ToString();
            }
            ExHandler.RecordErrorToFile("8");
        }

        public void Run()
        {
            YouTubeVideos ytVideos = new YouTubeVideos();
            ytVideos.RecalculateVideos();

            ytVideos.UpdateVideoDetails();

            if (DateTime.Now.Day < 16)
            {
                ytVideos = new YouTubeVideos();
                ytVideos.UpdateVideoAnalytics(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month);
            }

            ytVideos = new YouTubeVideos();
            ytVideos.UpdateVideoAnalytics(DateTime.Now.Year, DateTime.Now.Month);

            Stats statsObj = new Stats();
            ArrayList types = new ArrayList();
            types.Add(LatestSightingsLibrary.Stat.Top10Types.CountryViews);
            types.Add(LatestSightingsLibrary.Stat.Top10Types.Earnings);
            types.Add(LatestSightingsLibrary.Stat.Top10Types.Views);

            Month mnth = new Month();
            if (DateTime.Now.Day < 16)
            {
                mnth = new Month();
                mnth.UpdateMonthAnalytics(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month);

                for (int i = 0; i < types.Count; i++)
                {
                    List<Stat> stats = Stat.GetStats(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 0, (LatestSightingsLibrary.Stat.Top10Types)Enum.Parse(typeof(LatestSightingsLibrary.Stat.Top10Types), types[i].ToString()));
                    if (stats != null && stats.Count > 0)
                    {
                        if (stats[0].RunDateTime.AddDays(1) < DateTime.Now)
                        {
                            statsObj.GetTopTen(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, (LatestSightingsLibrary.Stat.Top10Types)Enum.Parse(typeof(LatestSightingsLibrary.Stat.Top10Types), types[i].ToString()));
                        }
                    }
                    else
                    {
                        statsObj.GetTopTen(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, (LatestSightingsLibrary.Stat.Top10Types)Enum.Parse(typeof(LatestSightingsLibrary.Stat.Top10Types), types[i].ToString()));
                    }
                }
            }

            mnth = new Month();
            mnth.UpdateMonthAnalytics(DateTime.Now.Year, DateTime.Now.Month);

            for (int i = 0; i < types.Count; i++)
            {
                List<Stat> stats = Stat.GetStats(DateTime.Now.Year, DateTime.Now.Month, 0, (LatestSightingsLibrary.Stat.Top10Types)Enum.Parse(typeof(LatestSightingsLibrary.Stat.Top10Types), types[i].ToString()));
                if (stats != null && stats.Count > 0)
                {
                    if (stats[0].RunDateTime.AddDays(1) < DateTime.Now)
                    {
                        statsObj.GetTopTen(DateTime.Now.Year, DateTime.Now.Month, (LatestSightingsLibrary.Stat.Top10Types)Enum.Parse(typeof(LatestSightingsLibrary.Stat.Top10Types), types[i].ToString()));
                    }
                }
                else
                {
                    statsObj.GetTopTen(DateTime.Now.Year, DateTime.Now.Month, (LatestSightingsLibrary.Stat.Top10Types)Enum.Parse(typeof(LatestSightingsLibrary.Stat.Top10Types), types[i].ToString()));
                }
            }

            sysTimer = new System.Timers.Timer(300000);
            sysTimer.Elapsed += new ElapsedEventHandler(timerElapsed);
            sysTimer.AutoReset = false;
            sysTimer.Enabled = true;
        }

        public void oAuth()
        {
            UserCredential credential;
            using (var stream = new FileStream(@"c:/534950743551.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { YouTubeAnalyticsService.Scope.YtAnalyticsMonetaryReadonly, YouTubeAnalyticsService.Scope.YtAnalyticsReadonly },
                    "user", CancellationToken.None, new FileDataStore("Drive.Auth.Store")).Result;
            }

            var service = new YouTubeAnalyticsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Earnings API",
            });

            Dictionary<string, string> videos = new Dictionary<string, string>();
            ArrayList videoIds = new ArrayList();
            videoIds.Add("IE5YsObwBRQ");
            videoIds.Add("0Yjdf3woXl8");
            videoIds.Add("rtkHNMk2jdE");
            videoIds.Add("UN93zomwXQI");
            videoIds.Add("wmtpQF6gKro");
            videoIds.Add("zSEpuO4wdQc");
            videoIds.Add("gvBvUiIRSPc");
            videoIds.Add("XyOoX9nmswI");
            videoIds.Add("ZWzkkRzt20s");
            videoIds.Add("r7DNfYoZgrw");
            videoIds.Add("wd5MvcwLgXE");

            for (int i = 0; i < videoIds.Count; i++)
            {
                Google.Apis.YouTubeAnalytics.v1.ReportsResource.QueryRequest request = service.Reports.Query("contentOwner==mruLyFmPbuFncZmkTtmBMg", "2014-09-01", "2014-09-01", "earnings,views");
                request.Dimensions = "month";
                request.Filters = "video==" + videoIds[i];
                ResultTable rTable = request.Execute();
                if (rTable != null && rTable.Rows.Count > 0)
                {
                    string earnings = rTable.Rows[0][1].ToString();
                    string views = rTable.Rows[0][2].ToString();
                    videos.Add(earnings, views);
                }
            }

            foreach (KeyValuePair<string, string> item in videos)
            {
                string earnings = item.Key;
                string views = item.Value;
            }

            //Google.Apis.YouTubeAnalytics.v1.ReportsResource.QueryRequest request = service.Reports.Query("contentOwner==mruLyFmPbuFncZmkTtmBMg", "2014-09-01", "2014-09-01", "earnings");
            //request.Dimensions = "month";
            //request.Filters = "video==IE5YsObwBRQ";
            //ResultTable aszsd = request.Execute();
        }

        public void oAuth3()
        {
            UserCredential credential;
            using (var stream = new FileStream(@"c:/470619179171.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { YouTubeService.Scope.Youtubepartner, YouTubeService.Scope.YoutubeReadonly, YouTubeService.Scope.YoutubepartnerChannelAudit },
                    "user", CancellationToken.None, null).Result;
            }

            var service = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "My Project 1",
            });

            ChannelsResource.ListRequest channelRequest = service.Channels.List("contentDetails");
            channelRequest.ManagedByMe = true;
            channelRequest.OnBehalfOfContentOwner = "mruLyFmPbuFncZmkTtmBMg";
            ChannelListResponse channels = channelRequest.Execute();
        }

        public void oAuth2()
        {
            UserCredential credential;
            using (var stream = new FileStream(@"c:/534950743551.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { YouTubeService.Scope.Youtube, YouTubeService.Scope.YoutubeReadonly, YouTubeService.Scope.Youtubepartner, YouTubeService.Scope.YoutubeReadonly, YouTubeService.Scope.YoutubepartnerChannelAudit },
                    "user", CancellationToken.None, null).Result;
            }

            // Create the service.
            var service = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "My Project 1",
            });

            //using (var stream = new FileStream(@"c:/470619179171.json", FileMode.Open, FileAccess.Read))
            //{
            //    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            //        GoogleClientSecrets.Load(stream).Secrets,
            //        new[] { YouTubeService.Scope.Youtube },
            //        "user", CancellationToken.None, null).Result;
            //}

            //// Create the service.
            //var service = new YouTubeService(new BaseClientService.Initializer()
            //{
            //    HttpClientInitializer = credential,
            //    ApplicationName = "My Project",
            //});

            //ChannelsResource.ListRequest channelRequest = service.Channels.List("contentDetails");
            //SearchResource AS = service.Search();
            //channelRequest.ManagedByMe = true;
            //channelRequest.OnBehalfOfContentOwner = "407408718192";
            VideosResource.ListRequest videosRequest = service.Videos.List("snippet");
            VideoListResponse aszsd = videosRequest.Execute();
            //Google.Apis.YouTubeAnalytics.v1.ReportsResource.QueryRequest request = service.Reports.Query("contentOwner==yfZleh4w7buTzi0WfY8WqA", "2014-01-01", "2014-01-31", "views");
            //ResultTable aszsd = request.Execute();
        }

        public void oAuth1()
        {
            String serviceAccountEmail = "680041534981-aqrukbdlhmhup3tmv17nb3n73hqb8gfb@developer.gserviceaccount.com";

            var certificate = new X509Certificate2(@"c:\2f75f0d58468.p12", "notasecret", X509KeyStorageFlags.Exportable);

            ServiceAccountCredential credential = new ServiceAccountCredential(
               new ServiceAccountCredential.Initializer(serviceAccountEmail)
               {
                   Scopes = new[] { YouTubeAnalyticsService.Scope.YtAnalyticsMonetaryReadonly }
               }.FromCertificate(certificate));

            // Create the service.
            var service = new YouTubeAnalyticsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "My Project",
            });

            Google.Apis.YouTubeAnalytics.v1.ReportsResource.QueryRequest request = service.Reports.Query("contentOwner==yfZleh4w7buTzi0WfY8WqA", "2014-01-01", "2014-01-31", "views");
            ResultTable aszsd = request.Execute();
            
            //ChannelsResource.ListRequest listRequest = service.Channels.List("id, snippet");
            //ChannelListResponse resp = listRequest.Execute();
            //foreach (Channel result in resp.Items)
            //{
            //    //Console.WriteLine(result.Snippet.Title);
            //}
        }
    }
}