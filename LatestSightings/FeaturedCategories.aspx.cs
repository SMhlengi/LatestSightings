using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LatestSightingsLibrary;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.HtmlControls;

namespace LatestSightings
{
    public partial class FeaturedCategories : System.Web.UI.Page
    {
        protected SqlConnection conn = new SqlConnection();
        protected SqlCommand query = new SqlCommand();
        protected SqlDataReader data = null;
        private const int itemCount = 10;

        protected void Page_Load(object sender, EventArgs e)
        {
            ConfigureConnection();
            ConfigureQuery();

            if (!Page.IsPostBack)
            {
                FillLists();
            }

            var master = Master as DefaultMaster;
            master.SetHeader("Categories", "list-alt");
            master.SetActiveNav("sitecategories");
        }

        protected void Save(object sender, EventArgs e)
        {
            string itemsList = string.Empty;

            for (int i = 1; i <= itemCount; i++)
            {
                DropDownList select = (DropDownList)main.FindControl("ddlCategory" + i.ToString());
                if (!String.IsNullOrEmpty(select.SelectedValue))
                {
                    if (!String.IsNullOrEmpty(itemsList))
                        itemsList += ",";
                    itemsList += select.SelectedValue;
                }
            }

            if (!String.IsNullOrEmpty(itemsList))
            {
                Galleries.UpdateFeaturedOrder(Galleries.GalleryType.Category, itemsList).ToString();
                UserInfo.AddAlert("Featured categories have been updated", GritterMessage.GritterMessageType.success);
                FillLists();
            }
        }

        private void FillLists()
        {
            List<Dictionary<string, string>> categories = library.GetAllCategories(conn, query, data);
            for (int i = 1; i <= itemCount; i++)
            {
                DropDownList select = (DropDownList)main.FindControl("ddlCategory" + i.ToString());
                select.Items.Clear();
                select.Items.Add(new ListItem("----- Blank -----", ""));
            }

            if (categories != null)
            {
                foreach (var category in categories)
                {
                    for (int i = 1; i <= itemCount; i++)
                    {
                        DropDownList select = (DropDownList)main.FindControl("ddlCategory" + i.ToString());
                        select.Items.Add(new ListItem(category["categoryname"], category["id"]));
                    }
                }

                List<GalleryItem> items = Galleries.GetFeatured(Galleries.GalleryType.Category);
                if (items != null && items.Count > 0)
                {
                    int counter = 1;
                    foreach (GalleryItem item in items)
                    {
                        DropDownList select = (DropDownList)main.FindControl("ddlCategory" + counter.ToString());
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