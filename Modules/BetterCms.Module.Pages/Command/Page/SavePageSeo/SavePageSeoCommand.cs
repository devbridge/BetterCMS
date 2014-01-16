using System;
using System.Collections.Generic;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Seo;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Helpers;

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
        /// Initializes a new instance of the <see cref="SavePageSeoCommand" /> class.
        /// </summary>
        /// <param name="redirectService">The redirect service.</param>
        /// <param name="pageService">The page service.</param>
        /// <param name="sitemapService">The sitemap service.</param>
        /// <param name="urlService">The URL service.</param>
        public SavePageSeoCommand(IRedirectService redirectService, IPageService pageService,
            ISitemapService sitemapService, IUrlService urlService)
        {
            this.pageService = pageService;
            this.sitemapService = sitemapService;
            this.redirectService = redirectService;
            this.urlService = urlService;
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
            var page = Repository.First<PageProperties>(model.PageId);

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

            Repository.Save(page);
            UnitOfWork.Commit();

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