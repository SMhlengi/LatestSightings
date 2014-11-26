using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatestSightings
{
    public class data
    {
        public static bool ExecuteNonQuery(SqlCommand sqlQuery)
        {
            bool executed = true;

            try
            {
                SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString);
                conn.Open();
                sqlQuery.Connection = conn;
                sqlQuery.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                executed = false;
                ExHandler.RecordError(ex);
            }

            return executed;
        }

        public static object ExecuteScalar(SqlCommand sqlQuery)
        {
            object item = null;

            try
            {
                SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString);
                conn.Open();
                sqlQuery.Connection = conn;
                item = sqlQuery.ExecuteScalar();
                conn.Close();
            }
            catch (Exception ex)
            {
                item = null;
                ExHandler.RecordError(ex);
            }

            return item;
        }

        public static SqlConnection Conn()
        {
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString);

            return conn;
        }
    }
}
