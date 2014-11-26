using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LatestSightings
{
    public partial class PaymentsDataCSV : System.Web.UI.Page
    {
        private int year = DateTime.Now.Year;
        private int month = DateTime.Now.Month;

        protected void Page_Load(object sender, EventArgs e)
        {
            SetParameters();
            string fileName = "Payments" + year.ToString() + "" + month.ToString() + ".csv";
            string attachment = "attachment; filename=" + fileName;
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader("content-disposition", attachment);
            HttpContext.Current.Response.ContentType = "text/csv";
            HttpContext.Current.Response.AddHeader("Pragma", "public");

            PaymentsDC dc = new PaymentsDC(year, month);
            List<PaymentDataItem> items = dc.GetDataItems();
            String line = string.Empty;
            if (items != null)
            {
                Export.ExportToCsv(fileName, items);
                using (StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath("~/files") + "/" + fileName))
                {
                    line = sr.ReadToEnd();
                    Console.WriteLine(line);
                }
            }
            HttpContext.Current.Response.Write(line);
            HttpContext.Current.Response.End();
        }

        private void SetParameters()
        {
            if (!String.IsNullOrEmpty(Request.QueryString["year"]))
                year = Convert.ToInt32(Request.QueryString["year"]);

            if (Page.RouteData.Values["year"] != null && !String.IsNullOrEmpty(Page.RouteData.Values["year"].ToString()))
                year = Convert.ToInt32(Page.RouteData.Values["year"].ToString());

            if (!String.IsNullOrEmpty(Request.QueryString["month"]))
                month = Convert.ToInt32(Request.QueryString["month"]);

            if (Page.RouteData.Values["month"] != null && !String.IsNullOrEmpty(Page.RouteData.Values["month"].ToString()))
                month = Convert.ToInt32(Page.RouteData.Values["month"].ToString());
        }
    }
}