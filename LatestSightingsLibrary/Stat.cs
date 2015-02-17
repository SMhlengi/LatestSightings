using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LatestSightingsLibrary
{
    public class Stat
    {
        public enum Top10Types { Earnings, Views, CountryViews }

        public Top10Types Type { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Ordering { get; set; }
        public int Position { get; set; }
        public string VideoId { get; set; }
        public string VideoTitle { get; set; }
        public string Stat1 { get; set; }
        public string Stat2 { get; set; }
        public string Stat3 { get; set; }
        public string Stat4 { get; set; }
        public string Stat5 { get; set; }
        public DateTime RunDateTime { get; set; }

        private const string SQL_DELETE_STATS = "DELETE FROM latestsightings.dbo.stats WHERE (year = @year AND month = @month AND day = @day AND type = @type);";
        private const string SQL_INSERT_STATS = "INSERT INTO latestsightings.dbo.stats (type, year, month, day, ordering, position, videoId, stat1, stat2, stat3, stat4, stat5, runDateTime) VALUES (@type, @year, @month, @day, @ordering, @position, @videoId, @stat1, @stat2, @stat3, @stat4, @stat5, @runDateTime);";
        private const string SQL_GET_STATS = "SELECT * FROM latestsightings.dbo.stats WHERE (Year = @year AND Month = @month AND type = @type) ORDER BY Ordering";
        private const string SQL_GET_STATS_WITHDAY = "SELECT * FROM latestsightings.dbo.stats WHERE (Year = @year AND Month = @month AND day = @day AND type = @type) ORDER BY Ordering";
        private const string SQL_GET_STATS_BYVIDEO = "SELECT a.*, b.Title FROM latestsightings.dbo.stats a INNER JOIN latestsightings.dbo.videos b ON b.youTubeId = a.videoId WHERE (a.Year = @year AND a.Month = @month AND a.type = @type) ORDER BY a.Ordering";
        private const string SQL_GET_STATS_WITHDAY_BYVIDEO = "SELECT a.*, b.Title FROM latestsightings.dbo.stats a INNER JOIN latestsightings.dbo.videos b ON b.youTubeId = a.videoId WHERE (a.Year = @year AND a.Month = @month AND a.day = @day AND a.type = @type) ORDER BY a.Ordering";
        private const string SQL_GET_VIEWS_BYCONTRIBUTOR = "SELECT TOP 10 c.Id, c.firstname, c.lastname, SUM (a.Views) AS TotalViews FROM [latestsightings].[dbo].[videosAnalytics] a INNER JOIN [latestsightings].[dbo].[videos] b ON b.youtubeId = a.videoId INNER JOIN [latestsightings].[dbo].[people] c ON c.id = b.contributor WHERE (a.year = @year AND a.month = @month AND a.views > 0) GROUP BY c.Id, c.firstname, c.lastname ORDER BY TotalViews DESC";

        public static void SaveStats(List<Stat> stats)
        {
            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = SQL_DELETE_STATS;
                sqlQuery.Parameters.Add("year", System.Data.SqlDbType.Int).Value = stats[0].Year;
                sqlQuery.Parameters.Add("month", System.Data.SqlDbType.Int).Value = stats[0].Month;
                sqlQuery.Parameters.Add("day", System.Data.SqlDbType.Int).Value = stats[0].Day;
                sqlQuery.Parameters.Add("type", System.Data.SqlDbType.VarChar).Value = stats[0].Type.ToString();
                sqlQuery.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {

            }

            foreach (Stat stat in stats)
            {
                try
                {
                    conn.Open();
                    SqlCommand sqlQuery = new SqlCommand();
                    sqlQuery.Connection = conn;
                    sqlQuery.CommandText = SQL_INSERT_STATS;
                    sqlQuery.Parameters.Add("year", System.Data.SqlDbType.Int).Value = stat.Year;
                    sqlQuery.Parameters.Add("month", System.Data.SqlDbType.Int).Value = stat.Month;
                    sqlQuery.Parameters.Add("day", System.Data.SqlDbType.Int).Value = stat.Day;
                    sqlQuery.Parameters.Add("type", System.Data.SqlDbType.VarChar).Value = stat.Type.ToString();
                    sqlQuery.Parameters.Add("ordering", System.Data.SqlDbType.Int).Value = stat.Ordering;
                    sqlQuery.Parameters.Add("position", System.Data.SqlDbType.Int).Value = stat.Position;
                    sqlQuery.Parameters.Add("videoId", System.Data.SqlDbType.VarChar).Value = stat.VideoId;
                    sqlQuery.Parameters.Add("stat1", System.Data.SqlDbType.VarChar).Value = stat.Stat1;
                    sqlQuery.Parameters.Add("stat2", System.Data.SqlDbType.VarChar).Value = stat.Stat2;
                    sqlQuery.Parameters.Add("stat3", System.Data.SqlDbType.VarChar).Value = stat.Stat3;
                    sqlQuery.Parameters.Add("stat4", System.Data.SqlDbType.VarChar).Value = stat.Stat4;
                    sqlQuery.Parameters.Add("stat5", System.Data.SqlDbType.VarChar).Value = stat.Stat5;
                    sqlQuery.Parameters.Add("runDateTime", System.Data.SqlDbType.DateTime).Value = stat.RunDateTime;
                    sqlQuery.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    conn.Close();
                }
            }
        }

        public static List<Stat> GetStats(int year, int month, int day, Top10Types type)
        {
            List<Stat> stats = null;

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = day == 0 ? SQL_GET_STATS : SQL_GET_STATS_WITHDAY;
                sqlQuery.Parameters.Add("year", System.Data.SqlDbType.Int).Value = year;
                sqlQuery.Parameters.Add("month", System.Data.SqlDbType.Int).Value = month;
                sqlQuery.Parameters.Add("day", System.Data.SqlDbType.Int).Value = day;
                sqlQuery.Parameters.Add("type", System.Data.SqlDbType.VarChar).Value = type.ToString();
                SqlDataReader rdr = sqlQuery.ExecuteReader();
                if (rdr.HasRows)
                {
                    stats = new List<Stat>();
                    while (rdr.Read())
                    {
                        Stat stat = new Stat();

                        stat.Day = Convert.ToInt32(rdr["day"]);
                        stat.Month = Convert.ToInt32(rdr["month"]);
                        stat.Year = Convert.ToInt32(rdr["year"]);
                        stat.Ordering = Convert.ToInt32(rdr["ordering"]);
                        stat.Position = Convert.ToInt32(rdr["position"]);
                        stat.RunDateTime = Convert.ToDateTime(rdr["runDateTime"]);
                        stat.Stat1 = rdr["stat1"].ToString();
                        stat.Stat2 = rdr["stat2"].ToString();
                        stat.Stat3 = rdr["stat3"].ToString();
                        stat.Stat4 = rdr["stat4"].ToString();
                        stat.Stat5 = rdr["stat5"].ToString();
                        stat.Type = type;
                        stat.VideoId = rdr["videoId"].ToString();

                        stats.Add(stat);
                    }
                }
                rdr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex);
            }

            return stats;
        }

        public static List<Stat> GetVideoStats(int year, int month, int day, Top10Types type)
        {
            List<Stat> stats = null;

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = day == 0 ? SQL_GET_STATS_BYVIDEO : SQL_GET_STATS_WITHDAY_BYVIDEO;
                sqlQuery.Parameters.Add("year", System.Data.SqlDbType.Int).Value = year;
                sqlQuery.Parameters.Add("month", System.Data.SqlDbType.Int).Value = month;
                sqlQuery.Parameters.Add("day", System.Data.SqlDbType.Int).Value = day;
                sqlQuery.Parameters.Add("type", System.Data.SqlDbType.VarChar).Value = type.ToString();
                SqlDataReader rdr = sqlQuery.ExecuteReader();
                if (rdr.HasRows)
                {
                    stats = new List<Stat>();
                    while (rdr.Read())
                    {
                        Stat stat = new Stat();

                        stat.Day = Convert.ToInt32(rdr["day"]);
                        stat.Month = Convert.ToInt32(rdr["month"]);
                        stat.Year = Convert.ToInt32(rdr["year"]);
                        stat.Ordering = Convert.ToInt32(rdr["ordering"]);
                        stat.Position = Convert.ToInt32(rdr["position"]);
                        stat.RunDateTime = Convert.ToDateTime(rdr["runDateTime"]);
                        stat.Stat1 = rdr["stat1"].ToString();
                        stat.Stat2 = rdr["stat2"].ToString();
                        stat.Stat3 = rdr["stat3"].ToString();
                        stat.Stat4 = rdr["stat4"].ToString();
                        stat.Stat5 = rdr["stat5"].ToString();
                        stat.Type = type;
                        stat.VideoId = rdr["videoId"].ToString();
                        stat.VideoTitle = rdr["title"].ToString();

                        stats.Add(stat);
                    }
                }
                rdr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex);
            }

            return stats;
        }

        public static List<Stat> GetUserVideoStats(int year, int month, int day, Top10Types type, string contentOwner)
        {
            List<Stat> stats = null;

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = day == 0 ? SQL_GET_STATS_BYVIDEO : SQL_GET_STATS_WITHDAY_BYVIDEO;
                sqlQuery.Parameters.Add("year", System.Data.SqlDbType.Int).Value = year;
                sqlQuery.Parameters.Add("month", System.Data.SqlDbType.Int).Value = month;
                sqlQuery.Parameters.Add("day", System.Data.SqlDbType.Int).Value = day;
                sqlQuery.Parameters.Add("type", System.Data.SqlDbType.VarChar).Value = type.ToString();
                SqlDataReader rdr = sqlQuery.ExecuteReader();
                if (rdr.HasRows)
                {
                    stats = new List<Stat>();
                    while (rdr.Read())
                    {
                        Stat stat = new Stat();

                        stat.Day = Convert.ToInt32(rdr["day"]);
                        stat.Month = Convert.ToInt32(rdr["month"]);
                        stat.Year = Convert.ToInt32(rdr["year"]);
                        stat.Ordering = Convert.ToInt32(rdr["ordering"]);
                        stat.Position = Convert.ToInt32(rdr["position"]);
                        stat.RunDateTime = Convert.ToDateTime(rdr["runDateTime"]);
                        stat.Stat1 = rdr["stat1"].ToString();
                        stat.Stat2 = rdr["stat2"].ToString();
                        stat.Stat3 = rdr["stat3"].ToString();
                        stat.Stat4 = rdr["stat4"].ToString();
                        stat.Stat5 = rdr["stat5"].ToString();
                        stat.Type = type;
                        stat.VideoId = rdr["videoId"].ToString();
                        stat.VideoTitle = rdr["title"].ToString();

                        stats.Add(stat);
                    }
                }
                rdr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex);
            }

            return stats;
        }

        public static List<Stat> GetContributorViews(int year, int month, int quantity)
        {
            List<Stat> stats = null;

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = SQL_GET_VIEWS_BYCONTRIBUTOR;
                sqlQuery.Parameters.Add("year", System.Data.SqlDbType.Int).Value = year;
                sqlQuery.Parameters.Add("month", System.Data.SqlDbType.Int).Value = month;
                SqlDataReader rdr = sqlQuery.ExecuteReader();
                if (rdr.HasRows)
                {
                    stats = new List<Stat>();
                    int count = 1;
                    while (rdr.Read())
                    {
                        Stat stat = new Stat();

                        stat.Day = 0;
                        stat.Month = month;
                        stat.Year = year;
                        stat.Ordering = count;
                        stat.Position = count;
                        stat.Stat1 = rdr["Id"].ToString();
                        stat.Stat2 = rdr["firstname"].ToString();
                        stat.Stat3 = rdr["lastname"].ToString();
                        stat.Stat4 = rdr["TotalViews"].ToString();
                        stat.Stat5 = "";
                        stat.Type = Top10Types.Views;
                        stat.VideoId = "";
                        stat.VideoTitle = "";

                        stats.Add(stat);
                        count++;
                    }
                }
                rdr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex);
            }

            return stats;
        }
    }
}
