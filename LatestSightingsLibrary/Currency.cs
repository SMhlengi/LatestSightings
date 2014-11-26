using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LatestSightingsLibrary
{
    public class Currency
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string Description { get; set; }

        private const string SQL_GET_CURRENCIES = "SELECT * FROM latestsightings.dbo.currencies WHERE (active = 1) ORDER BY ordering;";

        public static List<Currency> GetCurrencies()
        {
            List<Currency> currencies = null;

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = SQL_GET_CURRENCIES;
                SqlDataReader rdr = sqlQuery.ExecuteReader();
                if (rdr.HasRows)
                {
                    currencies = new List<Currency>();
                    while (rdr.Read())
                    {
                        Currency currency = new Currency();
                        currency.Description = rdr["description"].ToString();
                        currency.Id = Convert.ToInt32(rdr["id"]);
                        currency.Symbol = rdr["symbol"].ToString();
                        currencies.Add(currency);
                    }
                }
                rdr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex);
                throw new Exception("Error retrieving currencies");
            }
            finally
            {
                conn.Dispose();
            }

            return currencies;
        }
    }
}