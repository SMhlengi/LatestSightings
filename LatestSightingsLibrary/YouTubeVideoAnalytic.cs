using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LatestSightingsLibrary
{
    public class YouTubeVideoAnalytic
    {
        public string Id { get; set; }
        public decimal EstimatedEarning { get; set; }
        public long Views { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }

        private const string SQL_DELETE_DAYS = "DELETE FROM latestsightings.dbo.videoAnalyticsByDay WHERE (year = @year AND month = @month AND videoId = 'All Videos');";
        private const string SQL_INSERT_DAYS = "INSERT INTO latestsightings.dbo.videoAnalyticsByDay (videoId, year, month, day, views, earnings) VALUES ('All Videos', @year, @month, @day, @views, @earnings);";
        private const string SQL_DELETE_VIDEO_DAYS = "DELETE FROM latestsightings.dbo.videoAnalyticsByDay WHERE (year = @year AND month = @month AND videoId = @videoId);";
        private const string SQL_INSERT_VIDEO_DAYS = "INSERT INTO latestsightings.dbo.videoAnalyticsByDay (videoId, year, month, day, views, earnings) VALUES (@videoId, @year, @month, @day, @views, @earnings);";
        private const string SQL_GET_DAYS = "SELECT * FROM latestsightings.dbo.videoAnalyticsByDay WHERE (year = @year AND month = @month AND videoId = 'All Videos') ORDER BY Day;";
        private const string SQL_GET_DAYS_BYVIDEO = "SELECT * FROM latestsightings.dbo.videoAnalyticsByDay WHERE (year = @year AND month = @month AND videoId = @videoId) ORDER BY Day;";
        private const string SQL_GET_CONTRIBUTOR_EARNINGS = "SELECT a.year, a.month, a.day, SUM(a.views) AS Views, SUM(a.earnings) AS Earnings FROM [latestsightings].[dbo].[videoAnalyticsByDay] a INNER JOIN [latestsightings].[dbo].[videos] b ON b.youtubeId = a.videoId WHERE (b.contributor = @contributor) AND a.year = @year AND a.month = @month GROUP BY a.year, a.month, a.day ORDER BY a.year, a.month, a.day";

        public static void SaveDays(List<YouTubeVideoAnalytic> days)
        {
            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = SQL_DELETE_DAYS;
                sqlQuery.Parameters.Add("year", System.Data.SqlDbType.Int).Value = days[0].Year;
                sqlQuery.Parameters.Add("month", System.Data.SqlDbType.Int).Value = days[0].Month;
                sqlQuery.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                
            }

            foreach (YouTubeVideoAnalytic day in days)
            {
                try
                {
                    conn.Open();
                    SqlCommand sqlQuery = new SqlCommand();
                    sqlQuery.Connection = conn;
                    sqlQuery.CommandText = SQL_INSERT_DAYS;
                    sqlQuery.Parameters.Add("year", System.Data.SqlDbType.Int).Value = day.Year;
                    sqlQuery.Parameters.Add("month", System.Data.SqlDbType.Int).Value = day.Month;
                    sqlQuery.Parameters.Add("day", System.Data.SqlDbType.Int).Value = day.Day;
                    sqlQuery.Parameters.Add("views", System.Data.SqlDbType.Decimal).Value = day.Views;
                    sqlQuery.Parameters.Add("earnings", System.Data.SqlDbType.Decimal).Value = day.EstimatedEarning;
                    sqlQuery.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    conn.Close();
                }
            }

            conn.Dispose();
        }

        public static void SaveVideoDays(List<YouTubeVideoAnalytic> days, string videoId)
        {
            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = SQL_DELETE_VIDEO_DAYS;
                sqlQuery.Parameters.Add("year", System.Data.SqlDbType.Int).Value = days[0].Year;
                sqlQuery.Parameters.Add("month", System.Data.SqlDbType.Int).Value = days[0].Month;
                sqlQuery.Parameters.Add("videoId", System.Data.SqlDbType.VarChar).Value = videoId;
                sqlQuery.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {

            }

            foreach (YouTubeVideoAnalytic day in days)
            {
                try
                {
                    conn.Open();
                    SqlCommand sqlQuery = new SqlCommand();
                    sqlQuery.Connection = conn;
                    sqlQuery.CommandText = SQL_INSERT_VIDEO_DAYS;
                    sqlQuery.Parameters.Add("year", System.Data.SqlDbType.Int).Value = day.Year;
                    sqlQuery.Parameters.Add("month", System.Data.SqlDbType.Int).Value = day.Month;
                    sqlQuery.Parameters.Add("day", System.Data.SqlDbType.Int).Value = day.Day;
                    sqlQuery.Parameters.Add("views", System.Data.SqlDbType.Decimal).Value = day.Views;
                    sqlQuery.Parameters.Add("earnings", System.Data.SqlDbType.Decimal).Value = day.EstimatedEarning;
                    sqlQuery.Parameters.Add("videoId", System.Data.SqlDbType.VarChar).Value = videoId;
                    sqlQuery.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    conn.Close();
                }
            }

            conn.Dispose();
        }

        public static List<YouTubeVideoAnalytic> GetDays(int year, int month)
        {
            List<YouTubeVideoAnalytic> days = new List<YouTubeVideoAnalytic>();

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = SQL_GET_DAYS;
                sqlQuery.Parameters.Add("year", System.Data.SqlDbType.Int).Value = year;
                sqlQuery.Parameters.Add("month", System.Data.SqlDbType.Int).Value = month;
                SqlDataReader rdr = sqlQuery.ExecuteReader();
                if (rdr.HasRows)
                {
                    days = new List<YouTubeVideoAnalytic>();
                    while (rdr.Read())
                    {
                        YouTubeVideoAnalytic day = new YouTubeVideoAnalytic();
                        day.Day = Convert.ToInt32(rdr["day"]);
                        day.Month = Convert.ToInt32(rdr["month"]);
                        day.Year = Convert.ToInt32(rdr["year"]);
                        day.EstimatedEarning = Convert.ToDecimal(rdr["earnings"]);
                        day.Views = Convert.ToInt64(rdr["views"]);
                        days.Add(day);
                    }
                }
                rdr.Close();
                
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
            }

            return days;
        }

        public static List<YouTubeVideoAnalytic> GetDaysByVideo(int year, int month, string videoId)
        {
            List<YouTubeVideoAnalytic> days = new List<YouTubeVideoAnalytic>();

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = SQL_GET_DAYS_BYVIDEO;
                sqlQuery.Parameters.Add("year", System.Data.SqlDbType.Int).Value = year;
                sqlQuery.Parameters.Add("month", System.Data.SqlDbType.Int).Value = month;
                sqlQuery.Parameters.Add("videoId", System.Data.SqlDbType.VarChar).Value = videoId;
                SqlDataReader rdr = sqlQuery.ExecuteReader();
                if (rdr.HasRows)
                {
                    days = new List<YouTubeVideoAnalytic>();
                    while (rdr.Read())
                    {
                        YouTubeVideoAnalytic day = new YouTubeVideoAnalytic();
                        day.Day = Convert.ToInt32(rdr["day"]);
                        day.Month = Convert.ToInt32(rdr["month"]);
                        day.Year = Convert.ToInt32(rdr["year"]);
                        day.EstimatedEarning = Convert.ToDecimal(rdr["earnings"]);
                        day.Views = Convert.ToInt64(rdr["views"]);
                        days.Add(day);
                    }
                }
                rdr.Close();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
            }

            return days;
        }

        public static List<YouTubeVideoAnalytic> GetVideoTotalsByContributor(int year, int month, string contentOwner)
        {
            List<YouTubeVideoAnalytic> days = new List<YouTubeVideoAnalytic>();

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = SQL_GET_CONTRIBUTOR_EARNINGS;
                sqlQuery.Parameters.Add("year", System.Data.SqlDbType.Int).Value = year;
                sqlQuery.Parameters.Add("month", System.Data.SqlDbType.Int).Value = month;
                sqlQuery.Parameters.Add("contributor", System.Data.SqlDbType.VarChar).Value = contentOwner;
                SqlDataReader rdr = sqlQuery.ExecuteReader();
                if (rdr.HasRows)
                {
                    days = new List<YouTubeVideoAnalytic>();
                    while (rdr.Read())
                    {
                        YouTubeVideoAnalytic day = new YouTubeVideoAnalytic();
                        day.Day = Convert.ToInt32(rdr["day"]);
                        day.Month = Convert.ToInt32(rdr["month"]);
                        day.Year = Convert.ToInt32(rdr["year"]);
                        day.EstimatedEarning = Convert.ToDecimal(rdr["earnings"]);
                        day.Views = Convert.ToInt64(rdr["views"]);
                        days.Add(day);
                    }
                }
                rdr.Close();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
            }

            return days;
        }
    }
}
