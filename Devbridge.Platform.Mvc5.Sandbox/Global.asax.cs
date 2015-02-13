using System;
using System.Web.Mvc;
using System.Web.Routing;

using Devbridge.Platform.Core.Web;
using Devbridge.Platform.Core.Web.Environment.Host;

namespace Devbridge.Platform.Mvc5.Sandbox
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static IWebApplicationHost dbPlatformAppHost;

        protected void Application_Start()
        {
            dbPlatformAppHost = WebApplicationContext.RegisterHost();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            dbPlatformAppHost.OnApplicationStart(this);
        }

        protected void Application_BeginRequest()
        {
            dbPlatformAppHost.OnBeginRequest(this);
        }

        protected void Application_EndRequest()
        {
            dbPlatformAppHost.OnEndRequest(this);
        }

        protected void Application_Error()
        {
            dbPlatformAppHost.OnApplicationError(this);
        }

        protected void Application_End()
        {
            dbPlatformAppHost.OnApplicationEnd(this);
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            dbPlatformAppHost.OnAuthenticateRequest(this);
        }
    }
}
