using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;

namespace DecorDuty.Classes
{
    public class HelperClass
    {
        public static void HandleException(string methodName, string SPName, List<SqlParameter> param, string exception)
        {
            StringBuilder sb = new StringBuilder();
            if (param != null)
                for (int i = 0; i < param.Count; i++)
                {
                    SqlParameter parameter = param.ElementAt(i);
                    string name = Convert.ToString(parameter.ParameterName);
                    string value = Convert.ToString(parameter.Value);
                    sb.Append(name + "='" + value + "', ");
                }

            string url = string.Empty;
            try
            {
                url = Convert.ToString(HttpContext.Current.Request.Url.AbsoluteUri);
            }
            catch { }

            MailService mailService = new MailService();
            Thread task = new Thread(() =>
            {
                mailService.SendExceptionDetailsForSP(methodName, SPName, sb.ToString(), exception, url);
            });
            task.IsBackground = true;
            task.Start();
        }

        public string GetIPAddress()
        {
            HttpContext context = HttpContext.Current;
            string ipAddress = string.Empty; //Getting a user IP behind a proxy (HTTP_X_FORWARDED_FOR)

            if (HttpContext.Current.Request.Headers["X-Sucuri-ClientIP"] != null)
            {
                ipAddress = Convert.ToString(HttpContext.Current.Request.Headers["X-Sucuri-ClientIP"]);
            }
            else if (HttpContext.Current.Request.Headers["X-Forwarded-For"] != null)
            {
                ipAddress = Convert.ToString(HttpContext.Current.Request.Headers["X-Forwarded-For"]);
            }
            else if (context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                ipAddress = Convert.ToString(HttpContext.Current.Request.Headers["HTTP_X_FORWARDED_FOR"]);
            }
            else if (context.Request.ServerVariables["REMOTE_ADDR"] != null)
            {
                ipAddress = Convert.ToString(HttpContext.Current.Request.Headers["REMOTE_ADDR"]);
            }
            return ipAddress;
        }

        public string replaceCommonVariables(ref string Input)
        {
            Input = Input.Replace("|CurrentYear|", DateTime.Now.Year.ToString());
            Input = Input.Replace("|ImageRootURL|", AppSettings.ImageRootURL);
            Input = Input.Replace("|EmailImageRootURL|", AppSettings.EmailImageRootURL);
            Input = Input.Replace("|ProductImageRootURL|", AppSettings.ProductImageRootURL);
            Input = Input.Replace("|TollFreeNumber|", AppSettings.TollFreeNumber);
            Input = Input.Replace("|CallCenterTime|", AppSettings.CallCenterTime);
            Input = Input.Replace("|SupportEmail|", AppSettings.SupportEmail);
            Input = Input.Replace("|ManagerEmail|", AppSettings.ManagerEmail);
            Input = Input.Replace("|SiteName|", AppSettings.SiteName);
            Input = Input.Replace("|MailHeaderValue|", AppSettings.MailHeaderValue);
            Input = Input.Replace("|SiteURL|", AppSettings.SiteURL);
            Input = Input.Replace("|Facebook|", AppSettings.Facebook);
            Input = Input.Replace("|Twitter|", AppSettings.Twitter);
            Input = Input.Replace("|Pinterest|", AppSettings.Pinterest);
            Input = Input.Replace("|Instagram|", AppSettings.Instagram);
            Input = Input.Replace("|FirstOrderDiscPercentage|", AppSettings.FirstOrderDiscPercentage);
            Input = Input.Replace("|FirstOrderDiscCoupon|", AppSettings.FirstOrderDiscCoupon);
            Input = Input.Replace("|NextOrderDiscPercentage|", AppSettings.NextOrderDiscPercentage);
            Input = Input.Replace("|NextOrderDiscCoupon|", AppSettings.NextOrderDiscCoupon);
            return Input;
        }
    }
}