using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatestSightingsLibrary
{
    public class ThirdPartyVideo
    {
        public string Video { get; set; }
        public string ThirdParty { get; set; }
        public string Alias { get; set; }
        public DateTime DateCreated{ get; set; }
        public DateTime DateRemoved { get; set; }

        private const string SQL_GETVIDEO_BYID = "SELECT * FROM latestsightings.dbo.thirdPartyVideos WHERE (video = @video);";
        private const string SQL_CHECK_VIDEO_BYID = "SELECT COUNT(*) FROM latestsightings.dbo.thirdPartyVideos WHERE (video = @video);";
        private const string SQL_INSERT_VIDEO = "INSERT INTO latestsightings.dbo.thirdPartyVideos (video, thirdParty, alias, dateCreated, dateRemoved) VALUES (@video, @thirdParty, @alias, @dateCreated, @dateRemoved);";
        private const string SQL_UPDATE_VIDEO = "UPDATE latestsightings.dbo.thirdPartyVideos SET thirdParty = @thirdParty, alias = @alias, dateCreated = @dateCreated, dateRemoved = @dateRemoved WHERE (video = @video)";

        public static List<ThirdPartyVideo> GetVideos(string videoId)
        {
            List<ThirdPartyVideo> vids = null;

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = SQL_GETVIDEO_BYID;
                sqlQuery.Parameters.Add("video", System.Data.SqlDbType.VarChar).Value = videoId;
                SqlDataReader rdr = sqlQuery.ExecuteReader();
                if (rdr.HasRows)
                {
                    vids = new List<ThirdPartyVideo>();
                    while (rdr.Read())
                    {
                        ThirdPartyVideo vid = new ThirdPartyVideo();
                        vid.Alias = rdr["alias"].ToString();
                        vid.DateCreated = Convert.ToDateTime(rdr["dateCreated"]);
                        vid.DateRemoved = Convert.ToDateTime(rdr["dateRemoved"]);
                        vid.ThirdParty = rdr["thirdParty"].ToString();
                        vid.Video = videoId;
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

        public static bool SaveVideo(ThirdPartyVideo video, string videoId)
        {
            bool saved = false;
            bool exists = VideoExists(videoId);

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = exists == true ? SQL_UPDATE_VIDEO : SQL_INSERT_VIDEO;
                sqlQuery.Parameters.Add("video", System.Data.SqlDbType.VarChar).Value = videoId;
                sqlQuery.Parameters.Add("thirdParty", System.Data.SqlDbType.VarChar).Value = video.ThirdParty;
                sqlQuery.Parameters.Add("alias", System.Data.SqlDbType.VarChar).Value = video.Alias;
                sqlQuery.Parameters.Add("dateCreated", System.Data.SqlDbType.DateTime).Value = video.DateCreated;
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
                sqlQuery.Parameters.Add("video", System.Data.SqlDbType.VarChar).Value = videoId;
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