using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace DecorDuty.Models
{
    public class CommonPageModel
    {
    }
}


public static class AppSettings
{
    public static bool IsIPCheck
    {
        get { return Convert.ToBoolean(ConfigurationManager.AppSettings["IsIPCheck"]); }
    }
    public static string MaxMind_URL
    {
        get { return ConfigurationManager.AppSettings["MaxMind_URL"]; }
    }
    public static string MaxMind_Credentials
    {
        get { return ConfigurationManager.AppSettings["MaxMind_Credentials"]; }
    }
    public static string ImageRootURL
    {
        get { return ConfigurationManager.AppSettings["ImageRootURL"]; }
    }
    public static string ImageRootPath
    {
        get { return ConfigurationManager.AppSettings["ImageRootPath"]; }
    }
    public static string EmailImageRootURL
    {
        get { return ConfigurationManager.AppSettings["EmailImageRootURL"]; }
    }
    public static string EmailProductImageRootURL
    {
        get { return ConfigurationManager.AppSettings["EmailImageRootURL"] + "/ProductImagesNew"; }
    }
    public static string ProductImageRootURL
    {
        get { return ConfigurationManager.AppSettings["ImageRootURL"] + "/ProductImagesNew"; }
    }
    public static string TollFreeNumber
    {
        get { return ConfigurationManager.AppSettings["TollFreeNumber"]; }
    }
    public static string CallCenterTime
    {
        get { return ConfigurationManager.AppSettings["CallCenterTime"]; }
    }
    public static string SupportEmail
    {
        get { return ConfigurationManager.AppSettings["SupportEmail"]; }
    }
    public static string ManagerEmail
    {
        get { return ConfigurationManager.AppSettings["ManagerEmail"]; }
    }
    public static string SiteName
    {
        get { return ConfigurationManager.AppSettings["SiteName"]; }
    }
    public static string MailHeaderValue
    {
        get { return ConfigurationManager.AppSettings["MailHeaderValue"]; }
    }
    public static string SiteURL
    {
        get { return ConfigurationManager.AppSettings["SiteURL"]; }
    }
    public static string Facebook
    {
        get { return ConfigurationManager.AppSettings["Facebook"]; }
    }
    public static string Twitter
    {
        get { return ConfigurationManager.AppSettings["Twitter"]; }
    }
    public static string Pinterest
    {
        get { return ConfigurationManager.AppSettings["Pinterest"]; }
    }
    public static string Instagram
    {
        get { return ConfigurationManager.AppSettings["Instagram"]; }
    }
    public static string FirstOrderDiscPercentage
    {
        get { return ConfigurationManager.AppSettings["FirstOrderDiscPercentage"]; }
    }
    public static string FirstOrderDiscCoupon
    {
        get { return ConfigurationManager.AppSettings["FirstOrderDiscCoupon"]; }
    }
    public static string NextOrderDiscPercentage
    {
        get { return ConfigurationManager.AppSettings["NextOrderDiscPercentage"]; }
    }
    public static string NextOrderDiscCoupon
    {
        get { return ConfigurationManager.AppSettings["NextOrderDiscCoupon"]; }
    }
}