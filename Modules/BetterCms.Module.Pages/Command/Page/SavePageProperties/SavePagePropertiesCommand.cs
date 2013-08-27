using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataAccess.DataContext.Fetching;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Security;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Page;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Helpers;
using BetterCms.Module.Root.Services;

using CategoryEntity = BetterCms.Module.Root.Models.Category;

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
        /// The url service
        /// </summary>
        private readonly IUrlService urlService;

        /// <summary>
        /// The options service
        /// </summary>
        private readonly IOptionService optionService;

        private readonly ICmsConfiguration cmsConfiguration;

        private readonly IAccessControlService accessControlService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SavePagePropertiesCommand" /> class.
        /// </summary>
        /// <param name="pageService">The page service.</param>
        /// <param name="redirectService">The redirect service.</param>
        /// <param name="tagService">The tag service.</param>
        /// <param name="sitemapService">The sitemap service.</param>
        /// <param name="urlService">The URL service.</param>
        /// <param name="optionService">The option service.</param>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        /// <param name="accessControlService">The access control service.</param>
        public SavePagePropertiesCommand(IPageService pageService, IRedirectService redirectService, ITagService tagService,
            ISitemapService sitemapService, IUrlService urlService, IOptionService optionService,
            ICmsConfiguration cmsConfiguration, IAccessControlService accessControlService)
        {
            this.pageService = pageService;
            this.redirectService = redirectService;
            this.tagService = tagService;
            this.sitemapService = sitemapService;
            this.urlService = urlService;
            this.optionService = optionService;
            this.cmsConfiguration = cmsConfiguration;
            this.accessControlService = accessControlService;
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

            var pageQuery = Repository
                .AsQueryable<PageProperties>(p => p.Id == request.Id)
                .FetchMany(p => p.Options)
                .Fetch(p => p.Layout).ThenFetchMany(l => l.LayoutOptions)
                .AsQueryable();
            
            if (cmsConfiguration.AccessControlEnabled)
            {
                pageQuery = pageQuery.FetchMany(f => f.AccessRules);
            }

            var page = pageQuery.ToList().FirstOne();

            Models.Redirect redirectCreated = null;
            bool initialSeoStatus = page.HasSEO;

            request.PageUrl = urlService.FixUrl(request.PageUrl);

            if (!string.Equals(page.PageUrl, request.PageUrl))
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

            page.PageUrlLowerTrimmed = page.PageUrl.LowerTrimmedUrl();
            page.Layout = Repository.AsProxy<Root.Models.Layout>(request.TemplateId);
            page.Category = request.CategoryId.HasValue ? Repository.AsProxy<CategoryEntity>(request.CategoryId.Value) : null;
            page.Title = request.PageName;
            page.CustomCss = request.PageCSS;
            page.CustomJS = request.PageJavascript;

            if (request.IsPagePublished)
            {
                page.Status = PageStatus.Published;
                page.PublishedOn = DateTime.Now;
            }
            else
            {
                page.Status = PageStatus.Unpublished;
            }

            page.UseNoFollow = request.UseNoFollow;
            page.UseNoIndex = request.UseNoIndex;
            page.UseCanonicalUrl = request.UseCanonicalUrl;
            page.IsArchived = request.IsArchived;
            page.Version = request.Version;

            page.Image = request.Image != null && request.Image.ImageId.HasValue ? Repository.AsProxy<MediaImage>(request.Image.ImageId.Value) : null;
            page.SecondaryImage = request.SecondaryImage != null && request.SecondaryImage.ImageId.HasValue ? Repository.AsProxy<MediaImage>(request.SecondaryImage.ImageId.Value) : null;
            page.FeaturedImage = request.FeaturedImage != null && request.FeaturedImage.ImageId.HasValue ? Repository.AsProxy<MediaImage>(request.FeaturedImage.ImageId.Value) : null;

            var optionValues = page.Options.Distinct();
            var parentOptions = page.Layout.LayoutOptions.Distinct();
            optionService.SaveOptionValues(request.OptionValues, optionValues, parentOptions, () => new Root.Models.PageOption { Page = page });

            if (cmsConfiguration.AccessControlEnabled)
            {
                accessControlService.UpdateAccessControl(page, request.UserAccessList != null ? request.UserAccessList.Cast<IAccessRule>().ToList() : null);
            }

            Repository.Save(page);

            // Save tags
            IList<Tag> newTags;
            tagService.SavePageTags(page, request.Tags, out newTags);

            UnitOfWork.Commit();

            // Notify about page properties change.
            Events.PageEvents.Instance.OnPagePropertiesChanged(page);

            // Notify about redirect creation.
            if (redirectCreated != null)
            {
                Events.PageEvents.Instance.OnRedirectCreated(redirectCreated);
            }

            // Notify about SEO status change.
            if (initialSeoStatus != page.HasSEO)
            {
                Events.PageEvents.Instance.OnPageSeoStatusChanged(page);
            }

            // Notify about new tags.
            Events.RootEvents.Instance.OnTagCreated(newTags);

            return new SavePageResponse(page);
        }
    }
}