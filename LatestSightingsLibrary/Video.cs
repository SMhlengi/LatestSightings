using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatestSightingsLibrary
{
    public class Video
    {
        public string Contributor { get; set; }
        public Person Person { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public string Alias { get; set; }
        public DateTime Recieved { get; set; }
        public DateTime IPDate { get; set; }
        public string IPDocument { get; set; }
        public string RevenueShare { get; set; }
        public string Keywords { get; set; }
        public string Region { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }
        public string YoutubeId { get; set; }
        public DateTime DateUploaded { get; set; }
        public DateTime DateRemoved { get; set; }
        public List<ThirdPartyVideo> ThirdParties { get; set; }

        private const string SQL_CHECK_VIDEO_BYID = "SELECT COUNT(Id) FROM latestsightings.dbo.videos WHERE (id = @id);";
        private const string SQL_GET_VIDEO = "SELECT * FROM latestsightings.dbo.videos WHERE (id = @id);";
        private const string SQL_INSERT_VIDEO = "INSERT INTO latestsightings.dbo.videos (contributor, id, title, alias, dateRecieved, ipDate, ipDocument, revenueShare, keywords, region, notes, created, modified, status, youtubeId, dateUploaded, dateRemoved) VALUES (@contributor, @id, @title, @alias, @dateRecieved, @ipDate, @ipDocument, @revenueShare, @keywords, @region, @notes, @created, @modified, @status, @youtubeId, @dateUploaded, @dateRemoved);";
        private const string SQL_UPDATE_VIDEO = "UPDATE latestsightings.dbo.videos SET title = @title, alias = @alias, dateRecieved = @dateRecieved, ipDate = @ipDate, ipDocument = @ipDocument, revenueShare = @revenueShare, keywords = @keywords, region = @region, notes = @notes, modified = @modified, status = @status, youtubeId = @youtubeId, dateUploaded = @dateUploaded, dateRemoved = @dateRemoved WHERE (id = @id);";
        private const string SQL_GET_VIDEOS_COMPACT = "SELECT a.id, a.title, a.alias, a.revenueShare, a.YoutubeId, b.id As Contributor, b.firstname, b.lastname FROM latestsightings.dbo.videos a INNER JOIN latestsightings.dbo.people b ON b.id = a.contributor ORDER BY a.Title";
        private const string SQL_GET_VIDEOS_COMPACT_BYPENDING = "SELECT a.id, a.title, a.alias, a.revenueShare, a.YoutubeId, b.id As Contributor, b.firstname, b.lastname FROM latestsightings.dbo.videos a INNER JOIN latestsightings.dbo.people b ON b.id = a.contributor WHERE (status = 'Pending') ORDER BY a.Title";
        private const string SQL_GET_VIDEOS_COMPACT_BYNOTPENDING = "SELECT a.id, a.title, a.alias, a.revenueShare, a.YoutubeId, b.id As Contributor, b.firstname, b.lastname FROM latestsightings.dbo.videos a INNER JOIN latestsightings.dbo.people b ON b.id = a.contributor WHERE (status <> 'Pending') ORDER BY a.Title";
        private const string SQL_GET_VIDEOS_BYCONTRIBUTOR = "SELECT a.id, a.title, a.alias, a.revenueShare, a.YoutubeId, b.firstname, b.lastname FROM latestsightings.dbo.videos a INNER JOIN latestsightings.dbo.people b ON b.id = a.contributor WHERE (contributor = @contributor) ORDER BY a.Title";
        private const string SQL_INSERT_ANALYTICS = "INSERT INTO latestsightings.dbo.videosAnalytics (videoId, year, month, views, youtubeEarningEstimate, youtubeEarnings, youtubeLastRun) VALUES (@videoId, @year, @month, @views, @youtubeEarningEstimate, @youtubeEarnings, @youtubeLastRun);";
        private const string SQL_UPDATE_ANALYTICS = "UPDATE latestsightings.dbo.videosAnalytics SET videoId = @videoId, views = @views, youtubeEarningEstimate = @youtubeEarningEstimate, youtubeEarnings = @youtubeEarnings, youtubeLastRun = @youtubeLastRun WHERE (videoId = @videoId AND year = @year AND month = @Month);";
        private const string SQL_CHECK_ANALYTICS_BYID = "SELECT COUNT(*) FROM latestsightings.dbo.videosAnalytics WHERE (videoId = @videoId AND Month = @month AND Year = @year);";
        private const string SQL_GET_ANALYTICS = "SELECT * FROM latestsightings.dbo.videosAnalytics WHERE (videoId = @videoId AND year = @year AND month = @month);";
        private const string SQL_GET_ANALYTICS_ALL = "SELECT * FROM latestsightings.dbo.videosAnalytics WHERE (year = @year AND month = @month);";
        private const string SQL_GET_ANALYTICS__ALL_BY_CONTRIBUTOR = "SELECT a.* FROM latestsightings.dbo.videosAnalytics a INNER JOIN latestsightings.dbo.videos b ON b.YoutubeId = a.VideoId WHERE (a.year = @year AND a.month = @month AND a.contributor = @contributor);";
        private const string SQL_GET_ANALYTICS_BY_Id = "SELECT * FROM latestsightings.dbo.videosAnalytics WHERE (videoId = @videoId);";
        private const string SQL_GET_ANALYTICS_BY_CONTRIBUTOR = "  SELECT a.year, a.month, SUM(a.youtubeEarnings) AS Total FROM [latestsightings].[dbo].[videosAnalytics] a INNER JOIN [latestsightings].[dbo].[videos] b ON b.youtubeId = a.videoId WHERE (b.contributor = @contributor) GROUP BY a.year, a.month;";

        public static bool SaveVideo(Video video)
        {
            bool saved = false;
            bool exists = VideoExists(video.Id);

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = exists == true ? SQL_UPDATE_VIDEO : SQL_INSERT_VIDEO;
                sqlQuery.Parameters.Add("id", System.Data.SqlDbType.VarChar).Value = video.Id == null ? string.Empty : video.Id;
                sqlQuery.Parameters.Add("title", System.Data.SqlDbType.VarChar).Value = video.Title == null ? string.Empty : video.Title;
                sqlQuery.Parameters.Add("alias", System.Data.SqlDbType.VarChar).Value = video.Alias == null ? string.Empty : video.Alias;
                sqlQuery.Parameters.Add("dateRecieved", System.Data.SqlDbType.DateTime).Value = video.Recieved == null ? DateTime.Now : video.Recieved;
                sqlQuery.Parameters.Add("ipDate", System.Data.SqlDbType.DateTime).Value = video.IPDate == null ? DateTime.Now : video.IPDate;
                sqlQuery.Parameters.Add("ipDocument", System.Data.SqlDbType.VarChar).Value = video.IPDocument == null ? string.Empty : video.IPDocument;
                sqlQuery.Parameters.Add("revenueShare", System.Data.SqlDbType.VarChar).Value = video.RevenueShare == null ? string.Empty : video.RevenueShare;
                sqlQuery.Parameters.Add("keywords", System.Data.SqlDbType.VarChar).Value = video.Keywords == null ? string.Empty : video.Keywords;
                sqlQuery.Parameters.Add("region", System.Data.SqlDbType.VarChar).Value = video.Region == null ? string.Empty : video.Region;
                sqlQuery.Parameters.Add("notes", System.Data.SqlDbType.VarChar).Value = video.Notes == null ? string.Empty : video.Notes;
                sqlQuery.Parameters.Add("created", System.Data.SqlDbType.DateTime).Value = DateTime.Now;
                sqlQuery.Parameters.Add("modified", System.Data.SqlDbType.DateTime).Value = DateTime.Now;
                sqlQuery.Parameters.Add("status", System.Data.SqlDbType.VarChar).Value = video.Status;
                sqlQuery.Parameters.Add("contributor", System.Data.SqlDbType.VarChar).Value = video.Contributor;
                sqlQuery.Parameters.Add("youtubeId", System.Data.SqlDbType.VarChar).Value = video.YoutubeId;
                sqlQuery.Parameters.Add("dateUploaded", System.Data.SqlDbType.DateTime).Value = video.DateUploaded;
                sqlQuery.Parameters.Add("dateRemoved", System.Data.SqlDbType.DateTime).Value = video.DateRemoved;
                sqlQuery.ExecuteNonQuery();
                conn.Close();

                saved = true;
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex);
            }
            finally
            {
                conn.Dispose();
            }

            if (video.ThirdParties != null && video.ThirdParties.Count > 0)
            {
                foreach (ThirdPartyVideo vid in video.ThirdParties)
                {
                    if (saved)
                        ThirdPartyVideo.SaveVideo(vid, video.Id);
                }
            }

            return saved;
        }

        private static bool VideoExists(string videoId)
        {
            bool exists = false;

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = SQL_CHECK_VIDEO_BYID;
                sqlQuery.Parameters.Add("id", System.Data.SqlDbType.VarChar).Value = videoId;
                int records = Convert.ToInt32(sqlQuery.ExecuteScalar());
                conn.Close();

                if (records > 0)
                    exists = true;
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex);
            }
            finally
            {
                conn.Dispose();
            }

            return exists;
        }

        public static Video GetVideo(string videoId)
        {
            Video vid = null;

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = SQL_GET_VIDEO;
                sqlQuery.Parameters.Add("id", System.Data.SqlDbType.VarChar).Value = videoId;
                SqlDataReader rdr = sqlQuery.ExecuteReader();
                if (rdr.HasRows)
                {
                    vid = new Video();
                    while (rdr.Read())
                    {
                        vid.Contributor = rdr["contributor"].ToString();
                        vid.Id = videoId;
                        vid.Alias = rdr["alias"].ToString();
                        vid.IPDate = Convert.ToDateTime(rdr["ipDate"]);
                        vid.IPDocument = rdr["ipDocument"].ToString();
                        vid.Keywords = rdr["keywords"].ToString();
                        vid.Notes = rdr["notes"].ToString();
                        vid.Recieved = Convert.ToDateTime(rdr["dateRecieved"]);
                        vid.Region = rdr["region"].ToString();
                        vid.RevenueShare = rdr["revenueShare"].ToString();
                        vid.Title = rdr["title"].ToString();
                        vid.Status = rdr["status"].ToString();
                        vid.YoutubeId = rdr["youtubeId"].ToString();
                        vid.DateUploaded = Convert.ToDateTime(rdr["dateUploaded"]);
                        vid.DateRemoved = Convert.ToDateTime(rdr["dateRemoved"]);
                        vid.ThirdParties = ThirdPartyVideo.GetVideos(videoId);
                    }
                }
                rdr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex);
            }
            finally
            {
                conn.Dispose();
            }

            return vid;
        }

        public static List<Video> GetVideosCompact()
        {
            List<Video> videos = null;

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = SQL_GET_VIDEOS_COMPACT;
                SqlDataReader rdr = sqlQuery.ExecuteReader();
                if (rdr.HasRows)
                {
                    videos = new List<Video>();
                    while (rdr.Read())
                    {
                        Video video = new Video();
                        video.Id = rdr["id"].ToString();
                        video.Alias = rdr["alias"].ToString();
                        video.RevenueShare = rdr["revenueShare"].ToString();
                        video.Title = rdr["title"].ToString();
                        video.YoutubeId = rdr["YoutubeId"].ToString();

                        Person person = new Person();
                        person.Id = rdr["Contributor"].ToString();
                        person.FirstName = rdr["firstname"].ToString();
                        person.LastName = rdr["lastname"].ToString();
                        video.Person = person;

                        videos.Add(video);
                    }
                }
                rdr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex);
            }
            finally
            {
                conn.Dispose();
            }

            return videos;
        }

        public static List<Video> GetVideosCompact(string status)
        {
            List<Video> videos = null;

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = status == "Pending" ? SQL_GET_VIDEOS_COMPACT_BYPENDING : SQL_GET_VIDEOS_COMPACT_BYNOTPENDING;
                sqlQuery.Parameters.Add("status", System.Data.SqlDbType.VarChar).Value = status;
                SqlDataReader rdr = sqlQuery.ExecuteReader();
                if (rdr.HasRows)
                {
                    videos = new List<Video>();
                    while (rdr.Read())
                    {
                        Video video = new Video();
                        video.Id = rdr["id"].ToString();
                        video.Alias = rdr["alias"].ToString();
                        video.RevenueShare = rdr["revenueShare"].ToString();
                        video.Title = rdr["title"].ToString();
                        video.YoutubeId = rdr["YoutubeId"].ToString();

                        Person person = new Person();
                        person.FirstName = rdr["firstname"].ToString();
                        person.LastName = rdr["lastname"].ToString();
                        video.Person = person;

                        videos.Add(video);
                    }
                }
                rdr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex);
            }
            finally
            {
                conn.Dispose();
            }

            return videos;
        }

        public static List<Video> GetContributorVideos(string contributor)
        {
            List<Video> videos = null;

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = SQL_GET_VIDEOS_BYCONTRIBUTOR;
                sqlQuery.Parameters.Add("contributor", System.Data.SqlDbType.VarChar).Value = contributor;
                SqlDataReader rdr = sqlQuery.ExecuteReader();
                if (rdr.HasRows)
                {
                    videos = new List<Video>();
                    while (rdr.Read())
                    {
                        Video video = new Video();
                        video.Id = rdr["id"].ToString();
                        video.Alias = rdr["alias"].ToString();
                        video.RevenueShare = rdr["revenueShare"].ToString();
                        video.Title = rdr["title"].ToString();
                        video.YoutubeId = rdr["YoutubeId"].ToString();

                        Person person = new Person();
                        person.FirstName = rdr["firstname"].ToString();
                        person.LastName = rdr["lastname"].ToString();
                        video.Person = person;

                        videos.Add(video);
                    }
                }
                rdr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex);
            }
            finally
            {
                conn.Dispose();
            }

            return videos;
        }

        public static bool SaveYouTubeVideoAnalytics(YouTubeVideoAnalytics video, int year, int month)
        {
            bool saved = false;
            bool exists = YouTubeVideoAnalyticExists(video.Id, year, month);

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = exists == true ? SQL_UPDATE_ANALYTICS : SQL_INSERT_ANALYTICS;
                sqlQuery.Parameters.Add("videoId", System.Data.SqlDbType.VarChar).Value = video.Id == null ? string.Empty : video.Id;
                sqlQuery.Parameters.Add("year", System.Data.SqlDbType.Int).Value = year;
                sqlQuery.Parameters.Add("month", System.Data.SqlDbType.Int).Value = month;
                sqlQuery.Parameters.Add("views", System.Data.SqlDbType.BigInt).Value = video.Views;
                sqlQuery.Parameters.Add("youtubeEarningEstimate", System.Data.SqlDbType.Decimal).Value = video.EstimatedEarning;
                sqlQuery.Parameters.Add("youtubeEarnings", System.Data.SqlDbType.Decimal).Value = video.Earning;
                sqlQuery.Parameters.Add("youtubeLastRun", System.Data.SqlDbType.DateTime).Value = DateTime.Now;
                sqlQuery.ExecuteNonQuery();
                conn.Close();

                saved = true;
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex);
            }
            finally
            {
                conn.Dispose();
            }

            return saved;
        }

        public static YouTubeVideoAnalytics GetYouTubeVideoAnalytics(string videoId, int year, int month)
        {
            YouTubeVideoAnalytics vid = null;

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = SQL_GET_ANALYTICS;
                sqlQuery.Parameters.Add("videoId", System.Data.SqlDbType.VarChar).Value = videoId;
                sqlQuery.Parameters.Add("year", System.Data.SqlDbType.VarChar).Value = year;
                sqlQuery.Parameters.Add("month", System.Data.SqlDbType.VarChar).Value = month;
                SqlDataReader rdr = sqlQuery.ExecuteReader();
                if (rdr.HasRows)
                {
                    vid = new YouTubeVideoAnalytics();
                    while (rdr.Read())
                    {
                        vid.Id = rdr["videoId"].ToString();
                        vid.Earning = Convert.ToDecimal(rdr["youtubeEarnings"]);
                        vid.EstimatedEarning = Convert.ToDecimal(rdr["youtubeEarningEstimate"]);
                        vid.LastRun = Convert.ToDateTime(rdr["youtubeLastRun"]);
                        vid.Views = Convert.ToInt64(rdr["views"]);
                    }
                }
                rdr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex);
            }
            finally
            {
                conn.Dispose();
            }

            return vid;
        }

        public static List<YouTubeVideoAnalytics> GetYouTubeVideoAnalytics(string videoId)
        {
            List<YouTubeVideoAnalytics> vids = null;

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = SQL_GET_ANALYTICS_BY_Id;
                sqlQuery.Parameters.Add("videoId", System.Data.SqlDbType.VarChar).Value = videoId;
                SqlDataReader rdr = sqlQuery.ExecuteReader();
                if (rdr.HasRows)
                {
                    vids = new List<YouTubeVideoAnalytics>();
                    while (rdr.Read())
                    {
                        YouTubeVideoAnalytics vid = new YouTubeVideoAnalytics();
                        vid.Id = rdr["videoId"].ToString();
                        vid.Earning = Convert.ToDecimal(rdr["youtubeEarnings"]);
                        vid.EstimatedEarning = Convert.ToDecimal(rdr["youtubeEarningEstimate"]);
                        vid.LastRun = Convert.ToDateTime(rdr["youtubeLastRun"]);
                        vid.Views = Convert.ToInt64(rdr["views"]);
                        vid.Month = Convert.ToInt32(rdr["month"]);
                        vid.Year = Convert.ToInt32(rdr["year"]);

                        vids.Add(vid);
                    }
                }
                rdr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex);
            }
            finally
            {
                conn.Dispose();
            }

            return vids;
        }

        public static List<YouTubeVideoAnalytics> GetYouTubeVideoAnalytics(int year, int month)
        {
            List<YouTubeVideoAnalytics> videos = null;

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = SQL_GET_ANALYTICS_ALL;
                sqlQuery.Parameters.Add("year", System.Data.SqlDbType.VarChar).Value = year;
                sqlQuery.Parameters.Add("month", System.Data.SqlDbType.VarChar).Value = month;
                SqlDataReader rdr = sqlQuery.ExecuteReader();
                if (rdr.HasRows)
                {
                    videos = new List<YouTubeVideoAnalytics>();
                    while (rdr.Read())
                    {
                        YouTubeVideoAnalytics vid = new YouTubeVideoAnalytics();
                        vid.Id = rdr["videoId"].ToString();
                        vid.Earning = Math.Round(Convert.ToDecimal(rdr["youtubeEarnings"]), 2);
                        vid.EstimatedEarning = Math.Round(Convert.ToDecimal(rdr["youtubeEarningEstimate"]), 2);
                        vid.LastRun = Convert.ToDateTime(rdr["youtubeLastRun"]);
                        vid.Views = Convert.ToInt64(rdr["views"]);
                        videos.Add(vid);
                    }
                }
                rdr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex);
            }
            finally
            {
                conn.Dispose();
            }

            return videos;
        }

        public static List<YouTubeVideoAnalytics> GetContributorYouTubeVideoAnalytics(string contributorId)
        {
            List<YouTubeVideoAnalytics> videos = null;

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = SQL_GET_ANALYTICS_BY_CONTRIBUTOR;
                sqlQuery.Parameters.Add("contributor", System.Data.SqlDbType.VarChar).Value = contributorId;
                SqlDataReader rdr = sqlQuery.ExecuteReader();
                if (rdr.HasRows)
                {
                    videos = new List<YouTubeVideoAnalytics>();
                    while (rdr.Read())
                    {
                        YouTubeVideoAnalytics vid = new YouTubeVideoAnalytics();
                        vid.Id = string.Empty;
                        vid.Earning = Math.Round(Convert.ToDecimal(rdr["Total"]), 2);
                        vid.EstimatedEarning = 0;
                        vid.Month = Convert.ToInt32(rdr["month"]);
                        vid.Year = Convert.ToInt32(rdr["year"]);
                        vid.LastRun = DateTime.Now;
                        vid.Views = 0;
                        videos.Add(vid);
                    }
                }
                rdr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex);
            }
            finally
            {
                conn.Dispose();
            }

            return videos;
        }

        public static List<YouTubeVideoAnalytics> GetYouTubeVideoAnalyticsByContributor(int year, int month, string contributorId)
        {
            List<YouTubeVideoAnalytics> videos = null;

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = SQL_GET_ANALYTICS_ALL;
                sqlQuery.Parameters.Add("year", System.Data.SqlDbType.VarChar).Value = year;
                sqlQuery.Parameters.Add("month", System.Data.SqlDbType.VarChar).Value = month;
                sqlQuery.Parameters.Add("contributor", System.Data.SqlDbType.VarChar).Value = contributorId;
                SqlDataReader rdr = sqlQuery.ExecuteReader();
                if (rdr.HasRows)
                {
                    videos = new List<YouTubeVideoAnalytics>();
                    while (rdr.Read())
                    {
                        YouTubeVideoAnalytics vid = new YouTubeVideoAnalytics();
                        vid.Id = rdr["videoId"].ToString();
                        vid.Earning = Math.Round(Convert.ToDecimal(rdr["youtubeEarnings"]), 2);
                        vid.EstimatedEarning = Math.Round(Convert.ToDecimal(rdr["youtubeEarningEstimate"]), 2);
                        vid.LastRun = Convert.ToDateTime(rdr["youtubeLastRun"]);
                        vid.Views = Convert.ToInt64(rdr["views"]);
                        videos.Add(vid);
                    }
                }
                rdr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex);
            }
            finally
            {
                conn.Dispose();
            }

            return videos;
        }

        public static bool UpDateEarnings(int year, int month)
        {
            bool updated = true;

            LatestSightingsLibrary.Month mnth = LatestSightingsLibrary.Month.GetMonth(year, month);
            if (mnth != null && mnth.ExchangeRate > 0)
            {
                List<LatestSightingsLibrary.Video> vids = LatestSightingsLibrary.Video.GetVideosCompact();
                List<YouTubeVideoAnalytics> videos = GetYouTubeVideoAnalytics(year, month);
                if (videos != null && videos.Count > 0)
                {
                    foreach (YouTubeVideoAnalytics vid in videos)
                    {
                        LatestSightingsLibrary.Video vidRate = vids.FirstOrDefault(x => { return x.YoutubeId == vid.Id; });

                        if (vid.EstimatedEarning > 0 && updated && vidRate != null && !String.IsNullOrEmpty(vidRate.RevenueShare))
                        {
                            try
                            {
                                vid.Earning = Financial.ApplyExchangeRate(vid.EstimatedEarning, mnth.ExchangeRate);
                                vid.Earning = Financial.ApplyRevenueShare(vid.Earning, vidRate.RevenueShare);
                                SaveYouTubeVideoAnalytics(vid, mnth.YearNumber, mnth.MonthNumber);
                            }
                            catch (Exception ex)
                            {
                                updated = false;
                            }
                        }
                    }
                }
            }

            return updated;
        }

        private static bool YouTubeVideoAnalyticExists(string id, int year, int month)
        {
            bool exists = false;

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = SQL_CHECK_ANALYTICS_BYID;
                sqlQuery.Parameters.Add("videoId", System.Data.SqlDbType.VarChar).Value = id;
                sqlQuery.Parameters.Add("year", System.Data.SqlDbType.VarChar).Value = year;
                sqlQuery.Parameters.Add("month", System.Data.SqlDbType.VarChar).Value = month;
                int records = Convert.ToInt32(sqlQuery.ExecuteScalar());
                conn.Close();

                if (records > 0)
                    exists = true;
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex);
            }
            finally
            {
                conn.Dispose();
            }

            return exists;
        }
    }
}