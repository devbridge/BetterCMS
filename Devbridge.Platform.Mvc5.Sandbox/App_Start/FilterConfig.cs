using System.Web;
using System.Web.Mvc;

namespace Devbridge.Platform.Mvc5.Sandbox
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
