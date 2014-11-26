using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatestSightingsLibrary
{
    public class YouTubeVideoAnalytics
    {
        public string Id { get; set; }
        public decimal Earning { get; set; }
        public decimal EstimatedEarning { get; set; }
        public long Views { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public DateTime LastRun { get; set; }
    }
}