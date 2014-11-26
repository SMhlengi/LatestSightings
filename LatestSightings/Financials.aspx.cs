using LatestSightingsLibrary;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LatestSightings
{
    public partial class Financials : System.Web.UI.Page
    {
        public int year = DateTime.Now.Year;
        public int month = DateTime.Now.Month;
        public string currencyScripts = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            var master = Master as DefaultMaster;
            if (master != null)
            {
                master.SetHeader("Video Payments Received", "money");
                master.SetActiveNav("financials");
            }

            SetPageValues();
            FillCurrencies();
            FillThirdParties();

            if (!Page.IsPostBack)
            {
                txtMonthPicker.Text = new DateTime(year, month, 1).ToString("MMMM yyyy");
                SetMonthEarnings();
            }
        }

        protected void Calculate(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtRecieved.Text))
            {
                decimal recieved = 0;
                decimal over = 0;
                bool converted = decimal.TryParse(txtRecieved.Text, out recieved);
                converted = decimal.TryParse(txtOverRide.Text, out over);

                LatestSightingsLibrary.Month mnth = LatestSightingsLibrary.Month.GetMonth(year, month);
                mnth.Earnings = recieved;
                mnth.EstimatedEarningsSupplied = over;
                if (recieved > 0)
                {
                    mnth.ExchangeRate = over > 0 ? LatestSightingsLibrary.Financial.GetExchageRate(mnth.Earnings, mnth.EstimatedEarningsSupplied) : LatestSightingsLibrary.Financial.GetExchageRate(mnth.Earnings, mnth.EstimatedEarnings);
                    bool saved = LatestSightingsLibrary.Month.SaveMonth(mnth);
                    if (saved)
                    {
                        bool updated = LatestSightingsLibrary.Video.UpDateEarnings(year, month);
                        if (updated)
                        {
                            UserInfo.AddAlert("Earnings successfully calculated", GritterMessage.GritterMessageType.success);
                        }
                        else
                        {
                            UserInfo.AddAlert("Error updating earnings", GritterMessage.GritterMessageType.error);
                        }
                    }
                }
                else
                {
                    mnth.ExchangeRate = 0;
                    UserInfo.AddAlert("Error updating earnings", GritterMessage.GritterMessageType.error);
                }

                if (HttpContext.Current.Cache["financialsDataVideos" + year.ToString() + "_" + month.ToString()] != null)
                {
                    HttpContext.Current.Cache.Remove("financialsDataVideos" + year.ToString() + "_" + month.ToString());
                }
                 if (HttpContext.Current.Cache["financialsDataVideos" + year.ToString() + "_" + month.ToString()] != null)
                {
                    HttpContext.Current.Cache.Remove("financialsDataVideos" + year.ToString() + "_" + month.ToString());
                }

                SetMonthEarnings();
            }
        }

        private void SetMonthEarnings()
        {
            LatestSightingsLibrary.Month mnth = LatestSightingsLibrary.Month.GetMonth(year, month);
            if (mnth != null)
            {
                if (mnth.EstimatedEarnings > 0)
                {
                    txtEstimated.Text = Math.Round(mnth.EstimatedEarnings, 2).ToString();
                }
                else
                {
                    txtEstimated.Text = "0";
                }
                if (mnth.EstimatedEarningsSupplied > 0)
                {
                    txtOverRide.Text = Math.Round(mnth.EstimatedEarningsSupplied, 2).ToString();
                }
                else
                {
                    txtOverRide.Text = "0";
                }
                if (mnth.Earnings > 0)
                {
                    txtRecieved.Text = Math.Round(mnth.Earnings, 2).ToString();
                }
                else
                {
                    txtRecieved.Text = "0";
                }
                if (mnth.ExchangeRate > 0)
                {
                    txtExchange.Text = Math.Round(mnth.ExchangeRate, 5).ToString();
                }
                else
                {
                    txtExchange.Text = "";
                }
            }
            else
            {
                txtEstimated.Text = "0";
                txtOverRide.Text = "0";
                txtRecieved.Text = "0";
            }
        }

        protected void ChangeMonth(object sender, EventArgs e)
        {
            int month = ResolveMonthNumber(txtMonthPicker.Text.Split(' ')[0]);
            int year = Convert.ToInt32(txtMonthPicker.Text.Split(' ')[1]);
            HttpContext.Current.Response.Redirect("/financials/" + year.ToString() + "/" + month.ToString());
        }

        private int ResolveMonthNumber(string monthName)
        {
            return DateTime.ParseExact(monthName, "MMMM", CultureInfo.CurrentCulture).Month;
        }

        private void SetPageValues()
        {
            if (!String.IsNullOrEmpty(Request.QueryString["year"]))
                year = Convert.ToInt32(Request.QueryString["year"]);

            if (Page.RouteData.Values["year"] != null && !String.IsNullOrEmpty(Page.RouteData.Values["year"].ToString()))
                year = Convert.ToInt32(Page.RouteData.Values["year"].ToString());

            if (!String.IsNullOrEmpty(Request.QueryString["month"]))
                month = Convert.ToInt32(Request.QueryString["month"]);

            if (Page.RouteData.Values["month"] != null && !String.IsNullOrEmpty(Page.RouteData.Values["month"].ToString()))
                month = Convert.ToInt32(Page.RouteData.Values["month"].ToString());

            SetCurrencies();
        }

        private void SetCurrencies()
        {
            List<Currency> currencies = Currency.GetCurrencies();
            if (currencies != null)
            {
                int count = 1;
                StringBuilder sb = new StringBuilder();
                foreach (Currency currency in currencies)
                {
                    currencyScripts += "{ \"data\": \"Currency" + count.ToString() + "\", orderable: true },";
                    sb.Append("<th style=\"background-color:#554337; color: #FFFFFF; border-bottom: 0px;\">" + currency.Description + "</th>" + Environment.NewLine);
                    count++;
                }
                ltlHeader.Text = sb.ToString();
                ltlCurrencies.Text = sb.ToString();
            }
        }

        private void FillCurrencies()
        {
            List<Currency> currencies = Currency.GetCurrencies();
            ddlCurrency.DataSource = currencies;
            ddlCurrency.DataTextField = "Description";
            ddlCurrency.DataValueField = "Id";
            ddlCurrency.DataBind();
            ddlCurrency.Items.Insert(0, new ListItem("Select Currency...", ""));
        }

        private void FillThirdParties()
        {
            List<ThirdParty> parties = ThirdParty.GetThirdParties();
            ddlThirdParty.DataSource = parties;
            ddlThirdParty.DataTextField = "Name";
            ddlThirdParty.DataValueField = "Id";
            ddlThirdParty.DataBind();
            ddlThirdParty.Items.Insert(0, new ListItem("Select Third Party...", ""));
        }

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public static void AddThirdPartyPayment(string id, string currency, string thirdParty, string value, string paymentYear, string paymentMonth)
        {
            bool saved = false;
            decimal payment = Convert.ToDecimal(value);
            int year = Convert.ToInt32(paymentYear);
            int month = Convert.ToInt32(paymentMonth);
            int currencyId = Convert.ToInt32(currency);

            ThirdPartyPayment paid = new ThirdPartyPayment();
            paid.Currency = currencyId;
            paid.Month = month;
            paid.Payment = payment;
            paid.VideoId = id;
            paid.Year = year;
            paid.ThirdParty = thirdParty;

            LatestSightingsLibrary.Video vidRate = LatestSightingsLibrary.Video.GetVideo(id);
            if (vidRate != null && !String.IsNullOrEmpty(vidRate.RevenueShare))
            {
                paid.Payment = Financial.ApplyRevenueShare(paid.Payment, vidRate.RevenueShare);
                saved = ThirdPartyPayment.SaveThirdPartyPayment(paid);

                string cacheKey = "financialsDataThirdPayments_" + year.ToString() + "_" + month.ToString();
                if (HttpContext.Current.Cache[cacheKey] != null)
                {
                    HttpContext.Current.Cache.Remove(cacheKey);
                }
            }

            if (!saved)
                throw new Exception("Failure to save payment");
        }
    }
}