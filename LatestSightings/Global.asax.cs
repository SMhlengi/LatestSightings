using LatestSightingsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace LatestSightings
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Session_OnStart(object sender, EventArgs e)
        {
            Person person = null;

            if (HttpContext.Current.Request.RawUrl.ToLower().IndexOf("login") < 0 && HttpContext.Current.Request.RawUrl.ToLower().IndexOf("logout") < 0)
            {
                if (HttpContext.Current.Request.Cookies != null)
                {
                    if (HttpContext.Current.Request.Cookies["user"] != null)
                    {
                        person = Person.GetPerson(HttpContext.Current.Request.Cookies["user"].Value.ToString());
                        HttpContext.Current.Session["user"] = person;
                    }
                }

                if (HttpContext.Current.Session != null)
                {
                    if (HttpContext.Current.Session["user"] == null)
                    {
                        HttpContext.Current.Response.Redirect("/login");
                    }
                }
            }
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapPageRoute("", "login", "~/login.aspx");
            routes.MapPageRoute("", "logout", "~/logout.aspx");
            routes.MapPageRoute("", "contributor/{id}", "~/contributor.aspx");
            routes.MapPageRoute("", "contributor", "~/contributor.aspx");
            routes.MapPageRoute("", "contributors", "~/contributors.aspx");
            routes.MapPageRoute("", "profile/{id}", "~/profile.aspx");
            routes.MapPageRoute("", "video", "~/video.aspx");
            routes.MapPageRoute("", "video/{id}", "~/video.aspx");
            routes.MapPageRoute("", "videos", "~/videos.aspx");
            routes.MapPageRoute("", "videos/featured", "~/videosorting.aspx");
            routes.MapPageRoute("", "videos/{status}", "~/videos.aspx");
            routes.MapPageRoute("", "financials", "~/financials.aspx");
            routes.MapPageRoute("", "financials/{year}/{month}", "~/financials.aspx");
            routes.MapPageRoute("", "payments", "~/payments.aspx");
            routes.MapPageRoute("", "payments/{year}/{month}", "~/payments.aspx");
            routes.MapPageRoute("", "paymentscsv/{year}/{month}", "~/paymentsdatacsv.aspx");
            routes.MapPageRoute("", "user", "~/user.aspx");
            routes.MapPageRoute("", "category/featured", "~/featuredcategories.aspx");
            routes.MapPageRoute("", "articles/featured", "~/featuredarticles.aspx");

            routes.MapPageRoute("", "articles", "~/articles.aspx");
            routes.MapPageRoute("cms-add-new-article",
                "articles/addnewarticle",
                "~/articles.aspx", true,
                 new RouteValueDictionary { 
                { "view", "addnewarticle"}
            });

            routes.MapPageRoute("cms-view-articles-by-category",
                "articles/category/{categoryid}",
                "~/articles.aspx", true,
                 new RouteValueDictionary { });

            routes.MapPageRoute("cms-add-new-category",
                "articles/addcategory",
                "~/articles.aspx", true,
                 new RouteValueDictionary { 
                { "view", "addnewcategory"}
            });

            routes.MapPageRoute("cms-view-article-board",
                "articles/articlesboard",
                "~/articles.aspx", true,
                 new RouteValueDictionary { });

            routes.MapPageRoute("cms-edit-article",
                "articles/edit/{articleid}",
                "~/articles.aspx", true,
                 new RouteValueDictionary { 
                { "view", "editarticle"}
            });

            routes.Ignore("Language/assets/{*pathInfo}");
        }
    }
}