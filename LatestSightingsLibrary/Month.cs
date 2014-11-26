using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatestSightingsLibrary
{

    public class Month
    {
        public int YearNumber { get; set; }
        public int MonthNumber { get; set; }
        public decimal EstimatedEarnings { get; set; }
        public decimal EstimatedEarningsSupplied { get; set; }
        public decimal Earnings { get; set; }
        public decimal ExchangeRate { get; set; }
        public DateTime EstimatedDate { get; set; }
        public DateTime CalculationDate { get; set; }
        public long Views { get; set; }

        private const string SQL_GET_MONTH = "SELECT * FROM latestsightings.dbo.months WHERE (year = @year AND month = @month);";
        private const string SQL_CHECK_MONTH = "SELECT COUNT(*) FROM latestsightings.dbo.months WHERE (year = @year AND month = @month);";
        private const string SQL_INSERT_MONTH = "INSERT INTO latestsightings.dbo.months (year, month, estimatedEarnings, suppliedEstimatedEarnings, earnings, exchangeRate, estimatedDate, calculationDate, views) VALUES (@year, @month, @estimatedEarnings, @suppliedEstimatedEarnings, @earnings, @exchangeRate, @estimatedDate, @calculationDate, @views);";
        private const string SQL_UPDATE_MONTH = "UPDATE latestsightings.dbo.months SET estimatedEarnings = @estimatedEarnings, suppliedEstimatedEarnings = @suppliedEstimatedEarnings, earnings = @earnings, exchangeRate = @exchangeRate, estimatedDate = @estimatedDate, calculationDate = @calculationDate, views = @views WHERE (year = @year AND month = @month);";

        public static Month GetMonth(int yearNumber, int monthNumber)
        {
            Month mnth = null;

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = SQL_GET_MONTH;
                sqlQuery.Parameters.Add("year", System.Data.SqlDbType.Int).Value = yearNumber;
                sqlQuery.Parameters.Add("month", System.Data.SqlDbType.Int).Value = monthNumber;
                SqlDataReader rdr = sqlQuery.ExecuteReader();
                if (rdr.HasRows)
                {
                    mnth = new Month();
                    while (rdr.Read())
                    {
                        mnth.Earnings = Convert.ToDecimal(rdr["earnings"]);
                        mnth.EstimatedEarnings = Convert.ToDecimal(rdr["estimatedEarnings"]);
                        mnth.EstimatedEarningsSupplied = Convert.ToDecimal(rdr["suppliedEstimatedEarnings"]);
                        mnth.ExchangeRate = Convert.ToDecimal(rdr["exchangeRate"]);
                        mnth.MonthNumber = monthNumber;
                        mnth.YearNumber = yearNumber;
                        mnth.EstimatedDate = Convert.ToDateTime(rdr["estimatedDate"]);
                        mnth.CalculationDate = Convert.ToDateTime(rdr["calculationDate"]);
                        mnth.Views = Convert.ToInt64(rdr["views"]);
                    }
                }
                rdr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex);
                throw new Exception("Error retrieving month");
            }
            finally
            {
                conn.Dispose();
            }

            return mnth;
        }

        public static bool SaveMonth(Month mnth)
        {
            bool saved = false;
            bool exists = MonthExists(mnth.YearNumber, mnth.MonthNumber);

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = exists == true ? SQL_UPDATE_MONTH : SQL_INSERT_MONTH;
                sqlQuery.Parameters.Add("year", System.Data.SqlDbType.Int).Value = mnth.YearNumber;
                sqlQuery.Parameters.Add("month", System.Data.SqlDbType.Int).Value = mnth.MonthNumber;
                sqlQuery.Parameters.Add("estimatedEarnings", System.Data.SqlDbType.Decimal).Value = mnth.EstimatedEarnings;
                sqlQuery.Parameters.Add("suppliedEstimatedEarnings", System.Data.SqlDbType.Decimal).Value = mnth.EstimatedEarningsSupplied;
                sqlQuery.Parameters.Add("earnings", System.Data.SqlDbType.Decimal).Value = mnth.Earnings;
                sqlQuery.Parameters.Add("exchangeRate", System.Data.SqlDbType.Decimal).Value = mnth.ExchangeRate;
                sqlQuery.Parameters.Add("estimatedDate", System.Data.SqlDbType.DateTime).Value = mnth.EstimatedDate;
                sqlQuery.Parameters.Add("calculationDate", System.Data.SqlDbType.DateTime).Value = mnth.CalculationDate;
                sqlQuery.Parameters.Add("views", System.Data.SqlDbType.BigInt).Value = mnth.Views;
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

        private static bool MonthExists(int year, int month)
        {
            bool exists = false;

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = SQL_CHECK_MONTH;
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

        public static Month GetNewMonth(int year, int month)
        {
            Month mnth = new Month();

            mnth = new LatestSightingsLibrary.Month();
            mnth.Earnings = 0;
            mnth.CalculationDate = new DateTime(1900, 1, 1);
            mnth.EstimatedDate = new DateTime(1900, 1, 1);
            mnth.EstimatedEarnings = 0;
            mnth.EstimatedEarningsSupplied = 0;
            mnth.MonthNumber = month;
            mnth.YearNumber = year;
            mnth.Views = 0;
            mnth.ExchangeRate = 0;

            return mnth;
        }
    }
}