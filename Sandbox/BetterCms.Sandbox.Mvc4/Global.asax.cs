using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

using Autofac;

using BetterCms.Api;
using BetterCms.Core;
using BetterCms.Core.Dependencies;
using BetterCms.Core.Environment.Host;
using BetterCms.Core.Modules.Registration;
using BetterCms.Core.Modules.Projections;

using Common.Logging;

namespace BetterCms.Sandbox.Mvc4
{
    public class MvcApplication : HttpApplication
    {
        public const string UserRolesKey = "UserRoles";

        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private static ICmsHost cmsHost;

        protected void Application_Start()
        {     
            cmsHost = CmsContext.RegisterHost();

            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var userRoles = new List<string> { "User", "Admin" };
//            var container = ContextScopeProvider.CreateChildContainer();
//            var modulesRegistration = container.Resolve<IModulesRegistration>();
//            var roles = modulesRegistration.GetUserAccessRoles().Select(m => m.Name).ToList();
//            userRoles.AddRange(roles);
            Application[UserRolesKey] = userRoles;
            
            cmsHost.OnApplicationStart(this);
            
            AddPageEvents();
            AddRedirectEvents();
            AddTagEvents();
            AddCategoryEvents();
            AddWidgetEvents();
            AddBlogPostEvents();
            AddBlogAuthorEvents();
            AddMediaManagerEvents();
        }

        private void AddMediaManagerEvents()
        {
            MediaManagerApiContext.Events.MediaFileUploaded += args =>
            {
                Log.Info("MediaFileUploaded:" + args.Item.ToString());
            };

            MediaManagerApiContext.Events.MediaFileUpdated += args =>
            {
                Log.Info("MediaFileUpdated:" + args.Item.ToString());
            };

            MediaManagerApiContext.Events.MediaFileDeleted += args =>
            {
                Log.Info("MediaFileDeleted:" + args.Item.ToString());
            };

            MediaManagerApiContext.Events.MediaFolderCreated += args =>
            {
                Log.Info("MediaFolderCreated:" + args.Item.ToString());
            };

            MediaManagerApiContext.Events.MediaFolderUpdated += args =>
            {
                Log.Info("MediaFolderUpdated:" + args.Item.ToString());
            };

            MediaManagerApiContext.Events.MediaFolderDeleted += args =>
            {
                Log.Info("MediaFolderDeleted:" + args.Item.ToString());
            };
        }

        private void AddBlogPostEvents()
        {
            BlogsApiContext.Events.BlogCreated += args =>
            {
                Log.Info("BlogCreated:" + args.Item.ToString());
            };

            BlogsApiContext.Events.BlogUpdated += args =>
            {
                Log.Info("BlogUpdated:" + args.Item.ToString());
            };

            BlogsApiContext.Events.BlogDeleted += args =>
            {
                Log.Info("BlogDeleted:" + args.Item.ToString());
            };
        }

        private void AddBlogAuthorEvents()
        {
            BlogsApiContext.Events.AuthorCreated += args =>
            {
                Log.Info("AuthorCreated:" + args.Item.ToString());
            };

            BlogsApiContext.Events.AuthorUpdated += args =>
            {
                Log.Info("AuthorUpdated:" + args.Item.ToString());
            };

            BlogsApiContext.Events.AuthorDeleted += args =>
            {
                Log.Info("AuthorDeleted:" + args.Item.ToString());
            };    
        }

        private void AddWidgetEvents()
        {
            PagesApiContext.Events.WidgetCreated += args =>
            {
                Log.Info("WidgetCreated:" + args.Item.ToString());
            };

            PagesApiContext.Events.WidgetUpdated += args =>
            {
                Log.Info("WidgetUpdated:" + args.Item.ToString());
            };

            PagesApiContext.Events.WidgetDeleted += args =>
            {
                Log.Info("WidgetDeleted:" + args.Item.ToString());
            };
        }
        
        private void AddCategoryEvents()
        {
            PagesApiContext.Events.CategoryCreated += args =>
            {
                Log.Info("CategoryCreated:" + args.Item.ToString());
            };

            PagesApiContext.Events.CategoryUpdated += args =>
            {
                Log.Info("CategoryUpdated:" + args.Item.ToString());
            };

            PagesApiContext.Events.CategoryDeleted += args =>
            {
                Log.Info("CategoryDeleted:" + args.Item.ToString());
            };
        }

        private void AddTagEvents()
        {
            PagesApiContext.Events.TagCreated += args =>
                {
                    Log.Info("TagCreated:" + args.Item.ToString());
                };

            PagesApiContext.Events.TagUpdated += args =>
            {
                Log.Info("TagUpdated:" + args.Item.ToString());
            };

            PagesApiContext.Events.TagDeleted += args =>
            {
                Log.Info("TagDeleted:" + args.Item.ToString());
            };
        }

        private void AddRedirectEvents()
        {
            PagesApiContext.Events.RedirectCreated += args =>
                {
                    Log.Info("RedirectCreated:" + args.Item.ToString());
                };

            PagesApiContext.Events.RedirectUpdated += args =>
                {
                    Log.Info("RedirectUpdated:" + args.Item.ToString());
                };

            PagesApiContext.Events.RedirectDeleted += args =>
                {
                    Log.Info("RedirectDeleted:" + args.Item.ToString());
                };
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
                    var principal = new GenericPrincipal(identity, ((List<string>)Application[MvcApplication.UserRolesKey]).ToArray());
                    Context.User = principal;
                }
            }
        }
    }
}