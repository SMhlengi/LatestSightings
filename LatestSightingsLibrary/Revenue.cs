using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatestSightingsLibrary
{
    public class Revenue
    {
        private const string SQL_GET_REVENUES = "SELECT * FROM latestsightings.dbo.revenueShares Order By ContributorShare";
        private const string SQL_CHECKFOR_REVENUE = "SELECT COUNT(*) FROM latestsightings.dbo.revenueShares WHERE (contributorShare = @contributorShare);";
        private const string SQL_INSERT_SHARE = "INSERT INTO latestsightings.dbo.revenueShares (id, text, contributorShare) VALUES (@id, @text, @contributorShare);";

        public static List<RevenueShare> GetRevenueShares()
        {
            List<RevenueShare> shares = new List<RevenueShare>();

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand(SQL_GET_REVENUES, conn);
                SqlDataReader rdr = sqlQuery.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        RevenueShare share = new RevenueShare();
                        share.ContributorShare = Convert.ToDouble(rdr["contributorShare"]);
                        share.Id = rdr["id"].ToString();
                        share.Text = rdr["text"].ToString();
                        shares.Add(share);
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

            return shares;
        }

        public static bool AddRevenueShare(double share)
        {
            bool saved = false;

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();

                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = SQL_CHECKFOR_REVENUE;
                sqlQuery.Parameters.Add("contributorShare", System.Data.SqlDbType.VarChar).Value = share;
                int exists = Convert.ToInt32(sqlQuery.ExecuteScalar());

                if (exists <= 0)
                {
                    sqlQuery = new SqlCommand();
                    sqlQuery.Connection = conn;
                    sqlQuery.CommandText = SQL_INSERT_SHARE;
                    sqlQuery.Parameters.Add("id", System.Data.SqlDbType.VarChar).Value = Guid.NewGuid().ToString();
                    sqlQuery.Parameters.Add("text", System.Data.SqlDbType.VarChar).Value = share.ToString() + " / " + (100 - share).ToString();
                    sqlQuery.Parameters.Add("contributorShare", System.Data.SqlDbType.VarChar).Value = share;
                    sqlQuery.ExecuteNonQuery();
                }
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
    }

    public class RevenueShare
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public double ContributorShare { get; set; }
    }
}
