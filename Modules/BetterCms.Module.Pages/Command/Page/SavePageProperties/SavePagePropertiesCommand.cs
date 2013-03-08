using System;
using System.Collections;
using System.Collections.Generic;

using BetterCms.Api;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Page;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.Page.SavePageProperties
{
    /// <summary>
    /// Page properties save command.
    /// </summary>
    public class SavePagePropertiesCommand : CommandBase, ICommand<EditPagePropertiesViewModel, SavePageResponse>
    {
        /// <summary>
        /// The page service
        /// </summary>
        private readonly IPageService pageService;

        /// <summary>
        /// The redirect service
        /// </summary>
        private readonly IRedirectService redirectService;

        /// <summary>
        /// The tag service
        /// </summary>
        private readonly ITagService tagService;

        /// <summary>
        /// The sitemap service.
        /// </summary>
        private readonly ISitemapService sitemapService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SavePagePropertiesCommand" /> class.
        /// </summary>
        /// <param name="pageService">The page service.</param>
        /// <param name="redirectService">The redirect service.</param>
        /// <param name="tagService">The tag service.</param>
        /// <param name="sitemapService">The sitemap service.</param>
        public SavePagePropertiesCommand(IPageService pageService, IRedirectService redirectService, ITagService tagService, ISitemapService sitemapService)
        {
            this.pageService = pageService;
            this.redirectService = redirectService;
            this.tagService = tagService;
            this.sitemapService = sitemapService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Save response.</returns>
        /// <exception cref="CmsException">Failed to save page properties.</exception>
        public SavePageResponse Execute(EditPagePropertiesViewModel request)
        {            
            UnitOfWork.BeginTransaction();

            var page = Repository.First<PageProperties>(request.Id);

            Models.Redirect redirectCreated = null;
            bool initialSeoStatus = page.HasSEO;

            request.PageUrl = redirectService.FixUrl(request.PageUrl);

            pageService.ValidatePageUrl(request.PageUrl, request.Id);

            if (!string.Equals(page.PageUrl, request.PageUrl, StringComparison.OrdinalIgnoreCase))
            {
                pageService.ValidatePageUrl(request.PageUrl, request.Id);
                if (request.RedirectFromOldUrl)
                {
                    var redirect = redirectService.CreateRedirectEntity(page.PageUrl, request.PageUrl);
                    if (redirect != null)
                    {
                        Repository.Save(redirect);
                        redirectCreated = redirect;
                    }
                }

                page.NodeCountInSitemap = request.UpdateSitemap
                    ? sitemapService.ChangeUrl(page.PageUrl, request.PageUrl)
                    : sitemapService.NodesWithUrl(request.PageUrl);

                page.PageUrl = request.PageUrl;
            }

            page.Layout = Repository.AsProxy<Root.Models.Layout>(request.TemplateId);
            page.Category = request.CategoryId.HasValue ? Repository.AsProxy<Category>(request.CategoryId.Value) : null;
            page.Title = request.PageName;
            page.CustomCss = request.PageCSS;
            page.CustomJS = request.PageJavascript;
            page.IsPublic = request.IsVisibleToEveryone;
            page.UseNoFollow = request.UseNoFollow;
            page.UseNoIndex = request.UseNoIndex;
            page.Version = request.Version;

            if (request.Image != null && request.Image.ImageId.HasValue)
            {
                page.Image = Repository.AsProxy<MediaImage>(request.Image.ImageId.Value);
            }
            else
            {
                page.Image = null;
            }

            Repository.Save(page);

            // Save tags
            IList<Root.Models.Tag> newTags;
            tagService.SavePageTags(page, request.Tags, out newTags);

            UnitOfWork.Commit();

            // Notify about page properties change.
            PagesApiContext.Events.OnPagePropertiesChanged(page);

            // Notify about redirect creation.
            if (redirectCreated != null)
            {
                PagesApiContext.Events.OnRedirectCreated(redirectCreated);
            }

            // Notify about SEO status change.
            if (initialSeoStatus != page.HasSEO)
            {
                PagesApiContext.Events.OnPageSeoStatusChanged(page);
            }

            // Notify about new tags.
            PagesApiContext.Events.OnTagCreated(newTags);

            return new SavePageResponse(page);
        }
    }
}