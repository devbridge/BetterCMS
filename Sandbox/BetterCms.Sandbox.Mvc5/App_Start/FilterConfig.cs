using System.Web;
using System.Web.Mvc;

namespace BetterCms.Sandbox.Mvc5
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}