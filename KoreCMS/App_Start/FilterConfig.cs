using System.Web.Mvc;
using Kore.Web.Mvc.Optimization;

namespace KoreCMS
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}