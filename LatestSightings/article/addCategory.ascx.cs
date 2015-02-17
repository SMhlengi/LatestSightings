using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using LatestSightingsLibrary;

namespace LatestSightings
{
    public partial class addCategory : System.Web.UI.UserControl
    {
        #region Properties
        protected SqlConnection conn = new SqlConnection();
        protected SqlCommand query = new SqlCommand();
        protected SqlDataReader data = null;
        protected int page;
        protected int categoryId;
        protected List<Dictionary<string, string>> articles = null;
        protected List<Dictionary<string, string>> categories = null;
        public string Category_Id { get; set; }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            GetAndSetParameters();
            ConfigureConnection();
            ConfigureQuery();

            categories = library.GetAllCategories(conn, query, data);
        }

        private void GetAndSetParameters()
        {
            if (null != HttpContext.Current.Request.QueryString["page"])
            {
                this.page = Convert.ToInt32(HttpContext.Current.Request.QueryString["page"]);
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