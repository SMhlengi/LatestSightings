using LatestSightingsLibrary;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LatestSightings
{
    public partial class PaymentsData : System.Web.UI.Page
    {
        private int draw = 1;
        private int index = 0;
        private int pageSize = 10;
        private int orderColumn = -1;
        private string orderColumnName = string.Empty;
        private string orderDirection = "asc";
        private string searchValue = string.Empty;
        private string filter = string.Empty;
        private int year = DateTime.Now.Year;
        private int month = DateTime.Now.Month;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Set the page content type to json
            Response.ContentType = "application/json";

            // Set the data parameters passed in the query string
            SetParameters();

            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            filter = textInfo.ToTitleCase(filter);

            // Get the list of videos and convert to the financial data item format
            PaymentData data = new PaymentData();
            PaymentsDC dc = new PaymentsDC(year, month);
            List<PaymentDataItem> dataItems = dc.GetDataItems();

            if (dataItems != null)
            {
                if (!String.IsNullOrEmpty(filter) && dataItems != null && dataItems.Count > 0)
                {
                    if (filter == "Paid")
                    {
                        dataItems = dataItems.FindAll(x => { return x.Checked == "checked"; });
                    }
                    else if (filter == "Notpaid")
                    {
                        dataItems = dataItems.FindAll(x => { return x.Checked != "checked"; });
                    }
                }
            }

            int totalRecords = dataItems.Count;
            int totalFilteredRecords = dataItems.Count;

            // Return only values matching the search criteria if there is one
            Search(ref dataItems);

            // Order the list based on the column ordering
            Order(ref dataItems);
            totalFilteredRecords = dataItems.Count;

            // Return the right number of items and correct starting item based on recieved parameters
            Filter(ref dataItems);

            if (dataItems == null)
                dataItems = new List<PaymentDataItem>();

            // Finish and serialize the json
            data.draw = draw;
            data.recordsTotal = totalRecords;
            data.recordsFiltered = totalFilteredRecords;
            data.data = dataItems;
            JavaScriptSerializer jss = new JavaScriptSerializer();

            // Output and end
            string output = jss.Serialize(data);
            Response.Write(output);
            Response.Flush();
            Response.End();
        }

        private void SetParameters()
        {
            string ad = HttpUtility.UrlDecode(Request.QueryString.ToString());
            if (!String.IsNullOrEmpty(Request.QueryString["draw"]))
                draw = Convert.ToInt32(Request.QueryString["draw"]);
            if (!String.IsNullOrEmpty(Request.QueryString["start"]))
                index = Convert.ToInt32(Request.QueryString["start"]);
            if (!String.IsNullOrEmpty(Request.QueryString["length"]))
                pageSize = Convert.ToInt32(Request.QueryString["length"]);
            if (!String.IsNullOrEmpty(Request.QueryString["search[value]"]))
                searchValue = Request.QueryString["search[value]"].ToString().ToLower();
            if (!String.IsNullOrEmpty(Request.QueryString["order[0][column]"]))
                orderColumn = Convert.ToInt32(Request.QueryString["order[0][column]"]);
            if (!String.IsNullOrEmpty(Request.QueryString["order[0][dir]"]))
                orderDirection = Request.QueryString["order[0][dir]"].ToString().ToLower();
            if (!String.IsNullOrEmpty(Request.QueryString["filter"]))
                filter = Request.QueryString["filter"].ToString().ToLower();
            if (!String.IsNullOrEmpty(Request.QueryString["year"]))
                year = Convert.ToInt32(Request.QueryString["year"]);
            if (!String.IsNullOrEmpty(Request.QueryString["month"]))
                month = Convert.ToInt32(Request.QueryString["month"]);

            string sad = HttpUtility.UrlDecode(Request.QueryString.ToString());

            if (orderColumn > -1 && !String.IsNullOrEmpty(Request.QueryString["columns[" + orderColumn + "][data]"]))
            {
                orderColumnName = Request.QueryString["columns[" + (orderColumn) + "][data]"].ToString();
            }
        }

        private void Order(ref List<PaymentDataItem> dataItems)
        {
            if (dataItems != null && dataItems.Count > 0 && orderColumn > -1 && !String.IsNullOrEmpty(orderColumnName))
            {
                var pi = typeof(PaymentDataItem).GetProperty(orderColumnName);
                dataItems = orderDirection == "asc" ? dataItems.OrderBy(x => pi.GetValue(x, null)).ToList() : dataItems.OrderByDescending(x => pi.GetValue(x, null)).ToList(); ;
            }
        }

        private void Filter(ref List<PaymentDataItem> dataItems)
        {
            if (dataItems != null && dataItems.Count > 0)
            {
                if ((dataItems.Count) >= (index + pageSize))
                {
                    dataItems = dataItems.GetRange(index, pageSize);
                }
                else
                {
                    dataItems = index == 0 ? dataItems.GetRange(index, dataItems.Count) : dataItems.GetRange(index, dataItems.Count % pageSize);
                }
            }
        }

        private void Search(ref List<PaymentDataItem> dataItems)
        {
            if (dataItems != null && dataItems.Count > 0 && !String.IsNullOrEmpty(searchValue))
            {
                dataItems = dataItems.FindAll(x => { return x.ContributorName.ToLower().StartsWith(searchValue); });
            }
        }

    }
}