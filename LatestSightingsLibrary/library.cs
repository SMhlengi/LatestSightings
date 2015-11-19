using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;

namespace LatestSightingsLibrary
{
    public class library
    {
        private const string SQL_GET_USERNAME = "Select firstname, lastname from latestsightings.dbo.people WHERE (id = @id);";
        private const string SQL_GET_CURRENT_DAY_LODGE_TINGS = "SELECT TOP (5) userId, COUNT(userId) AS tingsTotal FROM tings WHERE lodgeId = @lodgeid and (CONVERT(date, time) = CONVERT(date, DATEADD(day, @dayHistory, GETDATE()))) GROUP BY userId order by tingsTotal Desc";
        private const string SQL_GET_LODGE_TINGS = "SELECT * FROM tings WHERE (lodgeId = @lodgeid AND animal IS NOT NULL) ORDER BY time DESC";
        private const string SQL_GET_LODGE_TINGS_BY_DATE = "SELECT * FROM tings WHERE (lodgeId = @lodgeid AND animal IS NOT NULL AND (CAST(time AS date) = @currentDate)) ORDER BY time DESC";
        private const string SQL_GET_TOP_LODGE_TINGS = "SELECT TOP (@tingnumber) id FROM tings ORDER BY time DESC";
        private const string SQL_GET_TOP_TINGS = "SELECT top (@tingnumber) a.id as id,a.title as title, a.time as time, b.name FROM tings a INNER JOIN parks b ON a.parkid = b.id WHERE a.animal IS NOT NULL ORDER BY time DESC";
        private const string SQL_GET_ARTICLES_BASED_ON_SEARCH_STRING = "SELECT * FROM Article WHERE Header LIKE '%#searchString#%' and Complete = 1;";
        private const string SQL_GET_VIDEO = "SELECT youtubeId FROM VIDEOS WHERE TITLE LIKE '%#searchString#%' and status = 'Published'";
        private const string SQL_GET_IMAGE = "SELECT * FROM IMAGES WHERE tags LIKE'%#searchTag#%' and display = 1;";
        private const string SQL_GET_TING_INFO = "SELECT * FROM tings where id = @tingid";
        private const string SQL_GET_PARKS = "SELECT id, name FROM parks WHERE (active = 1)";
        private const string SQL_GET_KRUGER_TINGS = "SELECT top 25 * FROM tings WHERE (parkId = @pid) AND animal IS NOT NULL ORDER BY time DESC";

        public static bool isArticleTableEmpty(SqlConnection conn, SqlCommand query)
        {
            int count = 0;
            query.CommandText = "SELECT Count(*) FROM [dbo].[Article]";
            conn.Open();
            count = Convert.ToInt32(query.ExecuteScalar());
            conn.Close();
            if (count > 0)
                return false;
            return true;
        }

        public static bool isCategoryTableEmpty(SqlConnection conn, SqlCommand query)
        {
            int count = 0;
            query.CommandText = "SELECT Count(*) FROM [dbo].[Category]";
            conn.Open();
            count = Convert.ToInt32(query.ExecuteScalar());
            conn.Close();
            if (count > 0)
                return false;
            return true;
        }

        public static bool HavingCategories(SqlConnection conn, SqlCommand query)
        {
            int count = 0;
            query.CommandText = "Select Count(*) From [dbo].[Category]";
            conn.Open();
            count = Convert.ToInt32(query.ExecuteScalar());
            conn.Close();
            if (count > 0)
                return true;
            return false;
        }

        public static int GetFirstCategoryId(SqlConnection conn, SqlCommand query, SqlDataReader data)
        {
            query.CommandText = "SELECT TOP (1) id FROM Category";
            conn.Open();
            data = query.ExecuteReader();
            int categoryId = -1;
            if (data.HasRows)
            {
                while (data.Read())
                {
                    categoryId = Convert.ToInt32(data["id"].ToString());
                }
            }
            data.Close();
            conn.Close();
            return categoryId;
        }

        public static List<Dictionary<string, string>> GetArticlesBasedOnCategoryId(int id, SqlConnection conn, SqlCommand query, SqlDataReader data)
        {
            List<Dictionary<string,string>> articles = new List<Dictionary<string,string>>();
            string format = "MMM d HH:mm yyyy"; // <!-- output example Feb 27 11:41 2009

            query.CommandText = "Select * from [dbo].[Article] where [Article].CategoryID = " + id + " ORDER BY DateCreated DESC";
            conn.Open();
            data = query.ExecuteReader();
            if (data.HasRows)
            {
                while(data.Read())
                {
                    Dictionary<string,string> article = new Dictionary<string,string>()
                    {
                        {"header", data["Header"].ToString()},
                        {"draft", data["draft"].ToString()},
                        {"complete", data["complete"].ToString()},
                        {"dateCreated", Convert.ToDateTime(data["DateCreated"]).ToString(format)},
                        {"body", data["body"].ToString()},
                        {"id", data["id"].ToString()}
                    };
                    articles.Add(article);
                }
            }
            conn.Close();
            data.Close();
            return articles;
        }

        public static List<Dictionary<string, string>> GetArticlesBasedOnCategoryId(int id, Boolean allAritcles = false)
        {
            SqlConnection conn = library.Conn();
            conn.Open();
            SqlCommand query = new SqlCommand();
            query.Connection = conn;
            List<Dictionary<string, string>> articles = new List<Dictionary<string, string>>();
            string format = "MMM d HH:mm yyyy"; // <!-- output example Feb 27 11:41 2009

            if (allAritcles == true)
                query.CommandText = "Select * from [dbo].[Article] where [Article].CategoryID = " + id + " AND Complete = 1 ORDER BY DateCreated DESC";
            else
                query.CommandText = "Select top 3 * from [dbo].[Article] where [Article].CategoryID = " + id + " AND Complete = 1 ORDER BY DateCreated DESC";

            SqlDataReader data = query.ExecuteReader();
            if (data.HasRows)
            {
                while (data.Read())
                {
                    Dictionary<string, string> article = new Dictionary<string, string>()
                    {
                        {"header", data["Header"].ToString()},
                        {"draft", data["draft"].ToString()},
                        {"complete", data["complete"].ToString()},
                        {"dateCreated", Convert.ToDateTime(data["DateCreated"]).ToString(format)},
                        {"body", data["body"].ToString()},
                        {"id", data["id"].ToString()},
                        {"picture",  data["Picture"].ToString()},
                        {"categoryId",  data["CategoryID"].ToString()},
                        {"url",  data["url"].ToString()}
                    };
                    articles.Add(article);
                }
            }
            conn.Close();
            data.Close();
            return articles;
        }

        public static List<Dictionary<string, string>> GetLatesArticles(int quantity, SqlConnection conn, SqlCommand query, SqlDataReader data)
        {
            List<Dictionary<string, string>> articles = new List<Dictionary<string, string>>();
            string format = "MMM d HH:mm yyyy"; // <!-- output example Feb 27 11:41 2009

            query.CommandText = "Select TOP " + quantity.ToString() + " * from [dbo].[Article] ORDER BY DateCreated DESC";
            conn.Open();
            data = query.ExecuteReader();
            if (data.HasRows)
            {
                while (data.Read())
                {
                    Dictionary<string, string> article = new Dictionary<string, string>()
                    {
                        {"header", data["Header"].ToString()},
                        {"draft", data["draft"].ToString()},
                        {"complete", data["complete"].ToString()},
                        {"dateCreated", Convert.ToDateTime(data["DateCreated"]).ToString(format)},
                        {"body", data["body"].ToString()},
                        {"id", data["id"].ToString()}
                    };
                    articles.Add(article);
                }
            }
            conn.Close();
            data.Close();
            return articles;
        }

        public static List<Dictionary<string, string>> GetLatestCompletedArticles(int quantity)
        {
            SqlConnection conn = library.Conn();
            conn.Open();
            SqlCommand query = new SqlCommand();
            query.Connection = conn;
            query.CommandText = "Select TOP " + quantity.ToString() + " * from [dbo].[Article] WHERE Complete = 1 ORDER BY DateCreated DESC";

            List<Dictionary<string, string>> articles = new List<Dictionary<string, string>>();
            string format = "MMM d HH:mm yyyy"; // <!-- output example Feb 27 11:41 2009
            
            SqlDataReader data = query.ExecuteReader();
            if (data.HasRows)
            {
                while (data.Read())
                {
                    if ((!data["Header"].ToString().ToLower().Contains("terms")) && (!data["Header"].ToString().ToLower().Contains("policy")))
                    {
                        Dictionary<string, string> article = new Dictionary<string, string>()
                        {
                            {"header", data["Header"].ToString()},
                            {"picture", data["Picture"].ToString()},
                            {"id", data["id"].ToString()},
                            {"categoryid", data["categoryid"].ToString()},
                        };
                        articles.Add(article);
                    }
                }
            }
            conn.Close();
            data.Close();
            return articles;
        }

        public static List<Dictionary<string, string>> GetAllCategories(SqlConnection conn, SqlCommand query, SqlDataReader data)
        {
            List<Dictionary<string, string>> categories = new List<Dictionary<string, string>>();

            query.CommandText = "Select * from [dbo].[Category]";
            conn.Open();
            data = query.ExecuteReader();
            if (data.HasRows)
            {
                while (data.Read())
                {
                    Dictionary<string, string> category = new Dictionary<string, string>()
                    {
                        {"id", data["id"].ToString()},
                        {"categoryname", data["Name"].ToString()},
                    };
                    categories.Add(category);
                }
            }
            conn.Close();
            data.Close();
            return categories;
        }

        public static Dictionary<string, string> GetArticle(int id, SqlConnection conn, SqlCommand query, SqlDataReader data)
        {
            query.CommandText = "Select * from [dbo].[Article] where id = " + id;
            conn.Open();
            data = query.ExecuteReader();
            Dictionary<string,string> article = new Dictionary<string,string>();
            if (data.HasRows)
            {
                article.Add("articleFound", "1");
                while (data.Read())
                {
                    article.Add("categoryId", data["CategoryID"].ToString());
                    article.Add("header", data["header"].ToString());
                    article.Add("body", data["Body"].ToString());
                    article.Add("picture", data["Picture"].ToString());
                    article.Add("draft", data["Draft"].ToString());
                    article.Add("complete", data["Complete"].ToString());
                }
            }
            else
            {
                article.Add("articleFound", "0");
            }
            data.Close();
            conn.Close();
            return article;
        }

        public static Dictionary<string, string> GetArticle(int id)
        {
            SqlConnection conn = library.Conn();
            conn.Open();
            SqlCommand query = new SqlCommand();
            query.Connection = conn;

            query.CommandText = "Select * from [dbo].[Article] where id = " + id;
            SqlDataReader data = query.ExecuteReader();
            Dictionary<string, string> article = new Dictionary<string, string>();
            string format = "MMM d yyyy"; // <!-- output example Feb 27 11:41 2009

            if (data.HasRows)
            {
                article.Add("articleFound", "1");
                while (data.Read())
                {
                    article.Add("categoryId", data["CategoryID"].ToString());
                    article.Add("header", data["header"].ToString());
                    article.Add("body", data["Body"].ToString());
                    article.Add("picture", data["Picture"].ToString());
                    article.Add("draft", data["Draft"].ToString());
                    article.Add("complete", data["Complete"].ToString());
                    article.Add("dateCreated", Convert.ToDateTime(data["DateCreated"]).ToString(format));
                }
            }
            else
            {
                article.Add("articleFound", "0");
            }
            data.Close();
            conn.Close();
            return article;
        }
        
        public static SqlConnection Conn()
        {
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString);

            return conn;
        }


        public static Dictionary<string, string> GetArticle(string url)
        {
            SqlConnection conn = library.Conn();
            conn.Open();
            SqlCommand query = new SqlCommand();
            query.Connection = conn;

            query.CommandText = "Select TOP 1 * from [dbo].[Article] where url = " + url;
            SqlDataReader data = query.ExecuteReader();
            Dictionary<string, string> article = new Dictionary<string, string>();
            string format = "MMM d yyyy"; // <!-- output example Feb 27 11:41 2009

            if (data.HasRows)
            {
                article.Add("articleFound", "1");
                while (data.Read())
                {
                    article.Add("categoryId", data["CategoryID"].ToString());
                    article.Add("header", data["header"].ToString());
                    article.Add("body", data["Body"].ToString());
                    article.Add("picture", data["Picture"].ToString());
                    article.Add("draft", data["Draft"].ToString());
                    article.Add("complete", data["Complete"].ToString());
                    article.Add("dateCreated", Convert.ToDateTime(data["DateCreated"]).ToString(format));
                }
            }
            else
            {
                article.Add("articleFound", "0");
            }
            data.Close();
            conn.Close();
            return article;
        }

        // LODGES 

        public static Dictionary<string, string> GetLodge(string name)
        {
            SqlConnection conn = library.Conn();
            conn.Open();
            SqlCommand query = new SqlCommand();
            query.Connection = conn;

            query.CommandText = "Select TOP 1 * from [dbo].[lodges] where name = '" + name + "' and active = 1";
            SqlDataReader data = query.ExecuteReader();
            Dictionary<string, string> lodge = new Dictionary<string, string>();
            string format = "MMM d yyyy"; // <!-- output example Feb 27 11:41 2009

            if (data.HasRows)
            {
                lodge.Add("lodgeFound", "1");
                while (data.Read())
                {
                    lodge.Add("id", data["id"].ToString());
                    lodge.Add("name", data["name"].ToString());
                    lodge.Add("desc", data["description"].ToString());
                    lodge.Add("location", data["location"].ToString());
                    lodge.Add("coordinates", data["coordinates"].ToString());
                    lodge.Add("prizes", data["prizes"].ToString());
                    lodge.Add("logo", data["logo"].ToString());
                }
            }
            else
            {
                lodge.Add("lodgeFound", "0");
            }
            data.Close();
            conn.Close();
            return lodge;
        }

        public static List<Dictionary<string, string>> GetTopFiveLodgeTingers(string lodgeId)
        {            
            int dayCounter = 0;
            SqlConnection conn = library.Conn();
            SqlCommand query = new SqlCommand();
            SqlDataReader data = null;
            conn.Open();

            while(true)
            {
                query = new SqlCommand();
                query.Connection = conn;
                query.CommandText = SQL_GET_CURRENT_DAY_LODGE_TINGS;
                query.Parameters.Add("lodgeid", System.Data.SqlDbType.VarChar).Value = lodgeId;
                query.Parameters.Add("dayHistory", System.Data.SqlDbType.Int).Value = dayCounter;
                data = query.ExecuteReader();
                if (data.HasRows)
                    break;
                dayCounter -= 1;
                data.Close();
            }
            List<Dictionary<string, string>> tings = new List<Dictionary<string, string>>();
            Dictionary<string, string> tingers;

            if (data.HasRows)
            {
                while (data.Read())
                {
                    tingers = new Dictionary<string, string>();
                    tingers.Add("userid", data["userId"].ToString());
                    tingers.Add("tingsTotal", data["tingsTotal"].ToString());
                    tings.Add(tingers);
                }
            }

            if (tings.Count > 0)
            {
                foreach (var ting in tings)
                {
                    ting.Add("username", GetTingerUserName(ting["userid"]));
                }
            }
            conn.Close();
            return tings;                
        }

        private static string GetTingerUserName(string userid)
        {
            SqlConnection conn = library.Conn();
            SqlCommand query = new SqlCommand();
            String username = "";
            query.Connection = conn;
            // SQL_GET_USERNAME = "Select firstname, lastname from latestsightings.dbo.people WHERE (id = @id);";
            query.CommandText = SQL_GET_USERNAME;
            query.Parameters.Add("id", System.Data.SqlDbType.VarChar).Value = userid;
            conn.Open();

            SqlDataReader data = query.ExecuteReader();
            if (data.HasRows)
            {
                while (data.Read())
                {
                    username = data["firstname"].ToString() + " " + data["lastname"].ToString();
                }
            }
            conn.Close();
            data.Close();
            return username;
        }

        public static List<Dictionary<string, string>> GetLodgeTings(string lodgeId)
        {
            SqlConnection conn = library.Conn();
            SqlCommand query = new SqlCommand();
            query.Connection = conn;
            // private const string SQL_GET_LODGE_TINGS = "SELECT * FROM tings WHERE (lodgeId = @lodgeid) ORDER BY time DESC";
            query.CommandText = SQL_GET_LODGE_TINGS;
            query.Parameters.Add("lodgeid", System.Data.SqlDbType.VarChar).Value = lodgeId;
            conn.Open();
            SqlDataReader data = query.ExecuteReader();

            List<Dictionary<string, string>> tings = new List<Dictionary<string, string>>();
            Dictionary<string, string> tingers;

            if (data.HasRows)
            {
                while (data.Read())
                {
                    tingers = new Dictionary<string, string>();
                    tingers.Add("id", data["id"].ToString());
                    tingers.Add("userid", data["userId"].ToString());
                    tingers.Add("time", ConvertToDateTimeFormat(data["time"].ToString()));
                    tingers.Add("title", data["title"].ToString());
                    tingers.Add("visibility", data["visibility"].ToString());
                    tingers.Add("traffic", data["traffic"].ToString());
                    tingers.Add("location", data["situation"].ToString());
                    tingers.Add("description", data["description"].ToString());
                    tingers.Add("longitude", data["longitude"].ToString());
                    tingers.Add("latitude", data["latitude"].ToString());
                    tingers.Add("animalid", data["animal"].ToString());
                    tings.Add(tingers);
                }
            }

            conn.Close();
            data.Close();
            if (tings.Count > 0)
            {
                foreach (var ting in tings)
                {
                    ting.Add("username", GetTingerUserName(ting["userid"]));
                }
            }
            return tings;
        }

        public static List<Dictionary<string, string>> GetLodgeTingsByDate(string lodgeId, string currentDate)
        {
            SqlConnection conn = library.Conn();
            SqlCommand query = new SqlCommand();
            query.Connection = conn;
            // private const string SQL_GET_LODGE_TINGS_BY_DATE = "SELECT * FROM tings WHERE (lodgeId = @lodgeid AND animal IS NOT NULL AND (CAST(time AS date) = '@currentDate')) ORDER BY time DESC";
            query.CommandText = SQL_GET_LODGE_TINGS_BY_DATE;
            query.Parameters.Add("lodgeid", System.Data.SqlDbType.VarChar).Value = lodgeId;
            query.Parameters.Add("currentDate", System.Data.SqlDbType.VarChar).Value = currentDate;
            conn.Open();
            SqlDataReader data = query.ExecuteReader();

            List<Dictionary<string, string>> tings = new List<Dictionary<string, string>>();
            Dictionary<string, string> tingers;

            if (data.HasRows)
            {
                while (data.Read())
                {
                    tingers = new Dictionary<string, string>();
                    tingers.Add("id", data["id"].ToString());
                    tingers.Add("userid", data["userId"].ToString());
                    tingers.Add("time", ConvertToDateTimeFormat(data["time"].ToString()));
                    tingers.Add("title", data["title"].ToString());
                    tingers.Add("visibility", data["visibility"].ToString());
                    tingers.Add("traffic", data["traffic"].ToString());
                    tingers.Add("location", data["situation"].ToString());
                    tingers.Add("description", data["description"].ToString());
                    tingers.Add("longitude", data["longitude"].ToString());
                    tingers.Add("latitude", data["latitude"].ToString());
                    tingers.Add("animalid", data["animal"].ToString());
                    tings.Add(tingers);
                }
            }

            conn.Close();
            data.Close();
            if (tings.Count > 0)
            {
                foreach (var ting in tings)
                {
                    ting.Add("username", GetTingerUserName(ting["userid"]));
                }
            }
            return tings;
        }

        private static string ConvertToDateTimeFormat(string p)
        {
            string format = "ddd d MMM HH:mm";
            DateTime tingDate = DateTime.Parse(p);
            return tingDate.ToString(format);

        }

        public static List<Dictionary<string,string>> GetTopTings(int number)
        {
            //string format = "MMM d HH:mm yyyy";
            string format = "HH:mm";
            SqlConnection conn = library.Conn();
            SqlCommand query = new SqlCommand();
            query.Connection = conn;
            query.CommandText = SQL_GET_TOP_TINGS;
            query.Parameters.Add("@tingnumber", System.Data.SqlDbType.Int).Value = number;
            conn.Open();
            SqlDataReader data = query.ExecuteReader();

            List<Dictionary<string,string>> tings = new List<Dictionary<string,string>>();

            if (data.HasRows)
            {
                while (data.Read())
                {
                    Dictionary<string, string> ting = new Dictionary<string, string>()
                    {
                        {"id", data["id"].ToString()},
                        {"title", data["title"].ToString()},
                        {"time", Convert.ToDateTime(data["time"]).ToString(format)},
                        {"name", data["name"].ToString()}
                    };
                    tings.Add(ting);
                }
            }

            conn.Close();
            data.Close();
            return tings;
        }

        public static List<Dictionary<string,string>> SearchAllArticle(string title)
        {
            SqlConnection conn = library.Conn();
            SqlCommand query = new SqlCommand();
            query.Connection = conn;
            query.CommandText = SQL_GET_ARTICLES_BASED_ON_SEARCH_STRING.Replace("#searchString#", title);
            conn.Open();
            SqlDataReader data = query.ExecuteReader();
            List<Dictionary<string,string>> results = new List<Dictionary<string,string>>();
            if (data.HasRows)
            {
                while(data.Read())
                {
                    var article = new Dictionary<string,string>(){
                        {"id", data["Id"].ToString()},
                        {"catid", data["CategoryId"].ToString()},
                        {"header", data["Header"].ToString()},
                        {"body", data["body"].ToString()}
                    };
                    results.Add(article);
                }
            }
            conn.Close();
            return results;

        }

        public static List<string> SearchAllVideo(string title)
        {
            SqlConnection conn = library.Conn();
            SqlCommand query = new SqlCommand();
            query.Connection = conn;
            query.CommandText = SQL_GET_VIDEO.Replace("#searchString#", title);
            conn.Open();
            SqlDataReader data = query.ExecuteReader();
            List<string> results = new List<string>();
            if (data.HasRows)
            {
                while (data.Read())
                {
                        results.Add(data["youtubeId"].ToString());
                }
            }
            conn.Close();
            return results;

        }

        public static List<Dictionary<string,string>> SearchAllImages(string tag)
        {
            List<Dictionary<string,string>> images = new List<Dictionary<string,string>>();
            SqlConnection conn = library.Conn();
            SqlCommand query = new SqlCommand();
            query.Connection = conn;
            query.CommandText = SQL_GET_IMAGE.Replace("#searchTag#", tag);
            conn.Open();
            SqlDataReader data = query.ExecuteReader();
            if (data.HasRows)
            {
                while(data.Read())
                {
                    Dictionary<string, string> image = new Dictionary<string, string>(){
                        {"id", data["id"].ToString()},
                        {"title", data["title"].ToString()},
                        {"comment", data["generalComment"].ToString()},
                        {"area", data["area"].ToString()},
                        {"filename", data["sixFiftyBYsixFifty"].ToString()},
                        {"activity", data["activity"].ToString()},
                    };
                    images.Add(image);

                }
            }

            return images;
        }

        public static Dictionary<string, string> GetTingInfo(string id)
        {
            SqlConnection conn = library.Conn();
            SqlCommand query = new SqlCommand();
            query.Connection = conn;
            Dictionary<string, string> ting = null;
            query.CommandText = SQL_GET_TING_INFO;
            query.Parameters.Add("@tingid", System.Data.SqlDbType.VarChar).Value = id;
            conn.Open();
            SqlDataReader data = query.ExecuteReader();
            if (data.HasRows)
            {
                while(data.Read())
                {
                    ting = new Dictionary<string, string>()
                    {
                        {"time", ConvertToDateTimeFormat(data["time"].ToString())},
                        {"title", data["title"].ToString()},
                        {"visibility", data["visibility"].ToString()},
                        {"traffic", data["traffic"].ToString()},
                        {"location", data["situation"].ToString()},
                        {"description", data["description"].ToString()},
                        {"longitude", data["longitude"].ToString()},
                        {"latitude", data["latitude"].ToString()},
                        {"animalid", data["animal"].ToString()},
                        {"lodgeId", data["lodgeId"].ToString()},
                        {"tingUser", GetTingerUserName(data["userId"].ToString())},
                        {"tingid", data["id"].ToString()}
                    };
                }
            }
            conn.Close();
            data.Close();
            return ting; 
        }

        public static List<Dictionary<string, string>> GetParks()
        {
            SqlConnection conn = library.Conn();
            SqlCommand query = new SqlCommand();
            query.Connection = conn;
            Dictionary<string, string> park = null;
            List<Dictionary<string, string>> parks = new List<Dictionary<string, string>>();
            query.CommandText = SQL_GET_PARKS;
            conn.Open();
            SqlDataReader data = query.ExecuteReader();
            if (data.HasRows)
            {
                while (data.Read())
                {
                    park = new Dictionary<string, string>()
                    {
                        {"id", data["id"].ToString()},
                        {"name", data["name"].ToString()}
                    };
                    parks.Add(park);
                }
            }
            conn.Close();
            data.Close();
            return parks;
        }

        
        public static List<Dictionary<string, string>> GetParkTingsByDate(Guid parkid)
        {
            SqlConnection conn = library.Conn();
            SqlCommand query = new SqlCommand();
            query.Connection = conn;
            query.CommandText = SQL_GET_KRUGER_TINGS;
            query.Parameters.Add("@pid", System.Data.SqlDbType.VarChar).Value = parkid.ToString();
            conn.Open();
            SqlDataReader data = query.ExecuteReader();

            List<Dictionary<string, string>> tings = new List<Dictionary<string, string>>();
            Dictionary<string, string> tingers;

            if (data.HasRows)
            {
                while (data.Read())
                {
                    tingers = new Dictionary<string, string>();
                    tingers.Add("id", data["id"].ToString());
                    tingers.Add("userid", data["userId"].ToString());
                    tingers.Add("time", ConvertToDateTimeFormat(data["time"].ToString()));
                    tingers.Add("title", data["title"].ToString());
                    tingers.Add("visibility", data["visibility"].ToString());
                    tingers.Add("traffic", data["traffic"].ToString());
                    tingers.Add("location", data["situation"].ToString());
                    tingers.Add("description", data["description"].ToString());
                    tingers.Add("longitude", data["longitude"].ToString());
                    tingers.Add("latitude", data["latitude"].ToString());
                    tingers.Add("animalid", data["animal"].ToString());
                    tings.Add(tingers);
                }
            }

            conn.Close();
            data.Close();
            if (tings.Count > 0)
            {
                foreach (var ting in tings)
                {
                    ting.Add("username", GetTingerUserName(ting["userid"]));
                }
            }
            return tings;
        }
    }
}