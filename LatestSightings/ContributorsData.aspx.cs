using LatestSightingsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LatestSightings
{
    public partial class ContributorsData : System.Web.UI.Page
    {
        private int draw = 1;
        private int index = 0;
        private int pageSize = 10;
        public int orderColumn = -1;
        public string orderColumnName = string.Empty;
        public string orderDirection = "asc";
        private string searchValue = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Set the page content type to json
            Response.ContentType = "application/json";

            // Set the data parameters passed in the query string
            SetParameters();
            
            // Get the list of people and convert to the contributor data item format
            List<Person> people = Person.GetPeople();
            ContributorData data = new ContributorData();
            List<ContributorDataItem> dataItems = new List<ContributorDataItem>();
            foreach (Person peep in people)
            {
                ContributorDataItem item = new ContributorDataItem();
                item.Id = peep.Id;
                item.Firstname = peep.FirstName;
                item.Lastname = peep.LastName;
                item.Email = peep.Email;
                dataItems.Add(item);
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
                dataItems = new List<ContributorDataItem>();

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
            Response.Write(people.ToList());
        }

        private void Order(ref List<ContributorDataItem> dataItems)
        {
            if (dataItems != null && dataItems.Count > 0 && orderColumn > -1 && !String.IsNullOrEmpty(orderColumnName))
            {
                var pi = typeof(ContributorDataItem).GetProperty(orderColumnName);
                dataItems = orderDirection == "asc" ? dataItems.OrderBy(x => pi.GetValue(x, null)).ToList() : dataItems.OrderByDescending(x => pi.GetValue(x, null)).ToList(); ;
            }
        }

        private void Filter(ref List<ContributorDataItem> dataItems)
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

        private void Search(ref List<ContributorDataItem> dataItems)
        {
            if (dataItems != null && dataItems.Count > 0 && !String.IsNullOrEmpty(searchValue))
            {
                dataItems = dataItems.FindAll(x => { return x.Firstname.ToLower().StartsWith(searchValue) || x.Lastname.ToLower().StartsWith(searchValue); });
            }
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

            string sad = HttpUtility.UrlDecode(Request.QueryString.ToString());

            if (orderColumn > -1 && !String.IsNullOrEmpty(Request.QueryString["columns[" + orderColumn + "][data]"]))
            {
                orderColumnName = Request.QueryString["columns[" + orderColumn + "][data]"].ToString();
            }
        }
    }

    [Serializable]
    public class ContributorData
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<ContributorDataItem> data { get; set; }
    }

    [Serializable]
    public class ContributorDataItem
    {
        public string Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
    }
}