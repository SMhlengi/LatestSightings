using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data.SqlClient;
using System.Configuration;

namespace LatestSightings
{
    public partial class AjaxFunction : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        #region LOGIN
        [WebMethod]
        public static Dictionary<string, string> ValidateCredentials(string username, string password)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            Dictionary<string, string> userCredentials = new Dictionary<string, string>();

            SqlCommand sqlQuery = new SqlCommand("Select [dbo].[Admins].Username, [dbo].[Admins].Password from [dbo].[Admins] where username = '" + username + "' and password = '" + password + "'", connection);
            connection.Open();
            SqlDataReader RsRec = sqlQuery.ExecuteReader();
            if (RsRec.HasRows)
            {
                while (RsRec.Read())
                {
                    userCredentials.Add("status", "valid");
                    userCredentials.Add("username", RsRec["Username"].ToString());
                    System.Web.HttpContext.Current.Session["loggedInStatus"] = true;
                }
            }
            else
            {
                userCredentials.Add("status", "invalid");
            }

            RsRec.Close();
            connection.Close();

            return userCredentials;
        }
        #endregion

        #region LOGOUT
        [WebMethod]
        public static bool logout(string sessionName)
        {
            try
            {
                System.Web.HttpContext.Current.Session[sessionName] = false;
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region SaveCategory
        [WebMethod]
        public static Dictionary<string,string> SaveCategory(string category)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            SqlCommand query = new SqlCommand();
            SqlDataReader data = null;
            query.Connection = conn;
            string categoryid = "";
            Dictionary<string, string> usercategory = new Dictionary<string, string>();

            if (DoesCategoryAlreadyExists(category, conn, query, data))
            {
                usercategory.Add("categoryExists", "1");
            }
            else
            {
                query.CommandText = "Insert into [dbo].[Category] values('" + category + "') SELECT SCOPE_IDENTITY () As categoryid";
                conn.Open();
                data = query.ExecuteReader();
                if (data.HasRows)
                {
                    data.Read();
                    categoryid = Convert.ToString(data["categoryid"]);
                }

                conn.Close();
                data.Close();

                usercategory.Add("categoryname", category);
                usercategory.Add("categoryid", categoryid);
                usercategory.Add("categoryExists", "0");
            }
            return usercategory;
        }

        private static bool DoesCategoryAlreadyExists(string category, SqlConnection conn, SqlCommand query, SqlDataReader data)
        {
            bool categoryFound = false;
            query.CommandText = "Select * from [dbo].Category where name = '" + category.ToLower() +"'";
            conn.Open();
            data = query.ExecuteReader();
            if (data.HasRows)
                categoryFound = true;
            data.Close();
            conn.Close();
            return categoryFound;
        }
        #endregion 

        #region SaveArticle
        [WebMethod]
        public static Dictionary<string, int> saveArticle(string heading, string body, string categoryid, string draft, string complete)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            SqlCommand query = new SqlCommand();
            query.Connection = conn;
            Dictionary<string, int> articleId = new Dictionary<string, int>()
            {
                {"articleSaved", 0}
            };

            heading = Utils.escapeApostrophe(heading);
            heading = Utils.escapeDoubleQuotation(heading);
            body = Utils.escapeApostrophe(body);
            body = Utils.escapeDoubleQuotation(body);

            int draftmode = 0;
            int completed = 0;

            if (draft == "1")
                draftmode = 1;
            if (complete == "1")
                completed = 1;
             
            query.CommandText = "Insert into [dbo].Article (CategoryID, Header, Body, SiteId, Draft, Complete) values (" +
                                categoryid + "," +
                                "'" + heading + "'," +
                                "'" + body + "'," +
                                "1," +
                                draftmode + "," +
                                completed + ") Select SCOPE_IDENTITY () as ArticleId";
            conn.Open();
            SqlDataReader data = query.ExecuteReader();
            if (data.HasRows)
            {
                data.Read();
                System.Web.HttpContext.Current.Session["savedArticleId"] = Convert.ToInt32(data["ArticleId"]);
                articleId["articleSaved"] = 1;
            }
            conn.Close();
            data.Close();
            return articleId;
        }
        #endregion
        #region Update Article
        [WebMethod]
        public static Dictionary<string, int> updateArticle(string heading, string body, string categoryid, string draft, string complete, string articleId)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            SqlCommand query = new SqlCommand();
            query.Connection = conn;
            Dictionary<string, int> article = new Dictionary<string, int>()
            {
                {"articleSaved", 0}
            };

            heading = Utils.escapeApostrophe(heading);
            heading = Utils.escapeDoubleQuotation(heading);
            body = Utils.escapeApostrophe(body);
            body = Utils.escapeDoubleQuotation(body);

            int draftmode = 0;
            int completed = 0;

            if (draft == "1")
                draftmode = 1;
            if (complete == "1")
                completed = 1;

            query.CommandText = "Update [dbo].Article set " +
                "CategoryID = " + categoryid + "," +
                 "Header = '" + heading + "'," +
                 "Body = '" + body + "'," +
                 "SiteId = 1," +
                 "Draft = " + draftmode + "," +
                 "Complete = " + completed +
                 "where id = " + articleId;

            conn.Open();
            query.ExecuteNonQuery();
            conn.Close();
            article["articleSaved"] = 1;
            return article;
        }
        #endregion

        #region DeleteArticle
        [WebMethod]
        public static Dictionary<string,string> DeleteArticle(string articleId)
        {
            Dictionary<string, string> status = new Dictionary<string, string>();
            try
            {
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
                SqlCommand query = new SqlCommand();
                query.Connection = conn;
                query.CommandText = "Delete from [dbo].[Article] where id = " + articleId;
                conn.Open();
                query.ExecuteNonQuery();
                conn.Close();
                status.Add("successfull", "1");
            }
            catch (Exception ex)
            {
                status.Add("successfull", "0");
            }
            return status;
        }
        #endregion

        #region UpdateCategory
        [WebMethod]
        public static Dictionary<string, string> UpdateCategory(string category, string id)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            SqlCommand query = new SqlCommand();
            SqlDataReader data = null;
            query.Connection = conn;

            Dictionary<string, string> usercategory = new Dictionary<string, string>();

            if (DoesCategoryAlreadyExists(category, conn, query, data))
            {
                usercategory.Add("categoryExists", "1");
            }
            else
            {
                query.CommandText = "Update [dbo].[category] set Name = '" + category + "' where id = " + id;
                conn.Open();
                query.ExecuteNonQuery();
                conn.Close();
                usercategory.Add("categoryExists", "0");
            }
            return usercategory;
        }
        #endregion

        #region Delete Category
        [WebMethod]
        public static Dictionary<string, string> DeleteCategory(string id)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            SqlCommand query = new SqlCommand();
            SqlDataReader data = null;
            query.Connection = conn;

            Dictionary<string, string> usercategory = new Dictionary<string, string>();

            if (AreTheArticlesForThisCategory(id, conn, query, data))
            {
                usercategory.Add("articlesExists", "1");
                if (DoesUnassignedCategoryExists(conn, query, data, "Unassigned"))
                {
                    usercategory.Add("UnassignedCategoryExists", "1");
                    string UnassignedCategoryId = GetUnassignedCategoryId(conn, query, data, "Unassigned");
                    SetAllArticlesWithDeletedCategoryIdToBeUnassignedCategoryId(UnassignedCategoryId, id, conn, query);
                    DeleteThisCat(id, query, conn);
                    usercategory.Add("categoryDeleted", "1");
                }
                else
                {
                    usercategory.Add("UnassignedCategoryExists", "0");
                }
            }
            else
            {
                DeleteThisCat(id, query, conn);
                usercategory.Add("articlesExists", "0");
                usercategory.Add("categoryDeleted", "1");
            }
            return usercategory;
        }

        private static void DeleteThisCat(string id, SqlCommand query, SqlConnection conn)
        {
            query.CommandText = "Delete from [dbo].[category] where id = " + id;
            conn.Open();
            query.ExecuteNonQuery();
            conn.Close();
        }

        private static void SetAllArticlesWithDeletedCategoryIdToBeUnassignedCategoryId(string UnassignedCategoryId, string id, SqlConnection conn, SqlCommand query)
        {
            query.CommandText = "Update [dbo].[Article] Set CategoryId = " + UnassignedCategoryId + " where CategoryId = " + id;
            conn.Open();
            query.ExecuteNonQuery();
            conn.Close();
        }

        private static string GetUnassignedCategoryId(SqlConnection conn, SqlCommand query, SqlDataReader data, string categoryName)
        {
            string id = "";
            query.CommandText = "Select id from [dbo].[Category] where name = '" + categoryName + "'";
            conn.Open();
            data = query.ExecuteReader();
            if (data.HasRows)
            {
                while (data.Read())
                {
                    id = data["id"].ToString();
                }
            }
            conn.Close();
            data.Close();
            return id;

        }

        private static bool DoesUnassignedCategoryExists(SqlConnection conn, SqlCommand query, SqlDataReader data, string categoryName)
        {
            query.CommandText = "Select * from [dbo].[Category] where name = '" +  categoryName.ToLower() + "'";
            conn.Open();
            data = query.ExecuteReader();
            if (data.HasRows)
            {
                conn.Close();
                return true;
            }
            conn.Close();
            return false;
        }

        private static bool AreTheArticlesForThisCategory(string id, SqlConnection conn, SqlCommand query, SqlDataReader data)
        {
            query.CommandText = "Select Top 1 * from [dbo].[Article] where CategoryId = " + id;
            conn.Open();
            data = query.ExecuteReader();
            if (data.HasRows)
            {
                conn.Close();
                data.Close();
                return true;
            }
            conn.Close();
            data.Close();
            return false;
        }
        #endregion
        #region Create Unassigned Category
        [WebMethod]
        public static void CreateUnassignedCategory()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            SqlCommand query = new SqlCommand();
            query.Connection = conn;

            query.CommandText = "Insert into [dbo].[Category] values ('unassigned')";
            conn.Open();
            query.ExecuteNonQuery();
            conn.Close();
        }
        #endregion
    }
}