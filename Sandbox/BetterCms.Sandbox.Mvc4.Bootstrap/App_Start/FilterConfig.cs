using System.Web;
using System.Web.Mvc;

namespace BetterCms.Sandbox.Mvc4.Bootstrap
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}