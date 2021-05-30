using DecorDuty.Filters;
using System.Web.Mvc;

namespace DecorDuty
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new IPCheckFilter());
        }
    }
}