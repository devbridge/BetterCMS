using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataAccess.DataContext.Fetching;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions.Api;
using BetterCms.Core.Exceptions.DataTier;
using BetterCms.Core.Security;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents.Content;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Exists;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Translations;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Models.Extensions;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Helpers;

using Microsoft.SqlServer.Server;

using ServiceStack.ServiceInterface;

using ITagService = BetterCms.Module.Pages.Services.ITagService;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page
{
    /// <summary>
    /// Default page service for pages API.
    /// </summary>
    public class PageService : Service, IPageService
    {
        /// <summary>
        /// The page properties service.
        /// </summary>
        private readonly IPagePropertiesService pagePropertiesService;

        /// <summary>
        /// The page exists service.
        /// </summary>
        private readonly IPageExistsService pageExistsService;

        /// <summary>
        /// The page contents service.
        /// </summary>
        private readonly IPageContentsService pageContentsService;

        /// <summary>
        /// The page content service.
        /// </summary>
        private readonly IPageContentService pageContentService;

        /// <summary>
        /// The page translations service.
        /// </summary>
        private readonly IPageTranslationsService pageTranslationsService;

        /// <summary>
        /// The master page service.
        /// </summary>
        private readonly IMasterPageService masterPageService;

        /// <summary>
        /// The CMS configuration.
        /// </summary>
        private readonly ICmsConfiguration cmsConfiguration;

        /// <summary>
        /// The page service.
        /// </summary>
        private readonly Module.Pages.Services.IPageService pageService;

        /// <summary>
        /// The sitemap service.
        /// </summary>
        private readonly ISitemapService sitemapService;

        /// <summary>
        /// The tag service.
        /// </summary>
        private readonly ITagService tagService;

        /// <summary>
        /// The access control service.
        /// </summary>
        private readonly IAccessControlService accessControlService;

        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The URL service.
        /// </summary>
        private readonly IUrlService urlService;

        /// <summary>
        /// The file URL resolver.
        /// </summary>
        private readonly IMediaFileUrlResolver fileUrlResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="PageService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="pagePropertiesService">The page properties service.</param>
        /// <param name="pageExistsService">The page exists service.</param>
        /// <param name="pageContentsService">The page contents service.</param>
        /// <param name="pageContentService">The page content service.</param>
        /// <param name="urlService">The URL service.</param>
        /// <param name="fileUrlResolver">The file URL resolver.</param>
        /// <param name="pageTranslationsService">The page translations service.</param>
        /// <param name="masterPageService">The master page service.</param>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        /// <param name="pageService">The page service.</param>
        /// <param name="sitemapService">The sitemap service.</param>
        /// <param name="tagService">The tag service.</param>
        /// <param name="accessControlService">The access control service.</param>
        public PageService(
            IRepository repository,
            IUnitOfWork unitOfWork,
            IPagePropertiesService pagePropertiesService,
            IPageExistsService pageExistsService,
            IPageContentsService pageContentsService,
            IPageContentService pageContentService,
            IUrlService urlService,
            IMediaFileUrlResolver fileUrlResolver,
            IPageTranslationsService pageTranslationsService,
            IMasterPageService masterPageService,
            ICmsConfiguration cmsConfiguration,
            Module.Pages.Services.IPageService pageService,
            ISitemapService sitemapService,
            ITagService tagService,
            IAccessControlService accessControlService)
        {
            this.pageContentsService = pageContentsService;
            this.pageContentService = pageContentService;
            this.pagePropertiesService = pagePropertiesService;
            this.pageExistsService = pageExistsService;
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.urlService = urlService;
            this.fileUrlResolver = fileUrlResolver;
            this.pageTranslationsService = pageTranslationsService;
            this.masterPageService = masterPageService;
            this.cmsConfiguration = cmsConfiguration;
            this.pageService = pageService;
            this.sitemapService = sitemapService;
            this.tagService = tagService;
            this.accessControlService = accessControlService;
        }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        IPagePropertiesService IPageService.Properties
        {
            get
            {
                return pagePropertiesService;
            }
        }

        /// <summary>
        /// Gets the contents.
        /// </summary>
        /// <value>
        /// The contents.
        /// </value>
        IPageContentsService IPageService.Contents
        {
            get
            {
                return pageContentsService;
            }
        }

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        IPageContentService IPageService.Content
        {
            get
            {
                return pageContentService;
            }
        }

        /// <summary>
        /// Gets the translations.
        /// </summary>
        /// <value>
        /// The translations.
        /// </value>
        IPageTranslationsService IPageService.Translations
        {
            get
            {
                return pageTranslationsService;
            }
        }

        /// <summary>
        /// Gets the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetPageResponse</c> with page properties.</returns>
        public GetPageResponse Get(GetPageRequest request)
        {
            var query = repository.AsQueryable<PageProperties>();
            
            if (request.PageId.HasValue)
            {
                query = query.Where(page => page.Id == request.PageId.Value);
            }
            else
            {
                var url = urlService.FixUrl(request.PageUrl);
                query = query.Where(page => page.PageUrlHash == url.UrlHash());
            }

            var model = query
                .Select(page => new PageModel
                    {
                        Id = page.Id,
                        Version = page.Version,
                        CreatedBy = page.CreatedByUser,
                        CreatedOn = page.CreatedOn,
                        LastModifiedBy = page.ModifiedByUser,
                        LastModifiedOn = page.ModifiedOn,

                        PageUrl = page.PageUrl,
                        Title = page.Title,
                        Description = page.Description,
                        IsPublished = page.Status == PageStatus.Published,
                        PublishedOn = page.PublishedOn,
                        LayoutId = page.Layout != null && !page.Layout.IsDeleted ? page.Layout.Id : (Guid?)null,
                        MasterPageId = page.MasterPage != null && !page.MasterPage.IsDeleted ? page.MasterPage.Id : (Guid?)null,
                        CategoryId = page.Category != null && !page.Category.IsDeleted ? page.Category.Id : (Guid?)null,
                        CategoryName = page.Category != null && !page.Category.IsDeleted ? page.Category.Name : null,
                        MainImageId = page.Image != null && !page.Image.IsDeleted ? page.Image.Id : (Guid?)null,
                        MainImageUrl = page.Image != null && !page.Image.IsDeleted ? page.Image.PublicUrl : null,
                        MainImageThumbnauilUrl = page.Image != null && !page.Image.IsDeleted ? page.Image.PublicThumbnailUrl : null,
                        MainImageCaption = page.Image != null && !page.Image.IsDeleted ? page.Image.Caption : null,
                        IsArchived = page.IsArchived,
                        IsMasterPage = page.IsMasterPage,
                        LanguageId = page.Language != null ? page.Language.Id : (Guid?)null,
                        LanguageCode = page.Language != null ? page.Language.Code : null,
                        LanguageGroupIdentifier = page.LanguageGroupIdentifier
                    })
                .FirstOne();

            model.MainImageUrl = fileUrlResolver.EnsureFullPathUrl(model.MainImageUrl);
            model.MainImageThumbnauilUrl = fileUrlResolver.EnsureFullPathUrl(model.MainImageThumbnauilUrl);

            return new GetPageResponse { Data = model };
        }

        /// <summary>
        /// Puts the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PutPageResponse</c> with created or updated item id.</returns>
        public PutPageResponse Put(PutPageRequest request)
        {
            var pageProperties =
                repository.AsQueryable<PageProperties>(e => e.Id == request.PageId.GetValueOrDefault())
                    .FetchMany(p => p.Options)
                    .Fetch(p => p.Layout)
                    .ThenFetchMany(l => l.LayoutOptions)
                    .FetchMany(p => p.MasterPages)
                    .FetchMany(f => f.AccessRules)
                    .ToList()
                    .FirstOrDefault();

            var isNew = pageProperties == null;
            if (isNew)
            {
                pageProperties = new PageProperties { Id = request.PageId.GetValueOrDefault(), AccessRules = new List<AccessRule>() };
            }
            else if (request.Data.Version > 0)
            {
                pageProperties.Version = request.Data.Version;
            }

            // Load master pages for updating page's master path and page's children master path
            IList<Guid> newMasterIds;
            IList<Guid> oldMasterIds;
            IList<Guid> childrenPageIds;
            IList<MasterPage> existingChildrenMasterPages;
            masterPageService.PrepareForUpdateChildrenMasterPages(pageProperties, request.Data.MasterPageId, out newMasterIds, out oldMasterIds, out childrenPageIds, out existingChildrenMasterPages);

            unitOfWork.BeginTransaction();

            var pageUrl = request.Data.PageUrl;
            if (string.IsNullOrEmpty(pageUrl) && !string.IsNullOrWhiteSpace(request.Data.Title))
            {
                pageUrl = pageService.CreatePagePermalink(request.Data.Title, null);
            }
            else
            {
                pageUrl = urlService.FixUrl(pageUrl);
                pageService.ValidatePageUrl(pageUrl, request.PageId);
            }

            pageProperties.PageUrl = pageUrl;
            pageProperties.PageUrlHash = pageUrl.UrlHash();
            pageProperties.Title = request.Data.Title;
            pageProperties.Description = request.Data.Description;
            pageProperties.Status = request.Data.IsMasterPage || request.Data.IsPublished ? PageStatus.Published : PageStatus.Unpublished;
            pageProperties.PublishedOn = request.Data.PublishedOn;

            masterPageService.SetMasterOrLayout(pageProperties, request.Data.MasterPageId, request.Data.LayoutId);

            pageProperties.Category = request.Data.CategoryId.HasValue
                                    ? repository.AsProxy<Category>(request.Data.CategoryId.Value)
                                    : null;
            pageProperties.IsArchived = request.Data.IsArchived;
            pageProperties.IsMasterPage = request.Data.IsMasterPage;
            pageProperties.LanguageGroupIdentifier = request.Data.LanguageGroupIdentifier;
            pageProperties.Language = request.Data.LanguageId.HasValue && !request.Data.LanguageId.Value.HasDefaultValue()
                                    ? repository.AsProxy<Language>(request.Data.LanguageId.Value)
                                    : null;

            pageProperties.Image = request.Data.MainImageId.HasValue
                                    ? repository.AsProxy<MediaImage>(request.Data.MainImageId.Value)
                                    : null;
            pageProperties.FeaturedImage = request.Data.FeaturedImageId.HasValue
                                    ? repository.AsProxy<MediaImage>(request.Data.FeaturedImageId.Value)
                                    : null;
            pageProperties.SecondaryImage = request.Data.SecondaryImageId.HasValue
                                    ? repository.AsProxy<MediaImage>(request.Data.SecondaryImageId.Value)
                                    : null;

            pageProperties.CustomCss = request.Data.CustomCss;
            pageProperties.CustomJS = request.Data.CustomJavaScript;
            pageProperties.UseCanonicalUrl = request.Data.UseCanonicalUrl;
            pageProperties.UseNoFollow = request.Data.UseNoFollow;
            pageProperties.UseNoIndex = request.Data.UseNoIndex;

            if (request.Data.MetaData != null)
            {
                pageProperties.MetaTitle = request.Data.MetaData.MetaTitle;
                pageProperties.MetaDescription = request.Data.MetaData.MetaDescription;
                pageProperties.MetaKeywords = request.Data.MetaData.MetaKeywords;
            }

            IList<Tag> newTags = null;
            if (request.Data.Tags != null)
            {
                var tags = request.Data.Tags.Select(t => t.Name).ToList();
                tagService.SavePageTags(pageProperties, tags, out newTags);
            }

            if (request.Data.AccessRules != null)
            {
                pageProperties.AccessRules.RemoveDuplicateEntities();
                var accessRules =
                    request.Data.AccessRules.Select(
                        r => (IAccessRule)new AccessRule { AccessLevel = (Core.Security.AccessLevel)(int)r.AccessLevel, Identity = r.Identity, IsForRole = r.IsForRole })
                        .ToList();
                accessControlService.UpdateAccessControl(pageProperties, accessRules);
            }

            // TODO: save options.
            // page.Options = optionService.SaveOptionValues(request.OptionValues, null, () => new PageOption { Page = page });

            repository.Save(pageProperties);

            masterPageService.UpdateChildrenMasterPages(existingChildrenMasterPages, oldMasterIds, newMasterIds, childrenPageIds);

            unitOfWork.Commit();

            // Fire events.
            Events.RootEvents.Instance.OnTagCreated(newTags);
            if (isNew)
            {
                Events.PageEvents.Instance.OnPageCreated(pageProperties);
            }
            else
            {
                Events.PageEvents.Instance.OnPagePropertiesChanged(pageProperties);
            }

            return new PutPageResponse { Data = Get(new GetPageRequest { PageId = request.PageId }).Data };
        }

        /// <summary>
        /// Deletes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>DeletePageResponse</c> with success status.</returns>
        public DeletePageResponse Delete(DeletePageRequest request)
        {
            if (request.Data == null || request.Data.Id.HasDefaultValue())
            {
                return new DeletePageResponse { Data = false };
            }

            var page = repository.First<PageProperties>(request.Data.Id);
            if (request.Data.Version > 0 && page.Version != request.Data.Version)
            {
                throw new ConcurrentDataException(page);
            }

            if (page.IsMasterPage && repository.AsQueryable<Module.Root.Models.MasterPage>(mp => mp.Master == page).Any())
            {
                var logMessage = string.Format("Failed to delete page. Page is selected as master page. Id: {0} Url: {1}", page.Id, page.PageUrl);
                throw new CmsApiValidationException(logMessage);
            }

            var sitemaps = new Dictionary<Module.Pages.Models.Sitemap, bool>();
            var sitemapNodes = sitemapService.GetNodesByPage(page);

            unitOfWork.BeginTransaction();

            IList<SitemapNode> updatedNodes = new List<SitemapNode>();
            IList<SitemapNode> deletedNodes = new List<SitemapNode>();
            if (sitemapNodes != null)
            {
                // Archive sitemaps before update.
                sitemaps.Select(pair => pair.Key).ToList().ForEach(sitemap => sitemapService.ArchiveSitemap(sitemap.Id));
                foreach (var node in sitemapNodes)
                {
                    if (!node.IsDeleted)
                    {
                        // Unlink sitemap node.
                        if (node.Page != null && node.Page.Id == page.Id)
                        {
                            node.Page = null;
                            node.Title = node.UsePageTitleAsNodeTitle ? page.Title : node.Title;
                            node.Url = page.PageUrl;
                            node.UrlHash = page.PageUrlHash;
                            repository.Save(node);
                            updatedNodes.Add(node);
                        }
                    }
                }
            }

            // Delete child entities.            
            if (page.PageTags != null)
            {
                foreach (var pageTag in page.PageTags)
                {
                    repository.Delete(pageTag);
                }
            }

            if (page.PageContents != null)
            {
                foreach (var pageContent in page.PageContents)
                {
                    repository.Delete(pageContent);
                }
            }

            if (page.Options != null)
            {
                foreach (var option in page.Options)
                {
                    repository.Delete(option);
                }
            }

            if (page.AccessRules != null)
            {
                var rules = page.AccessRules.ToList();
                rules.ForEach(page.RemoveRule);
            }

            if (page.MasterPages != null)
            {
                foreach (var master in page.MasterPages)
                {
                    repository.Delete(master);
                }
            }

            // Delete page
            repository.Delete<Module.Root.Models.Page>(request.Data.Id, request.Data.Version);

            unitOfWork.Commit();

            var updatedSitemaps = new List<Module.Pages.Models.Sitemap>();
            foreach (var node in updatedNodes)
            {
                Events.SitemapEvents.Instance.OnSitemapNodeUpdated(node);
                if (!updatedSitemaps.Contains(node.Sitemap))
                {
                    updatedSitemaps.Add(node.Sitemap);
                }
            }

            foreach (var node in deletedNodes)
            {
                Events.SitemapEvents.Instance.OnSitemapNodeDeleted(node);
                if (!updatedSitemaps.Contains(node.Sitemap))
                {
                    updatedSitemaps.Add(node.Sitemap);
                }
            }

            foreach (var updatedSitemap in updatedSitemaps)
            {
                Events.SitemapEvents.Instance.OnSitemapUpdated(updatedSitemap);
            }

            Events.PageEvents.Instance.OnPageDeleted(page);

            return new DeletePageResponse { Data = true };
        }

        /// <summary>
        /// Exists the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PageExistsResponse</c> with page data.</returns>
        PageExistsResponse IPageService.Exists(PageExistsRequest request)
        {
            return pageExistsService.Get(request);
        }
    }
}