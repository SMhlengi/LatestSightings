using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LatestSightingsLibrary
{
    public class YouTubeVideo
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageHigh { get; set; }
        public string ImageDefault { get; set; }
        public string ImageMax { get; set; }
        public string ImageMedium { get; set; }
        public string ImageStandard { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        private const string SQL_GET_VIDEO_BYID = "SELECT * FROM latestsightings.dbo.youTubeVideo WHERE (id = @id);";
        private const string SQL_CHECK_VIDEO_BYID = "SELECT COUNT(Id) FROM latestsightings.dbo.youTubeVideo WHERE (id = @id);";
        private const string SQL_INSERT_VIDEO = "INSERT INTO latestsightings.dbo.youTubeVideo (id ,title, description, imageHigh, imageDefault, imageMax, imageMedium, imageStandard, created, updated) VALUES (@id , @title, @description, @imageHigh, @imageDefault, @imageMax, @imageMedium, @imageStandard, @created, @updated);";
        private const string SQL_UPDATE_VIDEO = "UPDATE latestsightings.dbo.youTubeVideo SET title = @title, description = @description, imageHigh = @imageHigh, imageDefault = @imageDefault, imageMax = @imageMax, imageMedium = @imageMedium, imageStandard = @imageStandard, created = @created, updated = @updated WHERE (id = @id);";
        private const string SQL_GET_EMPTYVIDEOS = "SELECT a.youtubeId, (SELECT TOP 1 b.Id FROM latestsightings.dbo.youTubeVideo b WHERE b.id = a.youTubeId) As id FROM latestsightings.dbo.videos a WHERE (a.youtubeId <> '' AND a.status = 'Published')";

        public static void SaveVideo(YouTubeVideo video)
        {
            bool exists = VideoExists(video.Id);

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = exists == true ? SQL_UPDATE_VIDEO : SQL_INSERT_VIDEO;
                sqlQuery.Parameters.Add("id", System.Data.SqlDbType.VarChar).Value = video.Id;
                sqlQuery.Parameters.Add("title", System.Data.SqlDbType.VarChar).Value = video.Title;
                sqlQuery.Parameters.Add("description", System.Data.SqlDbType.VarChar).Value = video.Description;
                sqlQuery.Parameters.Add("imageHigh", System.Data.SqlDbType.VarChar).Value = video.ImageHigh;
                sqlQuery.Parameters.Add("imageDefault", System.Data.SqlDbType.VarChar).Value = video.ImageDefault;
                sqlQuery.Parameters.Add("imageMax", System.Data.SqlDbType.VarChar).Value = video.ImageMax;
                sqlQuery.Parameters.Add("imageMedium", System.Data.SqlDbType.VarChar).Value = video.ImageMedium;
                sqlQuery.Parameters.Add("imageStandard", System.Data.SqlDbType.VarChar).Value = video.ImageStandard;
                sqlQuery.Parameters.Add("created", System.Data.SqlDbType.DateTime).Value = video.Created;
                sqlQuery.Parameters.Add("updated", System.Data.SqlDbType.DateTime).Value = video.Updated;
                sqlQuery.ExecuteNonQuery();
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
        }

        public static YouTubeVideo GetVideo(string videoId)
        {
            YouTubeVideo vid = null;

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = SQL_GET_VIDEO_BYID;
                sqlQuery.Parameters.Add("id", System.Data.SqlDbType.VarChar).Value = videoId;
                SqlDataReader rdr = sqlQuery.ExecuteReader();
                if (rdr.HasRows)
                {
                    vid = new YouTubeVideo();
                    while (rdr.Read())
                    {
                        vid.Id = videoId;
                        vid.Title = rdr["title"].ToString();
                        vid.Description = rdr["description"].ToString();
                        vid.ImageDefault = rdr["imageDefault"].ToString();
                        vid.ImageHigh = rdr["imageHigh"].ToString();
                        vid.ImageMax = rdr["imageMax"].ToString();
                        vid.ImageMedium = rdr["imageMedium"].ToString();
                        vid.ImageStandard = rdr["imageStandard"].ToString();
                        vid.Created = Convert.ToDateTime(rdr["created"]);
                        vid.Updated = Convert.ToDateTime(rdr["updated"]);
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

        public static ArrayList GetVideosWitoutData()
        {
            ArrayList vids = null;

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = SQL_GET_EMPTYVIDEOS;
                SqlDataReader rdr = sqlQuery.ExecuteReader();
                if (rdr.HasRows)
                {
                    vids = new ArrayList();
                    while (rdr.Read())
                    {
                        if (rdr["id"] == DBNull.Value)
                        {
                            vids.Add(rdr["youtubeId"]);
                        }
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
    }
}
