using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LatestSightingsLibrary
{
    public class Financial
    {
        public static decimal GetExchageRate(decimal value1, decimal value2)
        {
            decimal rate = 0;

            rate = Math.Round(value1 / value2, 5);

            return rate;
        }

        public static decimal ApplyExchangeRate(decimal value, decimal exchangeRate)
        {
            decimal newValue = 0;

            newValue = Math.Round(value * exchangeRate, 2, MidpointRounding.AwayFromZero);

            return newValue;
        }

        public static decimal ApplyRevenueShare(decimal value, string share)
        {
            decimal newValue = 0;
            decimal contributorShare = 0;
            bool converted = decimal.TryParse(share.Split('/')[0].Trim(), out contributorShare);
            if (converted)
            {
                newValue = (value / Convert.ToDecimal(100)) * contributorShare;
                newValue = Math.Round(newValue, 2, MidpointRounding.AwayFromZero);
            }

            return newValue;
        }
    }
}
