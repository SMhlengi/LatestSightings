using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LatestSightings
{
    public class UserInfo
    {
        public static void AddAlert(string message, GritterMessage.GritterMessageType type)
        {
            List<GritterMessage> grits = null;

            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Session != null)
                {
                    if (HttpContext.Current.Session["grits"] != null)
                    {
                        grits = (List<GritterMessage>)HttpContext.Current.Session["grits"];
                    }
                }
            }

            if (grits == null)
                grits = new List<GritterMessage>();

            GritterMessage grit = new GritterMessage();
            grit.Created = DateTime.Now;
            grit.Message = message;
            grit.Type = type;
            grits.Add(grit);

            HttpContext.Current.Session["grits"] = grits;
        }

        public static List<GritterMessage> GetAlerts()
        {
            List<GritterMessage> grits = new List<GritterMessage>();

            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Session != null)
                {
                    if (HttpContext.Current.Session["grits"] != null)
                    {
                        grits = (List<GritterMessage>)HttpContext.Current.Session["grits"];
                    }
                }
            }

            return grits;
        }

        public static void ClearAlerts()
        {
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Session != null)
                {
                    if (HttpContext.Current.Session["grits"] != null)
                    {
                        HttpContext.Current.Session["grits"] = null;
                    }
                }
            }
        }
    }

    public class GritterMessage
    {
        public enum GritterMessageType { success, warning, info, error }

        public DateTime Created { get; set; }
        public string Message { get; set; }
        public GritterMessageType Type { get; set; }
    }
}