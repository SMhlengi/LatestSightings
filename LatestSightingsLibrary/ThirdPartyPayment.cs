using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LatestSightingsLibrary
{
    public class ThirdPartyPayment
    {
        public string VideoId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Currency { get; set; }
        public string CurrencyName { get; set; }
        public decimal Payment { get; set; }
        public string ThirdParty { get; set; }

        private const string SQL_DELETE_PAYMENT = "DELETE FROM latestsightings.dbo.thirdPartyPayments WHERE (video = @video AND year = @year AND month = @month AND Currency = @currency AND thirdParty = @party);";
        private const string SQL_INSERT_PAYMENT = "INSERT INTO latestsightings.dbo.thirdPartyPayments (video, year, month, currency, payment, thirdParty) VALUES (@video, @year, @month, @currency, @payment, @party);";
        private const string SQL_GET_PAYMENTS = "SELECT a.*, b.description FROM [latestsightings].[dbo].[thirdPartyPayments] a INNER JOIN [latestsightings].[dbo].[currencies] b ON b.id = a.currency WHERE (a.year = @year AND a.month = @month);";
        private const string SQL_GET_PAYMENTS_BY_ID = "SELECT a.*, b.description FROM [latestsightings].[dbo].[thirdPartyPayments] a INNER JOIN [latestsightings].[dbo].[currencies] b ON b.id = a.currency WHERE (a.video = @video);";
        private const string SQL_GET_PAYMENTS_BY_CONTRIBUTOR = "  SELECT a.year, a.month, a.currency, c.Description As CurrencyName, SUM(a.Payment) AS Total FROM [latestsightings].[dbo].[thirdPartyPayments] a INNER JOIN [latestsightings].[dbo].[videos] b ON b.id = a.video INNER JOIN [latestsightings].[dbo].[currencies] c ON c.id = a.currency WHERE (b.contributor = @contributor) GROUP BY a.year, a.month, a.currency, c.Description";

        public static bool SaveThirdPartyPayment(ThirdPartyPayment payment)
        {
            bool saved = false;

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = SQL_DELETE_PAYMENT;
                sqlQuery.Parameters.Add("video", System.Data.SqlDbType.VarChar).Value = payment.VideoId;
                sqlQuery.Parameters.Add("year", System.Data.SqlDbType.Int).Value = payment.Year;
                sqlQuery.Parameters.Add("month", System.Data.SqlDbType.Int).Value = payment.Month;
                sqlQuery.Parameters.Add("currency", System.Data.SqlDbType.Int).Value = payment.Currency;
                sqlQuery.Parameters.Add("party", System.Data.SqlDbType.VarChar).Value = payment.ThirdParty;
                sqlQuery.ExecuteNonQuery();

                sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = SQL_INSERT_PAYMENT;
                sqlQuery.Parameters.Add("video", System.Data.SqlDbType.VarChar).Value = payment.VideoId;
                sqlQuery.Parameters.Add("year", System.Data.SqlDbType.Int).Value = payment.Year;
                sqlQuery.Parameters.Add("month", System.Data.SqlDbType.Int).Value = payment.Month;
                sqlQuery.Parameters.Add("currency", System.Data.SqlDbType.Int).Value = payment.Currency;
                sqlQuery.Parameters.Add("payment", System.Data.SqlDbType.Decimal).Value = payment.Payment;
                sqlQuery.Parameters.Add("party", System.Data.SqlDbType.VarChar).Value = payment.ThirdParty;
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

        public static List<ThirdPartyPayment> GetThirdPartyPayments(int year, int month)
        {
            List<ThirdPartyPayment> payments = null;

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
                    payments = new List<ThirdPartyPayment>();
                    while (rdr.Read())
                    {
                        ThirdPartyPayment payment = new ThirdPartyPayment();
                        payment.Currency = Convert.ToInt32(rdr["currency"]);
                        payment.CurrencyName = rdr["description"].ToString();
                        payment.Month = Convert.ToInt32(rdr["month"]);
                        payment.Payment = Convert.ToDecimal(rdr["payment"]);
                        payment.VideoId = rdr["video"].ToString();
                        payment.Year = Convert.ToInt32(rdr["year"]);
                        payment.ThirdParty = rdr["thirdParty"].ToString();

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

            if (payments != null && payments.Count > 0)
            {
                payments = GroupPayments(payments);
            }

            return payments;
        }

        public static List<ThirdPartyPayment> GetThirdPartyPayments(string videoId)
        {
            List<ThirdPartyPayment> payments = null;

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = SQL_GET_PAYMENTS_BY_ID;
                sqlQuery.Parameters.Add("video", System.Data.SqlDbType.VarChar).Value = videoId;
                SqlDataReader rdr = sqlQuery.ExecuteReader();
                if (rdr.HasRows)
                {
                    payments = new List<ThirdPartyPayment>();
                    while (rdr.Read())
                    {
                        ThirdPartyPayment payment = new ThirdPartyPayment();
                        payment.Currency = Convert.ToInt32(rdr["currency"]);
                        payment.CurrencyName = rdr["description"].ToString();
                        payment.Month = Convert.ToInt32(rdr["month"]);
                        payment.Payment = Convert.ToDecimal(rdr["payment"]);
                        payment.VideoId = rdr["video"].ToString();
                        payment.Year = Convert.ToInt32(rdr["year"]);
                        payment.ThirdParty = rdr["thirdParty"].ToString();

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

            if (payments != null && payments.Count > 0)
            {
                payments = GroupPayments(payments);
            }

            return payments;
        }

        public static List<ThirdPartyPayment> GetContributorThirdPartyPayments(string contributorId)
        {
            List<ThirdPartyPayment> payments = null;

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = SQL_GET_PAYMENTS_BY_CONTRIBUTOR;
                sqlQuery.Parameters.Add("contributor", System.Data.SqlDbType.VarChar).Value = contributorId;
                SqlDataReader rdr = sqlQuery.ExecuteReader();
                if (rdr.HasRows)
                {
                    payments = new List<ThirdPartyPayment>();
                    while (rdr.Read())
                    {
                        ThirdPartyPayment payment = new ThirdPartyPayment();
                        payment.Currency = Convert.ToInt32(rdr["currency"]);
                        payment.CurrencyName = rdr["CurrencyName"].ToString();
                        payment.Month = Convert.ToInt32(rdr["month"]);
                        payment.Payment = Convert.ToDecimal(rdr["Total"]);
                        payment.VideoId = string.Empty;
                        payment.Year = Convert.ToInt32(rdr["year"]);
                        payment.ThirdParty = string.Empty;

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

            if (payments != null && payments.Count > 0)
            {
                payments = GroupPayments(payments);
            }

            return payments;
        }

        private static List<ThirdPartyPayment> GroupPayments(List<ThirdPartyPayment> payments)
        {
            List<ThirdPartyPayment> sortedPayments = new List<ThirdPartyPayment>();

            if (payments != null && payments.Count > 0)
            {
                List<string> videoIds = payments.Select(x => x.VideoId).Distinct().ToList();
                if (videoIds != null && videoIds.Count > 0)
                {
                    foreach (string id in videoIds)
                    {
                        List<ThirdPartyPayment> videoPayments = payments.Where(x => { return x.VideoId == id; }).ToList();
                        List<int> years = videoPayments.Select(x => x.Year).Distinct().ToList();

                        foreach (int year in years)
                        {
                            List<int> months = videoPayments.Where(x => { return x.Year == year; }).Select(x => x.Month).Distinct().ToList();

                            foreach (int month in months)
                            {
                                List<int> currencies = videoPayments.Where(x => { return x.Year == year && x.Month == month; }).Select(x => x.Currency).Distinct().ToList();

                                foreach (int currency in currencies)
                                {
                                    ThirdPartyPayment payment = videoPayments.FirstOrDefault(x => { return x.Currency == currency && x.Year == year && x.Month == month; });
                                    payment.Payment = videoPayments.Where(x => { return x.Currency == currency && x.Year == year && x.Month == month; }).Sum(x => x.Payment);

                                    sortedPayments.Add(payment);
                                }
                            }
                        }
                    }
                }
            }

            return sortedPayments;
        }
    }
}
