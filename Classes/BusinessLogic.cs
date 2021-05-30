using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DecorDuty.Classes
{
    public class BusinessLogic
    {
        HelperClass HelperClass = new HelperClass();

        public string constr = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["Con"]);
        private string SP_IP_ADD = "SP_IP_ADD";
        private string SP_EXCEPTION_ADD = "SP_EXCEPTION_ADD";

        private void addCustomerSession(ref List<SqlParameter> param)
        {
            param.Add(new SqlParameter("@CustomerId", Convert.ToString(HttpContext.Current.Session["CustomerId"])));
        }

        public DataSet addIPAddress(string ipAddress, string customerId = "", string maxmindData = "", string cxCity = "", string cxState = "", string cxCountry = "", string cxCountryCode = "")
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@IPAddress", ipAddress));
            param.Add(new SqlParameter("@MaxmindData", maxmindData));
            param.Add(new SqlParameter("@City", cxCity));
            param.Add(new SqlParameter("@State", cxState));
            param.Add(new SqlParameter("@Country", cxCountry));
            param.Add(new SqlParameter("@CountryCode", cxCountryCode));
            addCustomerSession(ref param);
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(constr, CommandType.StoredProcedure, SP_IP_ADD, param.ToArray());
                if (ds.Tables.Count > 0)
                    return ds;
                return null;
            }
            catch (Exception e)
            {
                string MethodName = System.Reflection.MethodInfo.GetCurrentMethod().Name;
                HelperClass.HandleException(MethodName, SP_IP_ADD, param, e.ToString());
                return null;
            }
        }

        public string addException(string exception, string page)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@ex", exception));
            param.Add(new SqlParameter("@page", page));
            try
            {
                return Convert.ToString(SqlHelper.ExecuteScalar(constr, CommandType.StoredProcedure, SP_EXCEPTION_ADD, param.ToArray()));
            }
            catch
            {
                return "";
            }
        }

    }
}