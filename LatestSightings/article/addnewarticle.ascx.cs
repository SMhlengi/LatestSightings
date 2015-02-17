using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using LatestSightingsLibrary;

namespace LatestSightings
{
    public partial class addnewarticle : System.Web.UI.UserControl
    {
        protected SqlConnection conn = new SqlConnection();
        protected SqlCommand query = new SqlCommand();
        protected SqlDataReader data = null;
        protected bool CategoriesCreated;
        protected bool justSavedArticle = false;
        protected List<Dictionary<string, string>> categories = null;


        protected void Page_Load(object sender, EventArgs e)
        {
            ConfigureConnection();
            if (!IsPostBack)
            {
                CategoriesCreated = library.HavingCategories(conn, query);
                if (CategoriesCreated)
                    categories = library.GetAllCategories(conn, query, data);
            }
        }

        private void ConfigureConnection()
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            query.Connection = conn;
        }

        protected void saveArticle_Click(object sender, EventArgs e)
        {
            string image = "";
            if (articleImage.HasFile)
            {
                this.articleImage.SaveAs(ConfigurationManager.AppSettings["saveArticleImagePath"] + this.articleImage.FileName);
                image = "'" + this.articleImage.FileName + "'";
                ConfigureConnection();
                query.CommandText = "Update [dbo].[Article]  SET [Picture] = " + image + " where id = " + System.Web.HttpContext.Current.Session["savedArticleId"] + "";
                conn.Open();
                query.ExecuteNonQuery();
                conn.Close();
            }

            categories = library.GetAllCategories(conn, query, data);

            UserInfo.AddAlert("Article was successfully added", GritterMessage.GritterMessageType.success);

            justSavedArticle = true;
            Response.Redirect("/articles");
        }
    }
}