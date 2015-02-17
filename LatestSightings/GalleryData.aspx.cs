using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Services;
using System.Web.Services;
using LatestSightingsLibrary;

namespace LatestSightings
{
    public partial class GalleryData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public static List<GalleryItem> GetGallery(string type, string queryText)
        {
            List<GalleryItem> items = new List<GalleryItem>();

            if (String.IsNullOrEmpty(queryText))
            {
                items = Galleries.GetGallery(Galleries.GalleryType.Video, 50);
            }
            else
            {
                items = Galleries.GetGallery(Galleries.GalleryType.Video, queryText);
            }

            return items;
        }

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public static List<GalleryItem> GetFeatured(string type)
        {
            List<GalleryItem> items = new List<GalleryItem>();

            items = Galleries.GetFeatured(Galleries.GalleryType.Video);

            return items;
        }

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public static string InsertNewFeatured(string type, string id)
        {
            return Galleries.InsertFeatured(Galleries.GalleryType.Video, id).ToString();
        }

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public static string UpdateFeaturedOrder(string type, string itemList)
        {
            return Galleries.UpdateFeaturedOrder(Galleries.GalleryType.Video, itemList).ToString();
        }
    }
}