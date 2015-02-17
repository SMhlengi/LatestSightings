using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LatestSightingsLibrary;

namespace LatestSightings
{
    public partial class Articles : System.Web.UI.Page
    {
        public string view;
        public string View
        {
            get
            {
                if (string.IsNullOrEmpty(view) && Page.RouteData.Values["view"] != null)
                {
                    view = Page.RouteData.Values["view"].ToString();
                }
                return view;
            }
            set
            {
                view = value;
            }
        }

        private string categoryid;
        protected string CategoryId
        {
            get
            {
                if (String.IsNullOrEmpty(categoryid) && Page.RouteData.Values["categoryid"] != null)
                {
                    categoryid = Page.RouteData.Values["categoryid"].ToString();
                }
                return categoryid;
            }
        }

        private string articleId;
        protected string ArticleId
        {
            get
            {
                if (String.IsNullOrEmpty(articleId) && Page.RouteData.Values["articleId"] != null)
                {
                    articleId = Page.RouteData.Values["articleId"].ToString();
                }
                return articleId;
            }
        }


        protected SqlConnection conn = new SqlConnection();
        protected SqlCommand query = new SqlCommand();
        protected SqlDataReader data = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DetermineControlToLoad();
            }
            else if (View == "editarticle") // THIS IS A HACK CAUSE ITS 01:25 AND WANT TO FIX IT TO GO TO SLEEP
            {
                editArticle uc_editArticle = (editArticle)LoadControl("~/article/editArticle.ascx");
                uc_editArticle.articleId = Convert.ToInt32(ArticleId);
                editArticle.Controls.Add(uc_editArticle);
            }

            SetPageHeader();
        }

        private void SetPageHeader()
        {
            var master = Master as DefaultMaster;
            switch (view)
            {
                case "norecords":
                    master.SetHeader("Articles", "edit");
                    master.SetActiveNav("sitearticles");
                    break;
                case "addnewarticle":
                    master.SetHeader("Add Article", "edit");
                    master.SetActiveNav("sitearticles");
                    break;
                case "articlesboard":
                    master.SetHeader("Articles", "edit");
                    master.SetActiveNav("sitearticles");
                    break;
                case "editarticle":
                    master.SetHeader("Edit Article", "edit");
                    master.SetActiveNav("sitearticles");
                    break;
                case "addnewcategory":
                    master.SetHeader("Categories", "list-alt");
                    master.SetActiveNav("sitecategories");
                    break;
                default:
                    master.SetActiveNav("sitearticles");
                    break;
            }
        }

        private void DetermineControlToLoad()
        {
            if (String.IsNullOrEmpty(View))
                LoadHome();
            else if (View == "editarticle")
            {
                editArticle uc_editArticle = (editArticle)LoadControl("~/article/editArticle.ascx");
                uc_editArticle.articleId = Convert.ToInt32(ArticleId);
                editArticle.Controls.Add(uc_editArticle);
            }
        }

        private void LoadHome()
        {
            ConfigureConnection();
            if (AreCategoryAndArticleTableEmpty())
                View = "norecords";
            else
            {
                View = "articlesboard";
                loadArticleBoard();
            }
        }

        private void loadArticleBoard()
        {
            articlesBoard a_board = (articlesBoard)LoadControl("~/article/articlesBoard.ascx");
            a_board.Category_Id = CategoryId;
            articlesboard_two.Controls.Add(a_board);
        }

        private bool AreCategoryAndArticleTableEmpty()
        {
            if (library.isArticleTableEmpty(conn, query) && library.isCategoryTableEmpty(conn, query))
                return true;
            return false;
        }

        private void ConfigureConnection()
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            query.Connection = conn;
        }
    }
}