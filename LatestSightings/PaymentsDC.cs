using LatestSightingsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace LatestSightings
{
    public class PaymentsDC
    {
        public int year { get; set; }
        public int month { get; set; }

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

        private List<LatestSightingsLibrary.Video> _contributors;
        private List<LatestSightingsLibrary.Video> contributors
        {
            get
            {
                if (_contributors == null)
                {
                    _contributors = GetContributors();
                }
                return _contributors;
            }
            set
            {
                _contributors = value;
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

        private List<LatestSightingsLibrary.Payment> _paidContributors;
        private List<LatestSightingsLibrary.Payment> paidContributors
        {
            get
            {
                if (_paidContributors == null)
                {
                    _paidContributors = GetPaidContributors(year, month);
                }
                return _paidContributors;
            }
            set
            {
                _paidContributors = value;
            }
        }

        public PaymentsDC(int paymentYear, int paymentMonth)
        {
            year = paymentYear;
            month = paymentMonth;
        }

        public List<PaymentDataItem> GetDataItems()
        {
            List<PaymentDataItem> items = new List<PaymentDataItem>();

            if (contributors != null)
            {
                foreach (LatestSightingsLibrary.Video vid in contributors)
                {
                    PaymentDataItem item = new PaymentDataItem();
                    item.ContributorId = vid.Person.Id;
                    item.ContributorName = vid.Person.FirstName + " " + vid.Person.LastName;
                    item.Checked = "";
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

                    if (paidContributors != null && paidContributors.Count > 0)
                    {
                        Payment isPaid = paidContributors.FirstOrDefault(x => { return x.Contributor == vid.Person.Id; });
                        if (isPaid != null && isPaid.Paid)
                            item.Checked = "checked";

                    }

                    List<LatestSightingsLibrary.Video> contributorVids = videos.FindAll(x => { return x.Person.Id == vid.Person.Id; });
                    List<ThirdPartyPayment> contributorPayments = new List<ThirdPartyPayment>();
                    decimal rands = 0;

                    if (contributorVids != null && contributorVids.Count > 0)
                    {
                        foreach (LatestSightingsLibrary.Video cntrVid in contributorVids)
                        {
                            YouTubeVideoAnalytics cnrtAnalytics = null;
                            if (analytics != null && analytics.Count > 0)
                            {
                                cnrtAnalytics = analytics.FirstOrDefault(x => { return x.Id == cntrVid.YoutubeId; });
                            }
                            if (cnrtAnalytics != null)
                                rands += cnrtAnalytics.Earning;

                            if (payments != null)
                            {
                                List<ThirdPartyPayment> contributorPaymentstmp = payments.FindAll(x => { return x.VideoId == cntrVid.Id; });
                                if (contributorPaymentstmp != null && contributorPaymentstmp.Count > 0)
                                    contributorPayments.AddRange(contributorPaymentstmp);
                            }
                        }
                    }

                    List<Currency> currencies = Currency.GetCurrencies();
                    if (currencies != null)
                    {
                        foreach (Currency currency in currencies)
                        {
                            decimal total = 0;
                            if (currency.Symbol == "R")
                                total += rands;

                            List<ThirdPartyPayment> cntrCurPayments = contributorPayments.FindAll(x => { return x.Currency == currency.Id; });
                            if (cntrCurPayments != null && cntrCurPayments.Count > 0)
                                total += cntrCurPayments.Sum(x => x.Payment);

                            item.GetType().GetProperty("Currency" + currency.Id).SetValue(item, Math.Round(total, 2, MidpointRounding.AwayFromZero), null);
                        }
                    }

                    items.Add(item);
                }
            }

            return items;
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
                    HttpContext.Current.Cache.Add(cacheKey, videos1, null, DateTime.Now.AddSeconds(180), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
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
                    HttpContext.Current.Cache.Add(cacheKey, videos1, null, DateTime.Now.AddSeconds(180), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
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
                    HttpContext.Current.Cache.Add(cacheKey, payments1, null, DateTime.Now.AddSeconds(180), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                }
            }

            return payments1;
        }

        private List<Payment> GetPaidContributors(int year, int month)
        {
            string cacheKey = "financialsDataPaidContributors_" + year.ToString() + "_" + month.ToString();
            List<Payment> payments1 = null;

            if (HttpContext.Current.Cache[cacheKey] != null)
            {
                payments1 = (List<Payment>)HttpContext.Current.Cache[cacheKey];
            }

            if (payments1 == null)
            {
                payments1 = LatestSightingsLibrary.Payment.GetPayments(year, month);
                if (payments1 != null)
                {
                    HttpContext.Current.Cache.Add(cacheKey, payments1, null, DateTime.Now.AddSeconds(180), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                }
            }

            return payments1;
        }

        private List<LatestSightingsLibrary.Video> GetContributors()
        {
            string cacheKey = "financialsDataContributors_" + year.ToString() + "_" + month.ToString();
            List<LatestSightingsLibrary.Video> contributrors1 = null;

            if (HttpContext.Current.Cache[cacheKey] != null)
            {
                contributrors1 = (List<LatestSightingsLibrary.Video>)HttpContext.Current.Cache[cacheKey];
            }

            if (contributrors1 == null)
            {
                if (videos != null)
                {
                    contributrors1 = videos
                      .GroupBy(p => new { p.Person.Id })
                      .Select(g => g.First())
                      .ToList();

                    contributrors1 = contributrors1.OrderBy(x => { return x.Person.FirstName; }).ToList();
                    if (contributrors1 != null)
                    {
                        HttpContext.Current.Cache.Add(cacheKey, contributrors1, null, DateTime.Now.AddSeconds(180), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                    }
                }
            }

            return contributrors1;
        }
    }

    [Serializable]
    public class PaymentData
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<PaymentDataItem> data { get; set; }
    }

    [Serializable]
    public class PaymentDataItem
    {
        public string ContributorId { get; set; }
        public string ContributorName { get; set; }
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
        public string Checked { get; set; }
    }
}