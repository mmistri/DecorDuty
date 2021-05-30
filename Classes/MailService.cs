using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mail;

namespace DecorDuty.Classes
{
    public class MailService
    {
        BusinessLogic objBL = new BusinessLogic();
        HelperClass helperClass = new HelperClass();

        string FromName = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["FromName"]);
        string From = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["From"]);
        string EmailOnException = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["EmailOnException"]);
        string MailHeader = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["MailHeader"]);
        string MailHeaderValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["MailHeaderValue"]);
        string MailServer = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["MailServer"]);
        string MailServerUser = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["MailServerUser"]);
        string MailServerPassword = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["MailServerPassword"]);
        string MailServerPort = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["MailServerPort"]);
        string MailServerAuthenticate = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["MailServerAuthenticate"]);
        string MailTemplateRoot = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["MailTemplateRoot"]);

        public void SendExceptionDetailsForSP(string methodName, string SPName, string parameters, string exception, string url)
        {
            try
            {
                string ExceptionId = objBL.addException(exception, "Exception in SP : " + SPName);

                System.Web.Mail.MailMessage msgMail = new System.Web.Mail.MailMessage();
                msgMail.To = EmailOnException.Replace(",", ";");

                msgMail.Headers.Add("From", string.Format("{0} <{1}>", FromName, From));
                msgMail.Subject = "Exception While Executing SP \"" + SPName + "\" in " + FromName + " at Server Time \"" + DateTime.Now.ToString() + "\"";
                msgMail.Priority = System.Web.Mail.MailPriority.High;
                msgMail.BodyFormat = MailFormat.Html;
                msgMail.Body += "<b>Exception ID: </b>" + ExceptionId + "<br /><br />";
                msgMail.Body += "<b>Exception: </b><br/>" + exception + "<br /><br /><b>Method Name: </b>" + methodName + "<br><br><b>SPName: </b>" + SPName + "<br><br><b>Parameters: </b>" + parameters + "<br /><br><b>Example: </b><span style='color:red'>" + SPName + " " + parameters + "</span><br><b>Request URL: </b>" + url;

                if (MailHeader != "")
                {
                    msgMail.Headers.Add(MailHeader, MailHeaderValue);
                }

                msgMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", MailServerPort);
                msgMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", MailServerAuthenticate);
                msgMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", MailServerUser);
                msgMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", MailServerPassword);

                SmtpMail.SmtpServer = MailServer;
                Thread task = new Thread(() =>
                {
                    SmtpMail.Send(msgMail);
                }
                );
                task.IsBackground = true;
                task.Start();
            }
            catch (Exception ex)
            {
                objBL.addException(ex.ToString(), "Exception While Sending Exception Email");
            }
        }

        public void SendExceptionDetailsForSite(string controllerName, string actionName, string requestURL, string exception)
        {
            try
            {
                string ExceptionId = objBL.addException(exception, "Exception in " + FromName + " Site : " + requestURL);

                string headers = "";
                foreach (var key in HttpContext.Current.Request.Headers.AllKeys)
                    headers += key + "=" + HttpContext.Current.Request.Headers[key] + Environment.NewLine;


                System.Web.Mail.MailMessage msgMail = new System.Web.Mail.MailMessage();
                msgMail.To = EmailOnException.Replace(",", ";");

                msgMail.Headers.Add("From", string.Format("{0} <{1}>", FromName, From));
                msgMail.Subject = "Exception in " + FromName + " Site at Server Time \"" + DateTime.Now.ToString() + "\"";
                msgMail.Priority = System.Web.Mail.MailPriority.High;
                msgMail.BodyFormat = MailFormat.Html;
                msgMail.Body += "<b>Exception ID: </b>" + ExceptionId + "<br /><br />";
                msgMail.Body += "<b>Exception: </b><br/>" + exception + "<br /><br /><b>Controller Name: </b>" + controllerName + "<br><br><b>Action Name: </b>" + actionName + "<br><br><b>Request URL: </b>" + requestURL + "<br><br><b>Request Headers: </b>" + headers;

                if (MailHeader != "")
                {
                    msgMail.Headers.Add(MailHeader, MailHeaderValue);
                }

                msgMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", MailServerPort);
                msgMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", MailServerAuthenticate);
                msgMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", MailServerUser);
                msgMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", MailServerPassword);

                SmtpMail.SmtpServer = MailServer;
                Thread task = new Thread(() =>
                {
                    SmtpMail.Send(msgMail);
                }
                );
                task.IsBackground = true;
                task.Start();
            }
            catch (Exception ex)
            {
                objBL.addException(ex.ToString(), "Exception While Sending Exception Email");
            }
        }

        public string getEmailTemplateString(string Body, string EmailTitle)
        {
            string TemplateString = File.ReadAllText(MailTemplateRoot + "/MainTemplate.html");
            TemplateString = TemplateString.Replace("|EmailTitle|", EmailTitle);
            TemplateString = TemplateString.Replace("|Body|", Body);
            helperClass.replaceCommonVariables(ref TemplateString);
            return TemplateString;
        }

    }
}