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
    public partial class Payments : System.Web.UI.Page
    {
        public int year = DateTime.Now.Year;
        public int month = DateTime.Now.Month;
        public string currencyScripts = string.Empty;
        public string currencyScripts1 = string.Empty;
        public string filter = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            var master = Master as DefaultMaster;
            if (master != null)
            {
                master.SetHeader("Payments", "money");
                master.SetActiveNav("financials");
            }

            SetPageValues();

            if (!Page.IsPostBack)
            {
                txtMonthPicker.Text = new DateTime(year, month, 1).ToString("MMMM yyyy");
            }
        }

        protected void ChangeMonth(object sender, EventArgs e)
        {
            int month = ResolveMonthNumber(txtMonthPicker.Text.Split(' ')[0]);
            int year = Convert.ToInt32(txtMonthPicker.Text.Split(' ')[1]);
            HttpContext.Current.Response.Redirect("/payments/" + year.ToString() + "/" + month.ToString());
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

            if (!String.IsNullOrEmpty(Request.QueryString["filter"]))
                filter = Request.QueryString["filter"];

            if (Page.RouteData.Values["filter"] != null && !String.IsNullOrEmpty(Page.RouteData.Values["filter"].ToString()))
                filter = Page.RouteData.Values["filter"].ToString();

            SetCurrencies();

            lnkCSV.NavigateUrl = "/paymentscsv/" + year.ToString() + "/" + month.ToString();
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
                    currencyScripts1 += "{ \"data\": \"Currency" + count.ToString() + "\", orderable: false },";
                    sb.Append("<th style=\"background-color:#554337; color: #FFFFFF; border-bottom: 0px;\">" + currency.Description + "</th>" + Environment.NewLine);
                    count++;
                }
                ltlHeader.Text = sb.ToString();
                ltlCurrencies.Text = sb.ToString();
                ltlCurrencies1.Text = sb.ToString();
            }
        }

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public static string UpdatePayment(string contributor, string isPaid, string paymentYear, string paymentMonth)
        {
            Payment payment = new Payment();
            payment.Contributor = contributor;
            payment.Month = Convert.ToInt32(paymentMonth);
            payment.Year = Convert.ToInt32(paymentYear);
            payment.Paid = Convert.ToBoolean(isPaid);
            bool updated = Payment.UpdatePayment(payment);
            if (!updated)
                throw new Exception("Error updating payment");

            string cacheKey = "financialsDataPaidContributors_" + paymentYear.ToString() + "_" + paymentMonth.ToString();
            if (HttpContext.Current.Cache[cacheKey] != null)
            {
                HttpContext.Current.Cache.Remove(cacheKey);
            }

            return "success";
        }
    }
}