using System;

using BetterCms.Api;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Seo;
using BetterCms.Module.Root.Mvc;

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

            if (!string.Equals(model.PageUrlPath, model.ChangedUrlPath, StringComparison.OrdinalIgnoreCase))
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

                page.NodeCountInSitemap = model.UpdateSitemap
                    ? sitemapService.ChangeUrl(page.PageUrl, model.ChangedUrlPath)
                    : sitemapService.NodesWithUrl(model.ChangedUrlPath);

                page.PageUrl = model.ChangedUrlPath;
            }

            page.MetaTitle = model.MetaTitle;
            page.MetaKeywords = model.MetaKeywords;
            page.MetaDescription = model.MetaDescription;

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

            return new EditSeoViewModel { PageUrlPath = page.PageUrl };
        }
    }
}