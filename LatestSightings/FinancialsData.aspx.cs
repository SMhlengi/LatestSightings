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
    public partial class FinancialsData : System.Web.UI.Page
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
        private string contributor = string.Empty;

        private List<LatestSightingsLibrary.Video> _videos;
        private List<LatestSightingsLibrary.Video> videos
        {
            get
            {
                if (_videos == null)
                {
                    _videos = GetVideos();
                }
                return _videos;
            }
            set
            {
                _videos = value;
            }
        }

        private List<LatestSightingsLibrary.YouTubeVideoAnalytics> _analytics;
        private List<LatestSightingsLibrary.YouTubeVideoAnalytics> analytics
        {
            get
            {
                if (_analytics == null)
                {
                    _analytics = GetAnalytics(year, month);
                }
                return _analytics;
            }
            set
            {
                _analytics = value;
            }
        }

        private List<LatestSightingsLibrary.ThirdPartyPayment> _payments;
        private List<LatestSightingsLibrary.ThirdPartyPayment> payments
        {
            get
            {
                if (_payments == null)
                {
                    _payments = GetThirdPartyPayments(year, month);
                }
                return _payments;
            }
            set
            {
                _payments = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Set the page content type to json
            Response.ContentType = "application/json";

            // Set the data parameters passed in the query string
            SetParameters();

            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            filter = textInfo.ToTitleCase(filter);

            // Get the list of videos and convert to the financial data item format
            FinancialData data = new FinancialData();
            List<FinancialDataItem> dataItems = GetDataItems();

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
                dataItems = new List<FinancialDataItem>();

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

        private List<FinancialDataItem> GetDataItems()
        {
            List<FinancialDataItem> dataItems = new List<FinancialDataItem>();
            if (videos != null && videos.Count > 0 && !String.IsNullOrEmpty(contributor))
            {
                videos = videos.FindAll(x => { return x.Person.Id == contributor; });
            }
            if (videos != null && videos.Count > 0)
            {
                foreach (LatestSightingsLibrary.Video vid in videos)
                {
                    FinancialDataItem item = new FinancialDataItem();
                    item.Title = vid.Title;
                    item.YouTubeId = vid.YoutubeId;
                    item.RevenueShare = vid.RevenueShare;
                    item.Id = vid.Id;
                    item.Contributor = vid.Person.FirstName + " " + vid.Person.LastName;
                    item.YouTubeEarnings = GetYouTubeEarnings(vid.YoutubeId);
                    List<ThirdPartyPayment> thirdPayments = GetThirdPartyPayments(vid.Id);
                    UpdateThirdPartyPayments(ref item, thirdPayments);

                    dataItems.Add(item);
                }
            }

            return dataItems;
        }

        private void UpdateThirdPartyPayments(ref FinancialDataItem item, List<ThirdPartyPayment> thirdPayments)
        {
            item.Currency1 = 0;
            item.Currency2 = 0;
            item.Currency3 = 0;
            item.Currency4 = 0;
            item.Currency5 = 0;
            item.Currency6 = 0;
            item.Currency7 = 0;
            item.Currency8 = 0;
            item.Currency9 = 0;
            item.Currency10 = 0;

            if (thirdPayments != null)
            {
                foreach (ThirdPartyPayment paid in thirdPayments)
                {
                    item.GetType().GetProperty("Currency" + paid.Currency).SetValue(item, Math.Round(paid.Payment, 2, MidpointRounding.AwayFromZero), null);
                }
            }
        }

        private decimal GetYouTubeEarnings(string id)
        {
            decimal earnings = 0;

            if (analytics != null)
            {
                YouTubeVideoAnalytics vid = analytics.FirstOrDefault(x => { return x.Id == id; });
                if (vid != null && vid.Earning > 0)
                {
                    earnings = vid.Earning;
                }
            }

            return earnings;
        }

        private List<ThirdPartyPayment> GetThirdPartyPayments(string id)
        {
            List<ThirdPartyPayment> thirdPayments = null;

            if (payments != null)
            {
                thirdPayments = payments.FindAll(x => { return x.VideoId == id; });
            }

            return thirdPayments;
        }

        private void Order(ref List<FinancialDataItem> dataItems)
        {
            if (dataItems != null && dataItems.Count > 0 && orderColumn > -1 && !String.IsNullOrEmpty(orderColumnName))
            {
                var pi = typeof(FinancialDataItem).GetProperty(orderColumnName);
                dataItems = orderDirection == "asc" ? dataItems.OrderBy(x => pi.GetValue(x, null)).ToList() : dataItems.OrderByDescending(x => pi.GetValue(x, null)).ToList(); ;
            }
        }

        private void Filter(ref List<FinancialDataItem> dataItems)
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

        private void Search(ref List<FinancialDataItem> dataItems)
        {
            if (dataItems != null && dataItems.Count > 0 && !String.IsNullOrEmpty(searchValue))
            {
                dataItems = dataItems.FindAll(x => { return x.Title.ToLower().StartsWith(searchValue) || x.Contributor.ToLower().StartsWith(searchValue); });
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
            if (!String.IsNullOrEmpty(Request.QueryString["filter"]))
                filter = Request.QueryString["filter"].ToString().ToLower();
            if (!String.IsNullOrEmpty(Request.QueryString["year"]))
                year = Convert.ToInt32(Request.QueryString["year"]);
            if (!String.IsNullOrEmpty(Request.QueryString["month"]))
                month = Convert.ToInt32(Request.QueryString["month"]);
            if (!String.IsNullOrEmpty(Request.QueryString["owner"]))
                contributor = Request.QueryString["owner"].ToString();

            if (pageSize < 1)
                pageSize = 1000;

            string sad = HttpUtility.UrlDecode(Request.QueryString.ToString());

            if (orderColumn > -1 && !String.IsNullOrEmpty(Request.QueryString["columns[" + orderColumn + "][data]"]))
            {
                orderColumnName = Request.QueryString["columns[" + (orderColumn) + "][data]"].ToString();
            }
        }

        private List<LatestSightingsLibrary.Video> GetVideos()
        {
            string cacheKey = "financialsDataVideos";
            List<LatestSightingsLibrary.Video> videos1 = null;

            if (HttpContext.Current.Cache[cacheKey] != null)
            {
                videos1 = (List<LatestSightingsLibrary.Video>)HttpContext.Current.Cache[cacheKey];
            }

            if (videos1 == null)
            {
                videos1 = LatestSightingsLibrary.Video.GetVideosCompact();
                if (videos1 != null)
                {
                    Cache.Add(cacheKey, videos1, null, DateTime.Now.AddSeconds(180), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                }
            }

            return videos1;
        }

        private List<YouTubeVideoAnalytics> GetAnalytics(int year, int month)
        {
            string cacheKey = "financialsDataAnalytics_" + year.ToString() + "_" + month.ToString();
            List<YouTubeVideoAnalytics> videos1 = null;

            if (HttpContext.Current.Cache[cacheKey] != null)
            {
                videos1 = (List<YouTubeVideoAnalytics>)HttpContext.Current.Cache[cacheKey];
            }

            if (videos1 == null)
            {
                videos1 = LatestSightingsLibrary.Video.GetYouTubeVideoAnalytics(year, month);
                if (videos1 != null)
                {
                    Cache.Add(cacheKey, videos1, null, DateTime.Now.AddSeconds(180), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                }
            }

            return videos1;
        }

        private List<ThirdPartyPayment> GetThirdPartyPayments(int year, int month)
        {
            string cacheKey = "financialsDataThirdPayments_" + year.ToString() + "_" + month.ToString();
            List<ThirdPartyPayment> payments1 = null;

            if (HttpContext.Current.Cache[cacheKey] != null)
            {
                payments1 = (List<ThirdPartyPayment>)HttpContext.Current.Cache[cacheKey];
            }

            if (payments1 == null)
            {
                payments1 = LatestSightingsLibrary.ThirdPartyPayment.GetThirdPartyPayments(year, month);
                if (payments1 != null)
                {
                    Cache.Add(cacheKey, payments1, null, DateTime.Now.AddSeconds(180), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                }
            }

            return payments1;
        }
    }

    [Serializable]
    public class FinancialData
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<FinancialDataItem> data { get; set; }
    }

    [Serializable]
    public class FinancialDataItem
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Contributor { get; set; }
        public decimal YouTubeEarnings { get; set; }
        public string YouTubeId { get; set; }
        public string RevenueShare { get; set; }
        public decimal Currency1 { get; set; }
        public decimal Currency2 { get; set; }
        public decimal Currency3 { get; set; }
        public decimal Currency4 { get; set; }
        public decimal Currency5 { get; set; }
        public decimal Currency6 { get; set; }
        public decimal Currency7 { get; set; }
        public decimal Currency8 { get; set; }
        public decimal Currency9 { get; set; }
        public decimal Currency10 { get; set; }
    }
}