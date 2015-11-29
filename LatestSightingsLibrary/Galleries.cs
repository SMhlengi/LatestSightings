using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace LatestSightingsLibrary
{
    public class Galleries
    {
        public enum GalleryType { Image, Video, Category, Article }
        public enum VideoSearchType { Title, Keywords }

        private const string SQL_GET_VIDEO_GALLERY_LATEST = "SELECT TOP (@quantity) a.Id, a.Title, a.youtubeId, b.Description, b.imageDefault As Url, b.imageHigh as Url2 , b.imageMedium as Url3 FROM latestsightings.dbo.videos a INNER JOIN latestsightings.dbo.youTubeVideo b ON b.Id = a.YoutubeId WHERE (a.Status = 'Published' AND a.youtubeId <> '') ORDER BY a.Created DESC";
        private const string SQL_GET_VIDEO_GALLERY_SEARCH = "SELECT a.Id, a.Title, a.youtubeId, b.Description, b.imageDefault As Url FROM latestsightings.dbo.videos a INNER JOIN latestsightings.dbo.youTubeVideo b ON b.Id = a.YoutubeId WHERE (a.Status = 'Published' AND a.youtubeId <> '' AND a.Title LIKE '%#TERM%') ORDER BY a.Title DESC";
        private const string SQL_GET_VIDEO_GALLERY_WITH_ONLY_KEYWORDS_SEARCH = "SELECT a.Id, a.Title, a.youtubeId, b.Description, b.imageDefault As Url FROM latestsightings.dbo.videos a INNER JOIN latestsightings.dbo.youTubeVideo b ON b.Id = a.YoutubeId WHERE (a.Status = 'Published' AND a.youtubeId <> '' AND a.Keywords LIKE '%#Keywords%') ORDER BY a.Title DESC";
        private const string SQL_GET_VIDEO_GALLERY_WITH_TITLE_AND_KEYWORDS_SEARCH = "SELECT a.Id, a.Title, a.youtubeId, b.Description, b.imageDefault As Url FROM latestsightings.dbo.videos a INNER JOIN latestsightings.dbo.youTubeVideo b ON b.Id = a.YoutubeId WHERE (a.Status = 'Published' AND a.youtubeId <> '' AND a.Title LIKE '%#TERM%' AND a.Keywords LIKE '%#Keywords%') ORDER BY a.Title DESC";
        private const string SQL_GET_IMAGE_GALLERY_LATEST = "SELECT TOP (@quantity) i.id, i.sixFiftyBYsixFifty, i.title, i.area, c.firstname, c.lastname from latestsightings.dbo.images i INNER JOIN latestsightings.dbo.people c ON i.contributor = c.ID  WHERE (i.display = 1) ORDER BY dateAdded DESC";
        private const string SQL_GET_VIDEO_FEATURED = "SELECT b.Id, b.Title, b.youtubeId, a.Sorting, c.Description, c.imageDefault As Url , c.imageHigh as Url2 , c.imageMedium as Url3 FROM latestsightings.dbo.featuredItems a INNER JOIN latestsightings.dbo.videos b ON b.Id = a.Id INNER JOIN latestsightings.dbo.youTubeVideo c ON c.Id = b.YouTubeId WHERE (a.Type = 'video') ORDER BY a.Sorting;";
        private const string SQL_GET_VIDEO_FEATURED_COUNT = "SELECT COUNT(Id) FROM latestsightings.dbo.featuredItems WHERE (type = 'video');";
        private const string SQL_GET_VIDEO_FEATURED_ID = "SELECT COUNT(Id) FROM latestsightings.dbo.featuredItems WHERE (type = 'video' AND id = @id);";
        private const string SQL_GET_CATEGORY_FEATURED = "SELECT b.Id, b.Name As Title, b.urlName As CategoryUrl, a.Sorting, '' As Description, '' As Url FROM latestsightings.dbo.featuredItems a INNER JOIN latestsightings.dbo.Category b ON b.Id = a.Id WHERE (a.Type = 'category') ORDER BY a.Sorting;";
        private const string SQL_GET_ARTICLE_FEATURED = "SELECT b.Id, b.Header As Title, b.CategoryID , a.Sorting, '' As Description, b.picture As Url, b.body as ArticleBody FROM latestsightings.dbo.featuredItems a INNER JOIN latestsightings.dbo.Article b ON b.Id = a.Id WHERE (a.Type = 'article') ORDER BY a.Sorting;";
        private const string SQL_INSERT_FEATURED_VIDEO_REPLACE = "DELETE FROM latestsightings.dbo.featuredItems WHERE (type = 'video' AND sorting = @sorting); INSERT INTO latestsightings.dbo.featuredItems (type, id, sorting) VALUES ('video', @id, @sorting);";
        private const string SQL_INSERT_FEATURED_VIDEO_ITEM = "INSERT INTO latestsightings.dbo.featuredItems (type, id, sorting) VALUES ('video', {0}, {1});";
        private const string SQL_INSERT_FEATURED_CATEGORY_ITEM = "INSERT INTO latestsightings.dbo.featuredItems (type, id, sorting) VALUES ('category', {0}, {1});";
        private const string SQL_INSERT_FEATURED_ARTICLE_ITEM = "INSERT INTO latestsightings.dbo.featuredItems (type, id, sorting) VALUES ('article', {0}, {1});";
        private const string SQL_DELETE_FEATURED_VIDEO = "DELETE FROM latestsightings.dbo.featuredItems WHERE (type = 'video');";
        private const string SQL_DELETE_FEATURED_CATEGORY = "DELETE FROM latestsightings.dbo.featuredItems WHERE (type = 'category');";
        private const string SQL_DELETE_FEATURED_ARTICLE = "DELETE FROM latestsightings.dbo.featuredItems WHERE (type = 'article');";

        private const string SQL_SELECT_IMAGES = "SELECT * FROM latestsightings.dbo.images WHERE (Display = @approved OR Display = @notapproved) AND (YEAR(dateAdded) = @year AND MONTH(dateAdded) = @month) ORDER BY dateAdded";
        private const string SQL_UPDATE_IMAGE_STATUS = "UPDATE latestsightings.dbo.images SET Display = @display WHERE (id = @id);";

        private const int videoFeatuered = 5;

        public static List<GalleryItem> GetGallery(GalleryType type, int quantity)
        {
            switch (type)
            {
                case GalleryType.Image:
                    return GetImageGallery(quantity, string.Empty);
                default:
                    return GetVideoGallery(quantity, string.Empty);
            }
        }

        public static List<GalleryItem> GetGallery(GalleryType type, string query)
        {
            switch (type)
            {
                case GalleryType.Image:
                    return GetImageGallery(0, query);
                default:
                    return GetVideoGallery(0, query);
            }
        }

        public static List<GalleryItem> GetGallery(GalleryType type, string query, VideoSearchType filter)
        {
            if (type == GalleryType.Video && filter == VideoSearchType.Title)
            {
                return GetVideoGallery(0, query, VideoSearchType.Title);
            }
            else if (type == GalleryType.Video && filter == VideoSearchType.Keywords)
            {
                return GetVideoGallery(0, query, VideoSearchType.Keywords);
            }
            else
            {
                List<GalleryItem> empty = new List<GalleryItem>();
                return empty;
            }
        }

        public static List<GalleryItem> GetGallery(GalleryType type, string query, string query2)
        {
            switch (type)
            {
                case GalleryType.Video:
                    return GetVideoGallery(query, query2);
            }

            List<GalleryItem> empty = new List<GalleryItem>();
            return empty;
        }

        public static List<GalleryItem> GetGallery(GalleryType type, string query, string[] keywords)
        {
            switch (type)
            {
                case GalleryType.Image:
                    return GetImageGallery(0, query);
                default:
                    List<GalleryItem> items = null;

                    if (!String.IsNullOrEmpty(query) || keywords != null)
                    {
                        List<GalleryItem> queryItems = null;
                        List<GalleryItem> keywordItems = null;

                        if (keywords != null)
                        {
                            keywordItems = GetVideoGalleryWithKeywords(keywords);
                            if (keywordItems != null && keywordItems.Count > 0)
                            {
                                if (items == null)
                                    items = new List<GalleryItem>();
                                items.AddRange(keywordItems);
                            }
                        }

                        if (!String.IsNullOrEmpty(query))
                        {
                            queryItems = GetVideoGallery(0, string.Empty);
                            if (queryItems != null && queryItems.Count > 0)
                            {
                                if (items == null)
                                    items = new List<GalleryItem>();
                                items.AddRange(queryItems);
                            }
                        }

                        if (items != null && items.Count > 0)
                        {
                            items = RankSearch(ref items, query, keywords);
                        }
                    }
                    else
                    {
                        items = GetVideoGallery(0, string.Empty);
                    
                    }
                    return items;
            }
        }

        private static List<GalleryItem> RankSearch(ref List<GalleryItem> items, string query, string[] keywords)
        {
            return items;
        }

        public static List<GalleryItem> GetImages(int year, int month, bool approved, bool notApproved)
        {
            List<GalleryItem> items = null;

            if (approved || notApproved)
            {
                SqlConnection conn = data.Conn();
                try
                {
                    conn.Open();
                    SqlCommand sqlQuery = new SqlCommand();
                    sqlQuery.Connection = conn;
                    sqlQuery.CommandText = SQL_SELECT_IMAGES;
                    sqlQuery.Parameters.Add("@approved", System.Data.SqlDbType.Bit).Value = approved;
                    sqlQuery.Parameters.Add("@notapproved", System.Data.SqlDbType.Bit).Value = notApproved == true ? false : true;
                    sqlQuery.Parameters.Add("@year", System.Data.SqlDbType.Int).Value = year;
                    sqlQuery.Parameters.Add("@month", System.Data.SqlDbType.Int).Value = month;
                    SqlDataReader rdr = sqlQuery.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        items = new List<GalleryItem>();
                        while (rdr.Read())
                        {
                            GalleryItem item = new GalleryItem();
                            item.Id = rdr["id"].ToString();
                            item.Url = ConfigurationManager.AppSettings["uploadedImagesUrlThumb"] + rdr["eightyByEighty"].ToString();
                            item.Title = ConfigurationManager.AppSettings["uploadedImagesUrl"] + rdr["original"].ToString();
                            item.Description = Convert.ToBoolean(rdr["display"]).ToString();
                            items.Add(item);
                        }
                    }
                    rdr.Close();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    ExHandler.RecordError(ex);
                }
                finally
                {
                    conn.Dispose();
                }
            }

            return items;
        }

        private static List<GalleryItem> GetImageGallery(int quantity, string query)
        {
            List<GalleryItem> items = null;
            // "SELECT TOP @quantity sixFiftyBYsixFifty, title 
            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = SQL_GET_IMAGE_GALLERY_LATEST;
                sqlQuery.Parameters.Add("quantity", System.Data.SqlDbType.Int).Value = quantity;

                SqlDataReader rdr = sqlQuery.ExecuteReader();
                if (rdr.HasRows)
                {
                    items = new List<GalleryItem>();
                    while (rdr.Read())
                    {
                        GalleryItem item = new GalleryItem();
                        item.Id = rdr["id"].ToString();
                        item.Title = rdr["title"].ToString();
                        item.Url = rdr["sixFiftyBYsixFifty"].ToString();
                        item.Firstname = rdr["firstname"].ToString();
                        item.Lastname = rdr["lastname"].ToString();
                        item.Location = rdr["area"].ToString();
                        items.Add(item);
                    }
                }
                rdr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex);
            }
            finally
            {
                conn.Dispose();
            }

            return items;
        }

        private static List<GalleryItem> GetVideoGallery(int quantity, string query)
        {
            List<GalleryItem> items = null;

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                if (!String.IsNullOrEmpty(query))
                {
                    sqlQuery.CommandText = SQL_GET_VIDEO_GALLERY_SEARCH.Replace("#TERM", query.Replace("'", ""));
                }
                else
                {
                    sqlQuery.CommandText = SQL_GET_VIDEO_GALLERY_LATEST;
                    sqlQuery.Parameters.Add("quantity", System.Data.SqlDbType.Int).Value = quantity;
                }
                SqlDataReader rdr = sqlQuery.ExecuteReader();
                if (rdr.HasRows)
                {
                    items = new List<GalleryItem>();
                    while (rdr.Read())
                    {
                        GalleryItem item = new GalleryItem();
                        item.Id = rdr["id"].ToString();
                        item.Description = rdr["description"].ToString();
                        item.Title = rdr["title"].ToString();
                        item.Url = rdr["url"].ToString();
                        item.Url2 = rdr["url2"].ToString();
                        item.Url3 = rdr["url3"].ToString();
                        item.YouTubeId = rdr["youtubeId"].ToString();
                        items.Add(item);
                    }
                }
                rdr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex);
            }
            finally
            {
                conn.Dispose();
            }

            return items;
        }

        private static List<GalleryItem> GetVideoGallery(int quantity, string query, VideoSearchType type)
        {
            List<GalleryItem> items = null;

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                if (!String.IsNullOrEmpty(query) && type == VideoSearchType.Title)
                {
                    sqlQuery.CommandText = SQL_GET_VIDEO_GALLERY_SEARCH.Replace("#TERM", query.Replace("'", ""));
                }
                else if (!String.IsNullOrEmpty(query) && type == VideoSearchType.Keywords)
                {
                    sqlQuery.CommandText = SQL_GET_VIDEO_GALLERY_WITH_ONLY_KEYWORDS_SEARCH.Replace("#Keywords", query.Replace("'", ""));
                }
                else
                {
                    sqlQuery.CommandText = SQL_GET_VIDEO_GALLERY_LATEST;
                    sqlQuery.Parameters.Add("quantity", System.Data.SqlDbType.Int).Value = quantity;
                }
                SqlDataReader rdr = sqlQuery.ExecuteReader();
                if (rdr.HasRows)
                {
                    items = new List<GalleryItem>();
                    while (rdr.Read())
                    {
                        GalleryItem item = new GalleryItem();
                        item.Id = rdr["id"].ToString();
                        item.Description = rdr["description"].ToString();
                        item.Title = rdr["title"].ToString();
                        item.Url = rdr["url"].ToString();
                        item.YouTubeId = rdr["youtubeId"].ToString();
                        items.Add(item);
                    }
                }
                rdr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex);
            }
            finally
            {
                conn.Dispose();
            }

            return items;
        }

        private static List<GalleryItem> GetVideoGallery(string title, string keywords)
        {
            List<GalleryItem> items = null;

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                if (!String.IsNullOrEmpty(title) && !String.IsNullOrEmpty(keywords))
                {
                    sqlQuery.CommandText = SQL_GET_VIDEO_GALLERY_WITH_TITLE_AND_KEYWORDS_SEARCH.Replace("#TERM", title.Replace("'", "")).Replace("#Keywords", keywords.Replace("'",""));
                }
                SqlDataReader rdr = sqlQuery.ExecuteReader();
                if (rdr.HasRows)
                {
                    items = new List<GalleryItem>();
                    while (rdr.Read())
                    {
                        GalleryItem item = new GalleryItem();
                        item.Id = rdr["id"].ToString();
                        item.Description = rdr["description"].ToString();
                        item.Title = rdr["title"].ToString();
                        item.Url = rdr["url"].ToString();
                        item.YouTubeId = rdr["youtubeId"].ToString();
                        items.Add(item);
                    }
                }
                rdr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex);
            }
            finally
            {
                conn.Dispose();
            }

            return items;
        }

        private static List<GalleryItem> GetVideoGalleryWithKeywords(string[] keywords)
        {
            List<GalleryItem> items = null;

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                SqlDataReader rdr = sqlQuery.ExecuteReader();
                if (rdr.HasRows)
                {
                    items = new List<GalleryItem>();
                    while (rdr.Read())
                    {
                        GalleryItem item = new GalleryItem();
                        item.Id = rdr["id"].ToString();
                        item.Description = rdr["description"].ToString();
                        item.Title = rdr["title"].ToString();
                        item.Url = rdr["url"].ToString();
                        items.Add(item);
                    }
                }
                rdr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex);
            }
            finally
            {
                conn.Dispose();
            }

            return items;
        }

        public static List<GalleryItem> GetFeatured(GalleryType type)
        {
            switch (type)
            {
                case GalleryType.Image:
                    return GetImageFeaturedItems();
                case GalleryType.Category:
                    return GetCategoryFeaturedItems();
                case GalleryType.Article:
                    return GetArticleFeaturedItems();
                default:
                    return GetVideoFeaturedItems();
            }
        }

        private static List<GalleryItem> GetImageFeaturedItems()
        {
            List<GalleryItem> items = null;

            return items;
        }

        private static List<GalleryItem> GetVideoFeaturedItems()
        {
            List<GalleryItem> items = null;

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = SQL_GET_VIDEO_FEATURED;
                SqlDataReader rdr = sqlQuery.ExecuteReader();
                if (rdr.HasRows)
                {
                    items = new List<GalleryItem>();
                    while (rdr.Read())
                    {
                        GalleryItem item = new GalleryItem();
                        item.Id = rdr["id"].ToString();
                        item.Description = rdr["description"].ToString();
                        item.Title = rdr["title"].ToString();
                        item.Url = rdr["url"].ToString();
                        item.Url2 = rdr["url2"].ToString();
                        item.Url3 = rdr["url3"].ToString();
                        item.Order = Convert.ToInt32(rdr["Sorting"]);
                        item.YouTubeId = rdr["youtubeId"].ToString();
                        items.Add(item);
                    }
                }
                rdr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex);
            }
            finally
            {
                conn.Dispose();
            }

            return items;
        }

        private static List<GalleryItem> GetCategoryFeaturedItems()
        {
            List<GalleryItem> items = null;

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = SQL_GET_CATEGORY_FEATURED;
                SqlDataReader rdr = sqlQuery.ExecuteReader();
                if (rdr.HasRows)
                {
                    items = new List<GalleryItem>();
                    while (rdr.Read())
                    {
                        GalleryItem item = new GalleryItem();
                        item.Id = rdr["id"].ToString();
                        item.Description = rdr["description"].ToString();
                        item.Title = rdr["title"].ToString();
                        item.Url = rdr["url"].ToString();
                        item.Url2 = rdr["CategoryUrl"].ToString();
                        item.Order = Convert.ToInt32(rdr["Sorting"]);
                        items.Add(item);
                    }
                }
                rdr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex);
            }
            finally
            {
                conn.Dispose();
            }

            return items;
        }

        private static List<GalleryItem> GetArticleFeaturedItems()
        {
            List<GalleryItem> items = null;

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.Connection = conn;
                sqlQuery.CommandText = SQL_GET_ARTICLE_FEATURED;
                SqlDataReader rdr = sqlQuery.ExecuteReader();
                if (rdr.HasRows)
                {
                    items = new List<GalleryItem>();
                    while (rdr.Read())
                    {
                        GalleryItem item = new GalleryItem();
                        item.Id = rdr["id"].ToString();
                        item.Description = rdr["description"].ToString();
                        item.Title = rdr["title"].ToString();
                        item.Url = rdr["url"].ToString();
                        item.Order = Convert.ToInt32(rdr["Sorting"]);
                        item.ArticleBody = rdr["ArticleBody"].ToString();
                        item.CateogryId = rdr["CategoryID"].ToString(); ;
                        items.Add(item);
                    }
                }
                rdr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex);
            }
            finally
            {
                conn.Dispose();
            }

            return items;
        }

        public static bool InsertFeatured(GalleryType type, string id)
        {
            switch (type)
            {
                case GalleryType.Image:
                    return InsertFeaturedImage(id);
                default:
                    return InsertFeaturedVideo(id);
            }
        }

        private static bool InsertFeaturedImage(string id)
        {
            return false;
        }

        private static bool InsertFeaturedVideo(string id)
        {
            bool inserted = false;

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();

                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.CommandText = SQL_GET_VIDEO_FEATURED_ID;
                sqlQuery.Parameters.Add("id", System.Data.SqlDbType.VarChar).Value = id;
                int present = Convert.ToInt32(data.ExecuteScalar(sqlQuery));

                if (present <= 0)
                {
                    sqlQuery = new SqlCommand();
                    sqlQuery.CommandText = SQL_GET_VIDEO_FEATURED_COUNT;
                    int items = Convert.ToInt32(data.ExecuteScalar(sqlQuery));
                    int item = items;

                    if (items < videoFeatuered)
                    {
                        item = items + 1;
                    }

                    sqlQuery = new SqlCommand();
                    sqlQuery.Connection = conn;
                    sqlQuery.CommandText = SQL_INSERT_FEATURED_VIDEO_REPLACE;
                    sqlQuery.Parameters.Add("id", System.Data.SqlDbType.VarChar).Value = id;
                    sqlQuery.Parameters.Add("sorting", System.Data.SqlDbType.VarChar).Value = item;
                    sqlQuery.ExecuteNonQuery();
                }

                conn.Close();
                inserted = true;
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex);
            }
            finally
            {
                conn.Dispose();
            }

            return inserted;
        }

        public static bool UpdateFeaturedOrder(GalleryType type, string itemList)
        {
            switch (type)
            {
                case GalleryType.Image:
                    return UpdateFeaturedImageOrder(itemList);
                case GalleryType.Category:
                    return UpdateFeaturedCategoryOrder(itemList);
                case GalleryType.Article:
                    return UpdateFeaturedArticleOrder(itemList);
                default:
                    return UpdateFeaturedVideoOrder(itemList);
            }
        }

        private static bool UpdateFeaturedImageOrder(string itemList)
        {
            bool updated = false;

            return updated;
        }

        private static bool UpdateFeaturedVideoOrder(string itemList)
        {
            bool updated = false;

            SqlConnection conn = data.Conn();
            try
            {
                if (!String.IsNullOrEmpty(itemList))
                {
                    conn.Open();
                    SqlCommand sqlQuery = new SqlCommand();
                    StringBuilder sb = new StringBuilder();
                    sb.Append(SQL_DELETE_FEATURED_VIDEO);
                    string[] items = itemList.Split(',');
                    for (int i = 0; i < items.Length; i++)
                    {
                        sb.Append(String.Format(SQL_INSERT_FEATURED_VIDEO_ITEM, "'" + items[i] + "'", (i + 1).ToString()));
                    }
                    sqlQuery.CommandText = sb.ToString();
                    updated = data.ExecuteNonQuery(sqlQuery);

                    conn.Close();
                }

                updated = true;
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex);
            }
            finally
            {
                conn.Dispose();
            }

            return updated;
        }

        private static bool UpdateFeaturedCategoryOrder(string itemList)
        {
            bool updated = false;

            SqlConnection conn = data.Conn();
            try
            {
                if (!String.IsNullOrEmpty(itemList))
                {
                    conn.Open();
                    SqlCommand sqlQuery = new SqlCommand();
                    StringBuilder sb = new StringBuilder();
                    sb.Append(SQL_DELETE_FEATURED_CATEGORY);
                    string[] items = itemList.Split(',');
                    for (int i = 0; i < items.Length; i++)
                    {
                        sb.Append(String.Format(SQL_INSERT_FEATURED_CATEGORY_ITEM, "'" + items[i] + "'", (i + 1).ToString()));
                    }
                    sqlQuery.CommandText = sb.ToString();
                    updated = data.ExecuteNonQuery(sqlQuery);

                    conn.Close();
                }

                updated = true;
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex);
            }
            finally
            {
                conn.Dispose();
            }

            return updated;
        }

        private static bool UpdateFeaturedArticleOrder(string itemList)
        {
            bool updated = false;

            SqlConnection conn = data.Conn();
            try
            {
                if (!String.IsNullOrEmpty(itemList))
                {
                    conn.Open();
                    SqlCommand sqlQuery = new SqlCommand();
                    StringBuilder sb = new StringBuilder();
                    sb.Append(SQL_DELETE_FEATURED_ARTICLE);
                    string[] items = itemList.Split(',');
                    for (int i = 0; i < items.Length; i++)
                    {
                        sb.Append(String.Format(SQL_INSERT_FEATURED_ARTICLE_ITEM, "'" + items[i] + "'", (i + 1).ToString()));
                    }
                    sqlQuery.CommandText = sb.ToString();
                    updated = data.ExecuteNonQuery(sqlQuery);

                    conn.Close();
                }

                updated = true;
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex);
            }
            finally
            {
                conn.Dispose();
            }

            return updated;
        }

        public static bool UpdateImageStatus(string id, bool status)
        {
            bool updated = false;

            SqlConnection conn = data.Conn();
            try
            {
                conn.Open();
                SqlCommand sqlQuery = new SqlCommand();
                sqlQuery.CommandText = SQL_UPDATE_IMAGE_STATUS;
                sqlQuery.Parameters.Add("display", System.Data.SqlDbType.Bit).Value = status;
                sqlQuery.Parameters.Add("id", System.Data.SqlDbType.Int).Value = id;
                updated = data.ExecuteNonQuery(sqlQuery);

                conn.Close();
                updated = true;
            }
            catch (Exception ex)
            {
                ExHandler.RecordError(ex);
            }
            finally
            {
                conn.Dispose();
            }

            return updated;
        }
    }

    public class GalleryItem
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string Url2 { get; set; }
        public string Url3 { get; set; }
        public string Keywords { get; set; }
        public int Order { get; set; }
        public bool MatchesSearch { get; set; }
        public string YouTubeId { get; set; }
        public string ArticleBody { get; set; }
        public string CateogryId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Location { get; set; }
    }
}
