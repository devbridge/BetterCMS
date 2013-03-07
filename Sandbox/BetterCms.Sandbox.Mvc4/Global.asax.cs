using System;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

using BetterCms.Api;
using BetterCms.Core;
using BetterCms.Core.Environment.Host;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Projections;

namespace BetterCms.Sandbox.Mvc4
{
    public class MvcApplication : HttpApplication
    {
        private static ICmsHost cmsHost;

        protected void Application_Start()
        {     
            cmsHost = CmsContext.RegisterHost();

            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            
            cmsHost.OnApplicationStart(this);

            RootApiContext.Events.PageRendering += Events_PageRendering;
        }

        void Events_PageRendering(Module.Root.Api.Events.PageRenderingEventArgs args)
        {            
            args.RenderPageData.Metadata.Add(new MetaDataProjection("test-metadata", "hello world!"));
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

        /// <summary>
        /// Handles the AuthenticateRequest event of the Application control.
        /// TODO: remove when authentication will be implemented
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null)
                {
                    var identity = new GenericIdentity(authTicket.Name, "Forms");
                    var principal = new GenericPrincipal(identity, new[] { "User", "Admin" });
                    Context.User = principal;
                }
            }
        }
    }
}