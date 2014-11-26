using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LatestSightingsLibrary
{
    public class Payment
    {
        public string Contributor { get; set; }
        public bool Paid { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }

        private const string SQL_GET_PAYMENTS = "SELECT * FROM [latestsightings].[dbo].[payments] WHERE (year = @year AND month = @month);";
        private const string SQL_PAYMENT_EXISTS = "SELECT COUNT(*) FROM [latestsightings].[dbo].[payments] WHERE (contributor = @contributor AND year = @year AND month = @month);";
        private const string SQL_INSERT_PAYMENT = "INSERT INTO [latestsightings].[dbo].[payments] (contributor, year, month, paid) VALUES (@contributor, @year, @month, @paid);";
        private const string SQL_UPDATE_PAYMENT = "UPDATE [latestsightings].[dbo].[payments] SET paid = @paid WHERE contributor = @contributor AND year = @year AND month = @month;";

        public static List<Payment> GetPayments(int year, int month)
        {
            List<Payment> payments = null;

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = SQL_GET_PAYMENTS;
                sqlQuery.Parameters.Add("year", System.Data.SqlDbType.Int).Value = year;
                sqlQuery.Parameters.Add("month", System.Data.SqlDbType.Int).Value = month;
                SqlDataReader rdr = sqlQuery.ExecuteReader();
                if (rdr.HasRows)
                {
                    payments = new List<Payment>();
                    while (rdr.Read())
                    {
                        Payment payment = new Payment();
                        payment.Contributor = rdr["contributor"].ToString();
                        payment.Paid = Convert.ToBoolean(rdr["paid"]);

                        payments.Add(payment);
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

            return payments;
        }

        public static bool UpdatePayment(Payment payment)
        {
            bool updated = false;
            bool exists = PaymentExists(payment.Contributor, payment.Year, payment.Month);

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = exists == true ? SQL_UPDATE_PAYMENT : SQL_INSERT_PAYMENT;
                sqlQuery.Parameters.Add("contributor", System.Data.SqlDbType.VarChar).Value = payment.Contributor;
                sqlQuery.Parameters.Add("year", System.Data.SqlDbType.Int).Value = payment.Year;
                sqlQuery.Parameters.Add("month", System.Data.SqlDbType.Int).Value = payment.Month;
                sqlQuery.Parameters.Add("paid", System.Data.SqlDbType.Bit).Value = payment.Paid;
                sqlQuery.ExecuteNonQuery();
                conn.Close();

                updated = true;
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex);
            }
            finally
            {
                conn.Dispose();
            }

            return updated;
        }

        private static bool PaymentExists(string contributor, int year, int month)
        {
            bool exists = false;

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = SQL_PAYMENT_EXISTS;
                sqlQuery.Parameters.Add("contributor", System.Data.SqlDbType.VarChar).Value = contributor;
                sqlQuery.Parameters.Add("year", System.Data.SqlDbType.Int).Value = year;
                sqlQuery.Parameters.Add("month", System.Data.SqlDbType.Int).Value = month;
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
