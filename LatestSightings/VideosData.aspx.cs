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
    public partial class VideosData : System.Web.UI.Page
    {
        private int draw = 1;
        private int index = 0;
        private int pageSize = 10;
        private int orderColumn = -1;
        private string orderColumnName = string.Empty;
        private string orderDirection = "asc";
        private string searchValue = string.Empty;
        private string status = string.Empty;
        private string contributor = string.Empty;

        private List<YouTubeVideoAnalytics> _thisMonthsVideos;
        private List<YouTubeVideoAnalytics> thisMonthsVideos
        {
            get
            {
                if (_thisMonthsVideos == null)
                {
                    _thisMonthsVideos = GetAnalytics(DateTime.Now.Year, DateTime.Now.Month);
                }

                return _thisMonthsVideos;
            }
            set
            {
                _thisMonthsVideos = value;
            }
        }

        private List<YouTubeVideoAnalytics> _lastMonthsVideos;
        private List<YouTubeVideoAnalytics> lastMonthsVideos
        {
            get
            {
                if (_lastMonthsVideos == null)
                {
                    _lastMonthsVideos = GetAnalytics(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month);
                }

                return _lastMonthsVideos;
            }
            set
            {
                _lastMonthsVideos = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Set the page content type to json
            Response.ContentType = "application/json";

            // Set the data parameters passed in the query string
            SetParameters();

            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            status = textInfo.ToTitleCase(status);

            // Get the list of videos and convert to the video data item format
            List<LatestSightingsLibrary.Video> videos;
            if (!String.IsNullOrEmpty(contributor))
            {
                videos = LatestSightingsLibrary.Video.GetContributorVideos(contributor);
            }
            else
            {
                videos = LatestSightingsLibrary.Video.GetVideosCompact(status);
            }
            VideoData data = new VideoData();
            List<VideoDataItem> dataItems = new List<VideoDataItem>();
            if (videos != null)
            {
                foreach (LatestSightingsLibrary.Video vid in videos)
                {
                    VideoDataItem item = new VideoDataItem();
                    item.Id = vid.Id;
                    item.Alias = vid.Alias;
                    item.Contributor = vid.Person.FirstName + " " + vid.Person.LastName;
                    item.RevenueSplit = vid.RevenueShare;
                    item.Title = vid.Title;
                    item.YoutubeId = vid.YoutubeId;
                    item.PreviousMonth = GetLastMonthsEarnings(vid.YoutubeId);
                    item.CurrentMonth = GetThisMonthsEarnings(vid.YoutubeId);
                    
                    dataItems.Add(item);
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
                dataItems = new List<VideoDataItem>();

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

        private string GetLastMonthsEarnings(string videoId)
        {
            string returnValue = "0";

            if (lastMonthsVideos != null)
            {
                YouTubeVideoAnalytics vidAnalytics = lastMonthsVideos.FirstOrDefault(x => { return x.Id == videoId; });
                if (vidAnalytics != null)
                {
                    if (vidAnalytics.Earning > 0)
                    {
                        returnValue = "R" + vidAnalytics.Earning.ToString().TrimEnd('0');
                    }
                    else if (vidAnalytics.EstimatedEarning > 0)
                    {
                        returnValue = "$" + vidAnalytics.EstimatedEarning.ToString().TrimEnd('0');
                    }
                }
            }

            return returnValue;
        }

        private string GetThisMonthsEarnings(string videoId)
        {
            string returnValue = "0";

            if (thisMonthsVideos != null)
            {
                YouTubeVideoAnalytics vidAnalytics = thisMonthsVideos.FirstOrDefault(x => { return x.Id == videoId; });
                if (vidAnalytics != null)
                {
                    if (vidAnalytics.EstimatedEarning > 0)
                    {
                        returnValue = "$" + vidAnalytics.EstimatedEarning.ToString().TrimEnd('0');
                    }
                }
            }

            return returnValue;
        }

        private void Order(ref List<VideoDataItem> dataItems)
        {
            if (dataItems != null && dataItems.Count > 0 && orderColumn > -1 && !String.IsNullOrEmpty(orderColumnName))
            {
                var pi = typeof(VideoDataItem).GetProperty(orderColumnName);
                dataItems = orderDirection == "asc" ? dataItems.OrderBy(x => pi.GetValue(x, null)).ToList() : dataItems.OrderByDescending(x => pi.GetValue(x, null)).ToList(); ;
            }
        }

        private void Filter(ref List<VideoDataItem> dataItems)
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

        private void Search(ref List<VideoDataItem> dataItems)
        {
            if (dataItems != null && dataItems.Count > 0 && !String.IsNullOrEmpty(searchValue))
            {
                dataItems = dataItems.FindAll(x => { return x.Alias.ToLower().StartsWith(searchValue) || x.Contributor.ToLower().StartsWith(searchValue) || x.Title.ToLower().StartsWith(searchValue) || x.YoutubeId.ToLower().StartsWith(searchValue); });
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
            if (!String.IsNullOrEmpty(Request.QueryString["status"]))
                status = Request.QueryString["status"].ToString().ToLower();
            if (!String.IsNullOrEmpty(Request.QueryString["contributor"]))
                contributor = Request.QueryString["contributor"].ToString().ToLower();

            string sad = HttpUtility.UrlDecode(Request.QueryString.ToString());

            if (orderColumn > -1 && !String.IsNullOrEmpty(Request.QueryString["columns[" + orderColumn + "][data]"]))
            {
                orderColumnName = Request.QueryString["columns[" + (orderColumn) + "][data]"].ToString();
            }
        }

        private List<YouTubeVideoAnalytics> GetAnalytics(int year, int month)
        {
            string cacheKey = "videoData_" + year.ToString() + "_" + month.ToString();
            List<YouTubeVideoAnalytics> videos = null;

            if (HttpContext.Current.Cache[cacheKey] != null)
            {
                videos = (List<YouTubeVideoAnalytics>)HttpContext.Current.Cache[cacheKey];
            }

            if (videos == null)
            {
                videos = LatestSightingsLibrary.Video.GetYouTubeVideoAnalytics(year, month);
                if (videos != null)
                {
                    Cache.Add(cacheKey, videos, null, DateTime.Now.AddSeconds(180), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                }
            }

            return videos;
        }
    }

    [Serializable]
    public class VideoData
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<VideoDataItem> data { get; set; }
    }

    [Serializable]
    public class VideoDataItem
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Alias { get; set; }
        public string Contributor { get; set; }
        public string YoutubeId { get; set; }
        public string RevenueSplit { get; set; }
        public string PreviousMonth { get; set; }
        public string CurrentMonth { get; set; }
    }
}