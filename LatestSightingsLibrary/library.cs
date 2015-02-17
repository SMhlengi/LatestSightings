using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace LatestSightingsLibrary
{
    public class library
    {
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

            query.CommandText = "Select * from [dbo].[Article] where [Article].CategoryID = " + id;
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
    }
}