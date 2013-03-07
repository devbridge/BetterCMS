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
        /// Initializes a new instance of the <see cref="SavePageSeoCommand" /> class.
        /// </summary>
        /// <param name="redirectService">The redirect service.</param>
        /// <param name="pageService">The page service.</param>
        /// <param name="sitemapService">The sitemap service.</param>
        public SavePageSeoCommand(IRedirectService redirectService, IPageService pageService, ISitemapService sitemapService)
        {
            this.pageService = pageService;
            this.sitemapService = sitemapService;
            this.redirectService = redirectService;
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

            page.Version = model.Version;
            page.Title = model.PageTitle;

            model.ChangedUrlPath = redirectService.FixUrl(model.ChangedUrlPath);

            if (!string.Equals(model.PageUrlPath, model.ChangedUrlPath, StringComparison.OrdinalIgnoreCase))
            {
                pageService.ValidatePageUrl(model.ChangedUrlPath, model.PageId);

                if (model.CreatePermanentRedirect)
                {
                    var redirect = redirectService.CreateRedirectEntity(model.PageUrlPath, model.ChangedUrlPath);
                    if (redirect != null)
                    {
                        Repository.Save(redirect);
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

            if (page.HasSEO != initialHasSeo)
            {
                PagesApiContext.Events.OnPageSeoStatusChanged(page);
            }

            return new EditSeoViewModel { PageUrlPath = page.PageUrl };
        }
    }
}