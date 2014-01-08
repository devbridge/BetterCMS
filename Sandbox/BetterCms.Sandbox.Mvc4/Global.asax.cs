using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

using BetterCms.Core;
using BetterCms.Core.Environment.Host;
using BetterCms.Core.Modules.Projections;
using BetterCms.Events;
using BetterCms.Sandbox.Mvc4.Helpers;

using Common.Logging;

namespace BetterCms.Sandbox.Mvc4
{
    public class MvcApplication : HttpApplication
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private static ICmsHost cmsHost;

        private static List<string> usersToForceRelogin = new List<string>();

        protected void Application_Start()
        {
            RouteTable.Routes.MapRoute(
                                 name: "Default",
                                 url: "demo/{controller}/{action}/{id}",
                                 defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                             ); 

            cmsHost = CmsContext.RegisterHost();

            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            RouteConfig.RegisterRoutes(RouteTable.Routes);

            cmsHost.OnApplicationStart(this);
            


            AddPageEvents();
            AddSitemapEvents();
            AddRedirectEvents();
            AddTagEvents();
            AddCategoryEvents();
            AddWidgetEvents();
            AddBlogPostEvents();
            AddBlogAuthorEvents();
            AddMediaManagerEvents();
            AddUsersEvents();
        }

        private void AddMediaManagerEvents()
        {
            BetterCms.Events.MediaManagerEvents.Instance.MediaFileUploaded += args =>
            {
                Log.Info("MediaFileUploaded:" + args.Item.ToString());
            };

            BetterCms.Events.MediaManagerEvents.Instance.MediaFileUpdated += args =>
            {
                Log.Info("MediaFileUpdated:" + args.Item.ToString());
            };

            BetterCms.Events.MediaManagerEvents.Instance.MediaFileDeleted += args =>
            {
                Log.Info("MediaFileDeleted:" + args.Item.ToString());
            };

            BetterCms.Events.MediaManagerEvents.Instance.MediaFolderCreated += args =>
            {
                Log.Info("MediaFolderCreated:" + args.Item.ToString());
            };

            BetterCms.Events.MediaManagerEvents.Instance.MediaFolderUpdated += args =>
            {
                Log.Info("MediaFolderUpdated:" + args.Item.ToString());
            };

            BetterCms.Events.MediaManagerEvents.Instance.MediaFolderDeleted += args =>
            {
                Log.Info("MediaFolderDeleted:" + args.Item.ToString());
            };
        }

        private void AddBlogPostEvents()
        {
            BetterCms.Events.BlogEvents.Instance.BlogCreated += args =>
            {
                Log.Info("BlogCreated:" + args.Item.ToString());
            };

            BetterCms.Events.BlogEvents.Instance.BlogUpdated += args =>
            {
                Log.Info("BlogUpdated:" + args.Item.ToString());
            };

            BetterCms.Events.BlogEvents.Instance.BlogDeleted += args =>
            {
                Log.Info("BlogDeleted:" + args.Item.ToString());
            };
        }

        private void AddBlogAuthorEvents()
        {
            BetterCms.Events.BlogEvents.Instance.AuthorCreated += args =>
            {
                Log.Info("AuthorCreated:" + args.Item.ToString());
            };

            BetterCms.Events.BlogEvents.Instance.AuthorUpdated += args =>
            {
                Log.Info("AuthorUpdated:" + args.Item.ToString());
            };

            BetterCms.Events.BlogEvents.Instance.AuthorDeleted += args =>
            {
                Log.Info("AuthorDeleted:" + args.Item.ToString());
            };    
        }

        private void AddWidgetEvents()
        {
            BetterCms.Events.PageEvents.Instance.WidgetCreated += args =>
            {
                Log.Info("WidgetCreated:" + args.Item.ToString());
            };

            BetterCms.Events.PageEvents.Instance.WidgetUpdated += args =>
            {
                Log.Info("WidgetUpdated:" + args.Item.ToString());
            };

            BetterCms.Events.PageEvents.Instance.WidgetDeleted += args =>
            {
                Log.Info("WidgetDeleted:" + args.Item.ToString());
            };
        }
        
        private void AddCategoryEvents()
        {
            BetterCms.Events.PageEvents.Instance.CategoryCreated += args =>
            {
                Log.Info("CategoryCreated:" + args.Item.ToString());
            };

            BetterCms.Events.PageEvents.Instance.CategoryUpdated += args =>
            {
                Log.Info("CategoryUpdated:" + args.Item.ToString());
            };

            BetterCms.Events.PageEvents.Instance.CategoryDeleted += args =>
            {
                Log.Info("CategoryDeleted:" + args.Item.ToString());
            };
        }

        private void AddTagEvents()
        {
            BetterCms.Events.PageEvents.Instance.TagCreated += args =>
                {
                    Log.Info("TagCreated:" + args.Item.ToString());
                };

            BetterCms.Events.PageEvents.Instance.TagUpdated += args =>
            {
                Log.Info("TagUpdated:" + args.Item.ToString());
            };

            BetterCms.Events.PageEvents.Instance.TagDeleted += args =>
            {
                Log.Info("TagDeleted:" + args.Item.ToString());
            };
        }

        private void AddRedirectEvents()
        {
            BetterCms.Events.PageEvents.Instance.RedirectCreated += args =>
                {
                    Log.Info("RedirectCreated:" + args.Item.ToString());
                };

            BetterCms.Events.PageEvents.Instance.RedirectUpdated += args =>
                {
                    Log.Info("RedirectUpdated:" + args.Item.ToString());
                };

            BetterCms.Events.PageEvents.Instance.RedirectDeleted += args =>
                {
                    Log.Info("RedirectDeleted:" + args.Item.ToString());
                };
        }

        private void AddPageEvents()
        {
            BetterCms.Events.RootEvents.Instance.PageRendering += Events_PageRendering;

            BetterCms.Events.PageEvents.Instance.PageCreated += args =>
                {
                    Log.Info("PageCreated: " + args.Item.ToString());
                };

            BetterCms.Events.PageEvents.Instance.PageCloned += args =>
            {
                Log.Info("PageCloned: " + args.Item.ToString());
            };

            BetterCms.Events.PageEvents.Instance.PageDeleted += args =>
            {
                Log.Info("PageDeleted: " + args.Item.ToString());
            };

            BetterCms.Events.PageEvents.Instance.PageContentInserted += args =>
            {
                Log.Info("PageContentInserted: " + args.Item.ToString());
            };

            BetterCms.Events.PageEvents.Instance.PagePropertiesChanged += args =>
            {
                Log.Info("PagePropertiesChanged: " + args.Item.ToString());
            };

            BetterCms.Events.PageEvents.Instance.PagePublishStatusChanged += args =>
            {
                Log.Info("PagePublishStatusChanged: " + args.Item.ToString());
            };

            BetterCms.Events.PageEvents.Instance.PageSeoStatusChanged += args =>
            {
                Log.Info("PageSeoStatusChanged: " + args.Item.ToString());
            };

            BetterCms.Events.PageEvents.Instance.PagePropertiesChanging += args =>
            {
                Log.Info("PagePropertiesChanging: BeforeUpdate: " + args.BeforeUpdate.ToString() + "; AfterUpdate: " + args.AfterUpdate.ToString());
            };
        }

        private void AddSitemapEvents()
        {
            BetterCms.Events.SitemapEvents.Instance.SitemapNodeCreated += args =>
            {
                Log.Info("SitemapNodeCreated: " + args.Item.ToString());
            };

            BetterCms.Events.SitemapEvents.Instance.SitemapNodeUpdated += args =>
            {
                Log.Info("SitemapNodeUpdated: " + args.Item.ToString());
            };

            BetterCms.Events.SitemapEvents.Instance.SitemapNodeDeleted += args =>
            {
                Log.Info("SitemapNodeDeleted: " + args.Item.ToString());
            };

            BetterCms.Events.SitemapEvents.Instance.SitemapCreated += args =>
            {
                Log.Info("SitemapCreated: " + args.Item.Title);
            };

            BetterCms.Events.SitemapEvents.Instance.SitemapUpdated += args =>
            {
                Log.Info("SitemapUpdated: " + args.Item.Title);
            };

            BetterCms.Events.SitemapEvents.Instance.SitemapDeleted += args =>
            {
                Log.Info("SitemapDeleted: " + args.Item.Title);
            };
        }

        private void AddUsersEvents()
        {
            BetterCms.Events.UserEvents.Instance.UserDeleted += args =>
                {
                    Log.Info("UserDeleted: " + args.Item.ToString());
                    usersToForceRelogin.Add(args.Item.UserName);
                };

            BetterCms.Events.UserEvents.Instance.UserProfileUpdated += args =>
            {
                Log.Info("UserProfileUpdated: " + args.AfterUpdate.ToString());

                if (args.BeforeUpdate != null && args.AfterUpdate != null && args.AfterUpdate.UserName != args.BeforeUpdate.UserName)
                {
                    AuthenticationHelper.Logout();

                    var roles = Roles.GetRolesForUser(args.AfterUpdate.UserName);
                    AuthenticationHelper.CreateTicket(roles, args.AfterUpdate.UserName);
                }
            };
        }


        void Events_PageRendering(PageRenderingEventArgs args)
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
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            // Users module covers it:

            //var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            //var roleCokie = Request.Cookies[Roles.CookieName];

            //if (authCookie != null)
            //{
            //    try
            //    {
            //        var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            //        if (authTicket != null)
            //        {
            //            var identity = new FormsIdentity(authTicket);
            //            var principal = roleCokie == null ? new RolePrincipal("BetterCmsRoleProvider", identity) : new RolePrincipal(identity, roleCokie.Value);
            //            Context.User = principal;
            //        }
            //    }
            //    catch
            //    {
            //        Session.Clear();
            //        FormsAuthentication.SignOut();
            //    }
            //}

            // Super simple example how to force deleted user reauthentication.
            if (User != null && usersToForceRelogin.Contains(User.Identity.Name))
            {
                if (HttpContext.Current.Session != null)
                {
                    HttpContext.Current.Session.Clear();
                }

                if (Roles.Enabled)
                {
                    Roles.DeleteCookie();
                }

                if (FormsAuthentication.IsEnabled)
                {
                    FormsAuthentication.SignOut();
                }

                Response.Redirect(FormsAuthentication.LoginUrl);
            }

            cmsHost.OnAuthenticateRequest(this);
        }
    }
}