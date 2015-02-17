using LatestSightingsLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LatestSightings
{
    public partial class FeaturedArticles : System.Web.UI.Page
    {
        protected SqlConnection conn = new SqlConnection();
        protected SqlCommand query = new SqlCommand();
        protected SqlDataReader data = null;
        private const int itemCount = 4;

        protected void Page_Load(object sender, EventArgs e)
        {
            ConfigureConnection();
            ConfigureQuery();

            if (!Page.IsPostBack)
            {
                FillLists();
            }

            var master = Master as DefaultMaster;
            master.SetHeader("Articles", "edit");
            master.SetActiveNav("sitearticles");
        }

        protected void Save(object sender, EventArgs e)
        {
            string itemsList = string.Empty;

            for (int i = 1; i <= itemCount; i++)
            {
                DropDownList select = (DropDownList)main.FindControl("ddlArticle" + i.ToString());
                if (!String.IsNullOrEmpty(select.SelectedValue))
                {
                    if (!String.IsNullOrEmpty(itemsList))
                        itemsList += ",";
                    itemsList += select.SelectedValue;
                }
            }

            if (!String.IsNullOrEmpty(itemsList))
            {
                Galleries.UpdateFeaturedOrder(Galleries.GalleryType.Article, itemsList).ToString();
                UserInfo.AddAlert("Featured articles have been updated", GritterMessage.GritterMessageType.success);
                FillLists();
            }
        }

        private void FillLists()
        {
            List<Dictionary<string, string>> articles = library.GetLatesArticles(20, conn, query, data);
            for (int i = 1; i <= itemCount; i++)
            {
                DropDownList select = (DropDownList)main.FindControl("ddlArticle" + i.ToString());
                select.Items.Clear();
                select.Items.Add(new ListItem("----- Blank -----", ""));
            }

            if (articles != null)
            {
                foreach (var article in articles)
                {
                    for (int i = 1; i <= itemCount; i++)
                    {
                        DropDownList select = (DropDownList)main.FindControl("ddlArticle" + i.ToString());
                        select.Items.Add(new ListItem(article["header"], article["id"]));
                    }
                }

                List<GalleryItem> items = Galleries.GetFeatured(Galleries.GalleryType.Article);
                if (items != null && items.Count > 0)
                {
                    int counter = 1;
                    foreach (GalleryItem item in items)
                    {
                        DropDownList select = (DropDownList)main.FindControl("ddlArticle" + counter.ToString());
                        if (select != null)
                        {
                            select.SelectedValue = item.Id;
                        }
                        counter++;
                    }
                }
            }
        }

        private void ConfigureConnection()
        {
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        }
        private void ConfigureQuery()
        {
            query.Connection = conn;
        }
    }
}