using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatestSightingsLibrary
{
    public class ThirdParty
    {
        public string Id { get; set; }
        public string Name { get; set; }

        private const string SQL_GET_THIRDPARTIES = "SELECT * FROM latestsightings.dbo.thirdParties ORDER BY Name";
        private const string SQL_CHECKFOR_THIRDPARTY = "SELECT COUNT(*) FROM latestsightings.dbo.thirdParties WHERE (name = @name);";
        private const string SQL_INSERT_THIRDPARTY = "INSERT INTO latestsightings.dbo.thirdParties (id, name) VALUES (@id, @name);";

        public static List<ThirdParty> GetThirdParties()
        {
            List<ThirdParty> thirdParties = new List<ThirdParty>();

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand(SQL_GET_THIRDPARTIES, conn);
                SqlDataReader rdr = sqlQuery.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        ThirdParty party = new ThirdParty();
                        party.Id = rdr["id"].ToString();
                        party.Name = rdr["name"].ToString();
                        thirdParties.Add(party);
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

            return thirdParties;
        }

        public static bool AddThirdParty(string name)
        {
            bool saved = false;

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();

                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = SQL_CHECKFOR_THIRDPARTY;
                sqlQuery.Parameters.Add("name", System.Data.SqlDbType.VarChar).Value = name;
                int exists = Convert.ToInt32(sqlQuery.ExecuteScalar());

                if (exists <= 0)
                {
                    sqlQuery = new SqlCommand();
                    sqlQuery.Connection = conn;
                    sqlQuery.CommandText = SQL_INSERT_THIRDPARTY;
                    sqlQuery.Parameters.Add("id", System.Data.SqlDbType.VarChar).Value = Guid.NewGuid().ToString();
                    sqlQuery.Parameters.Add("name", System.Data.SqlDbType.VarChar).Value = name;
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
}
