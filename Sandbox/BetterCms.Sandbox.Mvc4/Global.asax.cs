using System;
using System.Collections.Generic;
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

        private static List<string> usersToForceRelogin = new List<string>();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            RouteConfig.RegisterRoutes(RouteTable.Routes);

            AddCategoryEvents();
            AddContentEvents();
            AddLayoutEvents();
            AddPageEvents();
            AddRedirectEvents();
            AddSitemapEvents();
            AddTagEvents();
            AddWidgetEvents();
            AddLanguageEvents();
            AddBlogPostEvents();
            AddBlogAuthorEvents();
            AddMediaManagerEvents();
            AddUsersEvents();
            AddNewsletterEvents();
        }

        private void AddContentEvents()
        {
            BetterCms.Events.RootEvents.Instance.ContentRestored += args => Log.Info("ContentRestored:" + args.Item.ToString());
            BetterCms.Events.PageEvents.Instance.PageContentInserted += args => Log.Info("PageContentInserted:" + args.Item.ToString());
            BetterCms.Events.PageEvents.Instance.PageContentDeleted += args => Log.Info("PageContentDeleted:" + args.Item.ToString());
            BetterCms.Events.PageEvents.Instance.PageContentSorted += args => Log.Info("PageContentSorted:" + args.Item.ToString());
            BetterCms.Events.PageEvents.Instance.PageContentConfigured += args => Log.Info("PageContentConfigured:" + args.Item.ToString());
            BetterCms.Events.PageEvents.Instance.HtmlContentCreated += args => Log.Info("HtmlContentCreated:" + args.Item.ToString());
            BetterCms.Events.PageEvents.Instance.HtmlContentUpdated += args => Log.Info("HtmlContentUpdated:" + args.Item.ToString());
            BetterCms.Events.PageEvents.Instance.HtmlContentDeleted += args => Log.Info("HtmlContentDeleted:" + args.Item.ToString());
            BetterCms.Events.PageEvents.Instance.ContentDraftDestroyed += args => Log.Info("ContentDraftDestroyed:" + args.Item.ToString());
        }

        private void AddLayoutEvents()
        {
            BetterCms.Events.PageEvents.Instance.LayoutCreated += args => Log.Info("LayoutCreated:" + args.Item.ToString());
            BetterCms.Events.PageEvents.Instance.LayoutDeleted += args => Log.Info("LayoutDeleted:" + args.Item.ToString());
            BetterCms.Events.PageEvents.Instance.LayoutUpdated += args => Log.Info("LayoutUpdated:" + args.Item.ToString());
        }

        private void AddLanguageEvents()
        {
            BetterCms.Events.RootEvents.Instance.LanguageCreated += args => Log.Info("LanguageCreated:" + args.Item.ToString());
            BetterCms.Events.RootEvents.Instance.LanguageDeleted += args => Log.Info("LanguageDeleted:" + args.Item.ToString());
            BetterCms.Events.RootEvents.Instance.LanguageUpdated += args => Log.Info("LanguageUpdated:" + args.Item.ToString());
        }

        private void AddNewsletterEvents()
        {
            BetterCms.Events.NewsletterEvents.Instance.SubscriberCreated += args => Log.Info("SubscriberCreated:" + args.Item.ToString());
            BetterCms.Events.NewsletterEvents.Instance.SubscriberDeleted += args => Log.Info("SubscriberDeleted:" + args.Item.ToString());
            BetterCms.Events.NewsletterEvents.Instance.SubscriberUpdated += args => Log.Info("SubscriberUpdated:" + args.Item.ToString());
        }

        private void AddMediaManagerEvents()
        {
            BetterCms.Events.MediaManagerEvents.Instance.MediaFileUploaded += args => Log.Info("MediaFileUploaded:" + args.Item.ToString());
            BetterCms.Events.MediaManagerEvents.Instance.MediaFileUpdated += args => Log.Info("MediaFileUpdated:" + args.Item.ToString());
            BetterCms.Events.MediaManagerEvents.Instance.MediaFileDeleted += args => Log.Info("MediaFileDeleted:" + args.Item.ToString());
            BetterCms.Events.MediaManagerEvents.Instance.MediaFolderCreated += args => Log.Info("MediaFolderCreated:" + args.Item.ToString());
            BetterCms.Events.MediaManagerEvents.Instance.MediaFolderUpdated += args => Log.Info("MediaFolderUpdated:" + args.Item.ToString());
            BetterCms.Events.MediaManagerEvents.Instance.MediaFolderDeleted += args => Log.Info("MediaFolderDeleted:" + args.Item.ToString());
            BetterCms.Events.MediaManagerEvents.Instance.MediaArchived += args => Log.Info("MediaArchived:" + args.Item.ToString());
            BetterCms.Events.MediaManagerEvents.Instance.MediaUnarchived += args => Log.Info("MediaUnarchived:" + args.Item.ToString());
        }

        private void AddBlogPostEvents()
        {
            BetterCms.Events.BlogEvents.Instance.BlogCreated += args => Log.Info("BlogCreated:" + args.Item.ToString());
            BetterCms.Events.BlogEvents.Instance.BlogChanging += args => Log.Info("BlogChanging: BeforeUpdate: " + args.BeforeUpdate.ToString() + "; AfterUpdate: " + args.AfterUpdate.ToString());
            BetterCms.Events.BlogEvents.Instance.BlogUpdated += args => Log.Info("BlogUpdated:" + args.Item.ToString());
            BetterCms.Events.BlogEvents.Instance.BlogDeleted += args => Log.Info("BlogDeleted:" + args.Item.ToString());
        }

        private void AddBlogAuthorEvents()
        {
            BetterCms.Events.BlogEvents.Instance.AuthorCreated += args => Log.Info("AuthorCreated:" + args.Item.ToString());
            BetterCms.Events.BlogEvents.Instance.AuthorUpdated += args => Log.Info("AuthorUpdated:" + args.Item.ToString());
            BetterCms.Events.BlogEvents.Instance.AuthorDeleted += args => Log.Info("AuthorDeleted:" + args.Item.ToString());    
        }

        private void AddWidgetEvents()
        {
            BetterCms.Events.PageEvents.Instance.WidgetCreated += args => Log.Info("WidgetCreated:" + args.Item.ToString());
            BetterCms.Events.PageEvents.Instance.WidgetUpdated += args => Log.Info("WidgetUpdated:" + args.Item.ToString());
            BetterCms.Events.PageEvents.Instance.WidgetDeleted += args => Log.Info("WidgetDeleted:" + args.Item.ToString());
        }
        
        private void AddCategoryEvents()
        {
            BetterCms.Events.RootEvents.Instance.CategoryCreated += args => Log.Info("CategoryCreated:" + args.Item.ToString());
            BetterCms.Events.RootEvents.Instance.CategoryUpdated += args => Log.Info("CategoryUpdated:" + args.Item.ToString());
            BetterCms.Events.RootEvents.Instance.CategoryDeleted += args => Log.Info("CategoryDeleted:" + args.Item.ToString());
        }

        private void AddTagEvents()
        {
            BetterCms.Events.RootEvents.Instance.TagCreated += args => Log.Info("TagCreated:" + args.Item.ToString());
            BetterCms.Events.RootEvents.Instance.TagUpdated += args => Log.Info("TagUpdated:" + args.Item.ToString());
            BetterCms.Events.RootEvents.Instance.TagDeleted += args => Log.Info("TagDeleted:" + args.Item.ToString());
        }

        private void AddRedirectEvents()
        {
            BetterCms.Events.PageEvents.Instance.RedirectCreated += args => Log.Info("RedirectCreated:" + args.Item.ToString());
            BetterCms.Events.PageEvents.Instance.RedirectUpdated += args => Log.Info("RedirectUpdated:" + args.Item.ToString());
            BetterCms.Events.PageEvents.Instance.RedirectDeleted += args => Log.Info("RedirectDeleted:" + args.Item.ToString());
        }

        private void AddPageEvents()
        {
            BetterCms.Events.RootEvents.Instance.PageRendering += Events_PageRendering;
            BetterCms.Events.PageEvents.Instance.PageCreated += args => Log.Info("PageCreated: " + args.Item.ToString());
            BetterCms.Events.PageEvents.Instance.PageCloned += args => Log.Info("PageCloned: " + args.Item.ToString());
            BetterCms.Events.PageEvents.Instance.PageDeleted += args => Log.Info("PageDeleted: " + args.Item.ToString());
            BetterCms.Events.PageEvents.Instance.PagePropertiesChanged += args => Log.Info("PagePropertiesChanged: " + args.Item.ToString());
            BetterCms.Events.PageEvents.Instance.PagePublishStatusChanged += args => Log.Info("PagePublishStatusChanged: " + args.Item.ToString());
            BetterCms.Events.PageEvents.Instance.PageSeoStatusChanged += args => Log.Info("PageSeoStatusChanged: " + args.Item.ToString());
            BetterCms.Events.PageEvents.Instance.PagePropertiesChanging += args => Log.Info("PagePropertiesChanging: BeforeUpdate: " + args.BeforeUpdate.ToString() + "; AfterUpdate: " + args.AfterUpdate.ToString());
            BetterCms.Events.RootEvents.Instance.PageNotFound += args => Log.Info("PageNotFound:" + args.Item.ToString());
            BetterCms.Events.RootEvents.Instance.PageAccessForbidden += args => Log.Info("PageAccessForbidden:" + args.Item.ToString());
        }

        private void AddSitemapEvents()
        {
            BetterCms.Events.SitemapEvents.Instance.SitemapNodeCreated += args => Log.Info("SitemapNodeCreated: " + args.Item.ToString());
            BetterCms.Events.SitemapEvents.Instance.SitemapNodeUpdated += args => Log.Info("SitemapNodeUpdated: " + args.Item.ToString());
            BetterCms.Events.SitemapEvents.Instance.SitemapNodeDeleted += args => Log.Info("SitemapNodeDeleted: " + args.Item.ToString());
            BetterCms.Events.SitemapEvents.Instance.SitemapCreated += args => Log.Info("SitemapCreated: " + args.Item.Title);
            BetterCms.Events.SitemapEvents.Instance.SitemapUpdated += args => Log.Info("SitemapUpdated: " + args.Item.Title);
            BetterCms.Events.SitemapEvents.Instance.SitemapDeleted += args => Log.Info("SitemapDeleted: " + args.Item.Title);
        }

        private void AddUsersEvents()
        {
            BetterCms.Events.UserEvents.Instance.UserCreated += args => Log.Info("UserCreated:" + args.Item.ToString());
            BetterCms.Events.UserEvents.Instance.UserUpdated += args => Log.Info("UserUpdated:" + args.Item.ToString());
            BetterCms.Events.UserEvents.Instance.RoleCreated += args => Log.Info("RoleCreated:" + args.Item.ToString());
            BetterCms.Events.UserEvents.Instance.RoleDeleted += args => Log.Info("RoleDeleted:" + args.Item.ToString());
            BetterCms.Events.UserEvents.Instance.RoleUpdated += args => Log.Info("RoleUpdated:" + args.Item.ToString());

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
           // args.RenderPageData.Metadata.Add(new MetaDataProjection("test-metadata", "hello world!"));
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
        }
    }
}