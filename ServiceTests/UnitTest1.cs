using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LatestSightingsLibrary;
using System.Collections.Generic;

namespace ServiceTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Run()
        {
            LatestSightingsService.Service1 ls = new LatestSightingsService.Service1();
            ls.Run();
            Assert.AreEqual(0, 0);
        }

        [TestMethod]
        public void UpdateMonthAnalytics()
        {
            LatestSightingsService.Month mnth = new LatestSightingsService.Month();
            mnth.UpdateMonthAnalytics(DateTime.Now.Year, DateTime.Now.Month);
            Assert.AreEqual(0, 0);
        }

        [TestMethod]
        public void UpdateVideoAnalytics()
        {
            LatestSightingsService.YouTubeVideos youTubeVideos = new LatestSightingsService.YouTubeVideos();
            youTubeVideos.UpdateVideoAnalytics(DateTime.Now.Year, DateTime.Now.Month);
            Assert.AreEqual(0, 0);
        }

        [TestMethod]
        public void TestOAUth()
        {
            LatestSightingsService.Service1 ls = new LatestSightingsService.Service1();
            ls.oAuth();
            Assert.AreEqual(0, 0);
        }

        [TestMethod]
        public void TestOAUth2()
        {
            LatestSightingsService.Service1 ls = new LatestSightingsService.Service1();
            ls.oAuth2();
            Assert.AreEqual(0, 0);
        }

        [TestMethod]
        public void TestUpdateMonthAnalytics()
        {
            LatestSightingsService.Month mnth = new LatestSightingsService.Month();
            mnth.UpdateMonthAnalytics(DateTime.Now.Year, DateTime.Now.Month);
            Assert.AreEqual(0, 0);
        }

        [TestMethod]
        public void TestUpdateMonthAnalyticsByDay()
        {
            LatestSightingsService.Month mnth = new LatestSightingsService.Month();
            mnth.UpdateMonthAnalyticsByDay(DateTime.Now.Year, DateTime.Now.Month);
            Assert.AreEqual(0, 0);
        }

        [TestMethod]
        public void TestTopTenEarnings()
        {
            LatestSightingsService.Stats stats = new LatestSightingsService.Stats();
            stats.GetTopTen(DateTime.Now.Year, DateTime.Now.Month, LatestSightingsLibrary.Stat.Top10Types.Earnings);
            Assert.AreEqual(0, 0);
        }

        [TestMethod]
        public void TestTopTenViews()
        {
            LatestSightingsService.Stats stats = new LatestSightingsService.Stats();
            stats.GetTopTen(DateTime.Now.Year, DateTime.Now.Month, LatestSightingsLibrary.Stat.Top10Types.Views);
            Assert.AreEqual(0, 0);
        }

        [TestMethod]
        public void TestTopCountryViews()
        {
            LatestSightingsService.Stats stats = new LatestSightingsService.Stats();
            stats.GetTopTen(DateTime.Now.Year, DateTime.Now.Month, LatestSightingsLibrary.Stat.Top10Types.CountryViews);
            Assert.AreEqual(0, 0);
        }

        [TestMethod]
        public void TestTopContributorViews()
        {
            List<LatestSightingsLibrary.Stat> stats = LatestSightingsLibrary.Stat.GetContributorViews(2014, 10, 10);
            Assert.AreEqual(stats.Count, 1);
        }

        [TestMethod]
        public void TestGetVideoDetails()
        {
            LatestSightingsService.YouTubeVideos vids = new LatestSightingsService.YouTubeVideos();
            YouTubeVideo vid = vids.GetVideoDetails("dnKn9bCO2HQ");
            YouTubeVideo.SaveVideo(vid);
            Assert.AreEqual(0, 0);
        }
    }
}