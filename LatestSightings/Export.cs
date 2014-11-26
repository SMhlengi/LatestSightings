using CsvHelper;
using LatestSightingsLibrary;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;

namespace LatestSightings
{
    public class Export
    {
        public static void ExportToCsv(string fileName, List<PaymentDataItem> items)
        {
            List<Payment> payments = Payment.GetPayments(2014, 9);
            using (var sw = new StreamWriter(HttpContext.Current.Server.MapPath("~/files") + "/" + fileName))
            {
                var writer = new CsvWriter(sw);

                writer.WriteField("Name");
                List<Currency> currencies = Currency.GetCurrencies();
                if (currencies != null)
                {
                    foreach (Currency currency in currencies)
                    {
                        writer.WriteField(currency.Description);
                    }
                }
                writer.NextRecord();

                foreach (PaymentDataItem item in items)
                {
                    writer.WriteField(item.ContributorName);
                    if (currencies != null)
                    {
                        int count = 1;
                        foreach (Currency currency in currencies)
                        {
                            writer.WriteField(item.GetType().GetProperty("Currency" + currency.Id).GetValue(item, null));
                            count++;
                        }
                    }
                    writer.NextRecord();
                }
            }
        }
    }
}