using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace LatestSightings
{
    public class ExHandler
    {
        public static DateTime LastEmailDate { get; set; }
        public static int errorCount { get; set; }

        public static void RecordErrorToFile(string ex)
        {
            LogMessage(ex);
        }

        public static void RecordError(string ex)
        {
            string error = ex;

            StringBuilder sb = new StringBuilder();
            sb.Append("Error");
            sb.Append(" - " + DateTime.Now.ToString("dd-MM-yyyy HH:mm"));
            sb.Append(" - " + error);

            LogError(error);
        }

        public static void RecordError(Exception ex)
        {
            string error = ex.Message;
            if (ex.InnerException != null && !String.IsNullOrEmpty(ex.InnerException.Message))
                error = ex.InnerException.Message;

            StringBuilder sb = new StringBuilder();
            sb.Append("Error");
            sb.Append(" - " + DateTime.Now.ToString("dd-MM-yyyy HH:mm"));
            sb.Append(" - " + error);

            LogError(error);
        }

        public static void RecordError(Exception ex, string message)
        {
            string error = ex.Message;
            if (ex.InnerException != null && !String.IsNullOrEmpty(ex.InnerException.Message))
                error = ex.InnerException.Message;

            StringBuilder sb = new StringBuilder();
            sb.Append("Error");
            sb.Append(" - " + DateTime.Now.ToString("dd-MM-yyyy HH:mm"));
            sb.Append(" - " + message);
            sb.Append(" - " + error);

            LogError(error);
        }

        private static void LogError(string error)
        {
            LogMessage(error);
            EmailMessage(error);
        }

        private static void LogMessage(string errorText)
        {
            ArchiveLog();

            try
            {
                File.AppendAllText("c:/mediachannel/errors.txt", errorText);
            }
            catch (Exception ex)
            {

            }
        }

        private static void ArchiveLog()
        {
            try
            {
                if (!File.Exists("c:/mediachannel/errors.txt"))
                {
                    FileStream fs = new FileStream("c:/mediachannel/errors.txt", FileMode.Create);
                    fs.Dispose();
                }

                FileInfo fileInfo = new FileInfo("c:/mediachannel/errors.txt");
                if (fileInfo.Length > 1000000)
                {
                    if (!Directory.Exists("c:/mediachannel/LogArchive"))
                        Directory.CreateDirectory("c:/mediachannel/LogArchive");

                    File.Copy("c:/mediachannel/errors.txt", "c:/mediachannel/LogArchive/errors_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".txt");
                    FileStream fs = new FileStream("c:/mediachannel/errors.txt", FileMode.Create);
                    fs.Dispose();
                }

                fileInfo = null;
            }
            catch (Exception ex)
            {

            }
        }

        private static void EmailMessage(string errorText)
        {
            errorCount += 1;
            bool sendEmail = DateTime.Now.Subtract(LastEmailDate).TotalMinutes >= 20 ? true : false;

            if (sendEmail)
            {
                try
                {
                    MailMessage message = new MailMessage(ConfigurationManager.AppSettings["emailFromAddress"], ConfigurationManager.AppSettings["emailToAddress"]);
                    message.IsBodyHtml = true;
                    SmtpClient smtpClient = new SmtpClient();
                    smtpClient.Port = 25;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Host = ConfigurationManager.AppSettings["emailHost"];
                    NetworkCredential nc = new NetworkCredential(ConfigurationManager.AppSettings["emailUser"], ConfigurationManager.AppSettings["emailPassword"]);
                    smtpClient.Credentials = nc;
                    message.Subject = "Media Channel Error";
                    message.Body = GetEmailMessage(errorText);
                    message.Body += "<br /><br />Error Count = " + errorCount.ToString();
                    smtpClient.Send(message);

                    errorCount = 0;
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    LastEmailDate = DateTime.Now;
                }
            }
        }

        public static void SendEmail(string subject, string text)
        {
            try
            {
                MailMessage message = new MailMessage(ConfigurationManager.AppSettings["emailFromAddress"], ConfigurationManager.AppSettings["emailToAddress"]);
                message.IsBodyHtml = true;
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Port = 25;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Host = ConfigurationManager.AppSettings["emailHost"];
                NetworkCredential nc = new NetworkCredential(ConfigurationManager.AppSettings["emailUser"], ConfigurationManager.AppSettings["emailPassword"]);
                smtpClient.Credentials = nc;
                message.Subject = subject;
                message.Body = text;
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {

            }
        }

        private static string GetEmailMessage(string errorText)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Error details for media channel<br /><br />");
            sb.Append("Error: " + errorText);
            return sb.ToString();
        }
    }
}
