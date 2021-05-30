using DecorDuty.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DecorDuty.Filters
{
    public class IPCheckFilter : AuthorizeAttribute
    {
        BusinessLogic objBL = new BusinessLogic();
        HelperClass helperClass = new HelperClass();

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Request.HttpMethod == "GET")
            {
                return checkIsIPOK();
            }
            return true;
        }

        public bool checkIsIPOK()
        {
            bool IsIPOK = true;
            string strSessionIPOK = Convert.ToString(HttpContext.Current.Session["IPOK"]);
            string cityId = "0", stateId = "0", countryId = "0";
            try
            {
                bool IsIPCheck = AppSettings.IsIPCheck;

                if (IsIPCheck)
                {
                    if (string.IsNullOrEmpty(strSessionIPOK))
                    {
                        bool IsAlreadyChecked = false;
                        string UserAgent = Convert.ToString(HttpContext.Current.Request["HTTP_USER_AGENT"]);
                        string ipAdd = "", maxmindData = "", cityName = "", stateName = "", countryName = "", countryCode = "";
                        string CustomerId = Convert.ToString(HttpContext.Current.Session["CustomerId"]);

                        ipAdd = helperClass.GetIPAddress();

                        #region Check_Existing_IP

                        try
                        {
                            DataSet dsip = objBL.addIPAddress(ipAdd);
                            if (dsip != null)
                            {
                                if (dsip.Tables.Count > 0)
                                {
                                    if (dsip.Tables[0].Rows.Count > 0)
                                    {
                                        IsAlreadyChecked = Convert.ToBoolean(Convert.ToInt32(dsip.Tables[0].Rows[0]["IsAlreadyChecked"]));
                                        if (IsAlreadyChecked)
                                        {
                                            if (Convert.ToString(dsip.Tables[0].Rows[0]["IsIPOK"]).ToUpper() == "BLOCK")
                                                IsIPOK = false;

                                            cityId = Convert.ToString(dsip.Tables[0].Rows[0]["cityId"]);
                                            stateId = Convert.ToString(dsip.Tables[0].Rows[0]["stateId"]);
                                            countryId = Convert.ToString(dsip.Tables[0].Rows[0]["countryId"]);
                                        }
                                    }
                                }
                            }
                        }
                        catch
                        {
                        }

                        #endregion Check Existiong IP

                        #region IP_TO_LOCATION & SAVE_IP

                        if (!IsAlreadyChecked)
                        {
                            try
                            {
                                string MaxMind_URL = AppSettings.MaxMind_URL;
                                string MaxMind_Credentials = AppSettings.MaxMind_Credentials;
                                string url = MaxMind_URL + ipAdd + "?pretty";
                                WebRequest myReq = WebRequest.Create(url);
                                string credentials = MaxMind_Credentials;
                                CredentialCache mycache = new CredentialCache();
                                myReq.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials));
                                WebResponse wr = myReq.GetResponse();
                                Stream receiveStream = wr.GetResponseStream();
                                StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
                                maxmindData = reader.ReadToEnd();
                                Newtonsoft.Json.Linq.JObject jObject = new Newtonsoft.Json.Linq.JObject();
                                string chkCityString = maxmindData.ToLower();
                                jObject = Newtonsoft.Json.Linq.JObject.Parse(maxmindData);

                                cityName = Convert.ToString(jObject["city"]["names"]["en"]).ToLower();
                                stateName = Convert.ToString(jObject["subdivisions"][0]["names"]["en"]).ToLower();
                                countryName = Convert.ToString(jObject["country"]["names"]["en"]).ToLower();
                                if (string.IsNullOrEmpty(countryName))
                                {
                                    countryName = Convert.ToString(jObject["registered_country"]["names"]["en"]).ToLower();
                                }
                                countryCode = Convert.ToString(jObject["country"]["iso_code"]).ToUpper();
                                if (string.IsNullOrEmpty(countryCode))
                                {
                                    countryCode = Convert.ToString(jObject["registered_country"]["iso_code"]);
                                }

                                DataSet dsip = objBL.addIPAddress(ipAdd, CustomerId, maxmindData, cityName, stateName, countryName, countryCode);
                                if (dsip != null)
                                    if (dsip.Tables.Count > 0)
                                        if (dsip.Tables[0].Rows.Count > 0)
                                        {
                                            if (Convert.ToString(dsip.Tables[0].Rows[0]["IsIPOK"]).ToUpper() == "BLOCK")
                                                IsIPOK = false;
                                            cityId = Convert.ToString(dsip.Tables[0].Rows[0]["cityId"]);
                                            stateId = Convert.ToString(dsip.Tables[0].Rows[0]["stateId"]);
                                            countryId = Convert.ToString(dsip.Tables[0].Rows[0]["countryId"]);
                                        }
                            }
                            catch
                            {
                            }
                        }

                        #endregion                        

                        if (!IsIPOK)
                        {
                            HttpContext.Current.Response.StatusCode = 404;
                            HttpContext.Current.Response.End();
                            return !IsIPOK;
                        }

                        setSessionCityStateCountry(cityId, stateId, countryId);
                    }
                }
            }
            catch
            {
                IsIPOK = true;
            }

            if (string.IsNullOrEmpty(strSessionIPOK))
                HttpContext.Current.Session["IPOK"] = IsIPOK;

            return IsIPOK;
        }

        public void setSessionCityStateCountry(string cityId, string stateId, string countryId)
        {
            if (!string.IsNullOrEmpty(cityId))
                HttpContext.Current.Session["cityId"] = cityId;
            if (!string.IsNullOrEmpty(stateId))
                HttpContext.Current.Session["stateId"] = stateId;
            if (!string.IsNullOrEmpty(countryId))
                HttpContext.Current.Session["countryId"] = countryId;
        }
    }
}