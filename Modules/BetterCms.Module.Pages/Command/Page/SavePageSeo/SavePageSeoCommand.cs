using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Security;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Models.Events;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Seo;

using BetterCms.Module.Root;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Helpers;

using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.DataAccess.DataContext.Fetching;
using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Pages.Command.Page.SavePageSeo
{
    /// <summary>
    /// A command to save SEO information.
    /// </summary>
    public class SavePageSeoCommand : CommandBase, ICommand<EditSeoViewModel, EditSeoViewModel>
    {
        /// <summary>
        /// The page service
        /// </summary>
        private readonly IPageService pageService;

        /// <summary>
        /// The sitemap service.
        /// </summary>
        private readonly ISitemapService sitemapService;

        /// <summary>
        /// The redirect service
        /// </summary>
        private readonly IRedirectService redirectService;

        /// <summary>
        /// The url service
        /// </summary>
        private readonly IUrlService urlService;

        /// <summary>
        /// The CMS configuration.
        /// </summary>
        private readonly ICmsConfiguration cmsConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="SavePageSeoCommand" /> class.
        /// </summary>
        /// <param name="redirectService">The redirect service.</param>
        /// <param name="pageService">The page service.</param>
        /// <param name="sitemapService">The sitemap service.</param>
        /// <param name="urlService">The URL service.</param>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        public SavePageSeoCommand(
            IRedirectService redirectService,
            IPageService pageService,
            ISitemapService sitemapService,
            IUrlService urlService,
            ICmsConfiguration cmsConfiguration)
        {
            this.pageService = pageService;
            this.sitemapService = sitemapService;
            this.redirectService = redirectService;
            this.urlService = urlService;
            this.cmsConfiguration = cmsConfiguration;
        }

        /// <summary>
        /// Saves SEO information.
        /// </summary>
        /// <param name="model">The SEO information model.</param>
        /// <returns>
        /// true if SEO information saved successfully; false otherwise.
        /// </returns>
        public virtual EditSeoViewModel Execute(EditSeoViewModel model)
        {
            var pageQuery =
                Repository.AsQueryable<PageProperties>(p => p.Id == model.PageId)
                          .FetchMany(p => p.Options)
                          .Fetch(p => p.Layout)
                          .ThenFetchMany(l => l.LayoutOptions)
                          .FetchMany(p => p.MasterPages)
                          .AsQueryable();

            if (cmsConfiguration.Security.AccessControlEnabled)
            {
                pageQuery = pageQuery.FetchMany(f => f.AccessRules);
            }

            var page = pageQuery.ToList().FirstOne();
            var beforeChange = new UpdatingPagePropertiesModel(page);

            var roles = new[] { RootModuleConstants.UserRoles.EditContent };
            if (cmsConfiguration.Security.AccessControlEnabled)
            {
                AccessControlService.DemandAccess(page, Context.Principal, AccessLevel.ReadWrite, roles);
            }
            else
            {
                AccessControlService.DemandAccess(Context.Principal, roles);
            }

            bool initialHasSeo = page.HasSEO;
            Models.Redirect newRedirect = null;

            page.Version = model.Version;
            page.Title = model.PageTitle;

            model.ChangedUrlPath = urlService.FixUrl(model.ChangedUrlPath);

            IList<SitemapNode> updatedNodes = null;
            if (!string.Equals(model.PageUrlPath, model.ChangedUrlPath))
            {
                pageService.ValidatePageUrl(model.ChangedUrlPath, model.PageId);

                if (model.CreatePermanentRedirect)
                {
                    var redirect = redirectService.CreateRedirectEntity(model.PageUrlPath, model.ChangedUrlPath);
                    if (redirect != null)
                    {
                        Repository.Save(redirect);
                        newRedirect = redirect;
                    }
                }

                if (model.UpdateSitemap)
                {
                    updatedNodes = sitemapService.ChangeUrlsInAllSitemapsNodes(page.PageUrl, model.ChangedUrlPath);
                }

                page.PageUrl = model.ChangedUrlPath;
            }

            page.PageUrlHash = page.PageUrl.UrlHash();
            page.MetaTitle = model.MetaTitle;
            page.MetaKeywords = model.MetaKeywords;
            page.MetaDescription = model.MetaDescription;
            page.UseCanonicalUrl = model.UseCanonicalUrl;

            // Notify about page properties changing.
            var cancelEventArgs = Events.PageEvents.Instance.OnPagePropertiesChanging(beforeChange, new UpdatingPagePropertiesModel(page));
            if (cancelEventArgs.Cancel)
            {
                Context.Messages.AddError(cancelEventArgs.CancellationErrorMessages.ToArray());
                return null;
            }

            Repository.Save(page);
            UnitOfWork.Commit();

            Events.PageEvents.Instance.OnPagePropertiesChanged(page);

            // Notify about SEO change.
            if (page.HasSEO != initialHasSeo)
            {
                Events.PageEvents.Instance.OnPageSeoStatusChanged(page);
            }

            // Notify about new redirect creation.
            if (newRedirect != null)
            {
                Events.PageEvents.Instance.OnRedirectCreated(newRedirect);
            }

            // Notify about updated sitemap nodes.
            if (updatedNodes != null)
            {
                var updatedSitemaps = new List<Models.Sitemap>();
                foreach (var node in updatedNodes)
                {
                    Events.SitemapEvents.Instance.OnSitemapNodeUpdated(node);
                    if (!updatedSitemaps.Contains(node.Sitemap))
                    {
                        updatedSitemaps.Add(node.Sitemap);
                    }
                }

                foreach (var updatedSitemap in updatedSitemaps)
                {
                    Events.SitemapEvents.Instance.OnSitemapUpdated(updatedSitemap);
                }
            }

            return new EditSeoViewModel { PageUrlPath = page.PageUrl };
        }
    }
}