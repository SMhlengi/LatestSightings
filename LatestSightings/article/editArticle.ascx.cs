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
    public partial class editArticle : System.Web.UI.UserControl
    {
        public Boolean justUpdatedArticle = false;
        public int articleId;
        protected SqlConnection conn = new SqlConnection();
        protected SqlDataReader data = null;
        protected SqlCommand query = new SqlCommand();
        public Dictionary<string, string> article = new Dictionary<string, string>();
        public bool justUpdateArticle = false;
        public List<Dictionary<string, string>> categories = new List<Dictionary<string, string>>();
        public string articleContent;

        protected void Page_Load(object sender, EventArgs e)
        {
            ConfigureQuery();
            article = library.GetArticle(articleId, conn, query, data);
            if (!IsPostBack)
            {
                article = library.GetArticle(articleId, conn, query, data);
                categories = library.GetAllCategories(conn, query, data);
                articleContent = article["body"].ToString();
            }
        }

        private void ConfigureQuery()
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            query.Connection = conn;
        }

        protected void articleUpdate_Click(object sender, EventArgs e)
        {
            string image = "";
            if (articleImage.HasFile)
            {
                this.articleImage.SaveAs(ConfigurationManager.AppSettings["saveArticleImagePath"] + this.articleImage.FileName);
                image = "'" + this.articleImage.FileName + "'";
                ConfigureQuery();
                query.CommandText = "Update [dbo].[Article]  SET [Picture] = " + image + " where id = " + articleId;
                conn.Open();
                query.ExecuteNonQuery();
                conn.Close();
            }

            justUpdateArticle = true;
            UserInfo.AddAlert("Article was successfully updated", GritterMessage.GritterMessageType.success);
            Response.Redirect("/articles");
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {

        }
    }
}