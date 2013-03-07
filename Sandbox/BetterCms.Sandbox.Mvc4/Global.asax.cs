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

using Common.Logging;

namespace BetterCms.Sandbox.Mvc4
{
    public class MvcApplication : HttpApplication
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private static ICmsHost cmsHost;

        protected void Application_Start()
        {     
            cmsHost = CmsContext.RegisterHost();

            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            
            cmsHost.OnApplicationStart(this);
            
            AddPageEvents();
        }

        private void AddPageEvents()
        {
            RootApiContext.Events.PageRendering += Events_PageRendering;

            PagesApiContext.Events.PageCreated += args =>
                {
                    Log.Info("PageCreated: " + args.Item.ToString());
                };

            PagesApiContext.Events.PageCloned += args =>
            {
                Log.Info("PageCloned: " + args.Item.ToString());
            };

            PagesApiContext.Events.PageDeleted += args =>
            {
                Log.Info("PageDeleted: " + args.Item.ToString());
            };

            PagesApiContext.Events.PageContentInserted += args =>
            {
                Log.Info("PageContentInserted: " + args.Item.ToString());
            };

            PagesApiContext.Events.PagePropertiesChanged += args =>
            {
                Log.Info("PagePropertiesChanged: " + args.Item.ToString());
            };

            PagesApiContext.Events.PagePublishStatusChanged += args =>
            {
                Log.Info("PagePublishStatusChanged: " + args.Item.ToString());
            };

            PagesApiContext.Events.PageSeoStatusChanged += args =>
            {
                Log.Info("PageSeoStatusChanged: " + args.Item.ToString());
            };
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