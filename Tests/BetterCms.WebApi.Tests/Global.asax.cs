using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

using BetterCms.Core;
using BetterCms.Core.Environment.Host;
using BetterCms.WebApi.Tests.App_Start;

namespace BetterCms.WebApi.Tests
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        private static ICmsHost cmsHost;

        protected void Application_Start()
        {
            cmsHost = CmsContext.RegisterHost();

            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            cmsHost.OnApplicationStart(this);
        }

        protected void Application_BeginRequest()
        {
            cmsHost.OnBeginRequest(this);
        }

        protected void Application_EndRequest()
        {
            cmsHost.OnEndRequest(this);
        }

        protected void Application_Error()
        {            
            cmsHost.OnApplicationError(this);
        }

        protected void Application_End()
        {
            cmsHost.OnApplicationEnd(this);
        }
    }
}