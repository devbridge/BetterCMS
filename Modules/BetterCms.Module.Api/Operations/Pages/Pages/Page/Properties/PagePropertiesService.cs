using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions.Api;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Security;
using BetterCms.Core.Services;

using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Operations.Root;

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Services;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Models.Events;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Page;

using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Models.Extensions;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Helpers;
using BetterCms.Module.Root.Services;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.DataAccess.DataContext.Fetching;

using ServiceStack.ServiceInterface;

using AccessLevel = BetterCms.Module.Api.Operations.Root.AccessLevel;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties
{
    /// <summary>
    /// Default page properties CRUD service.
    /// </summary>
    public class PagePropertiesService : Service, IPagePropertiesService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The URL service.
        /// </summary>
        private readonly IUrlService urlService;

        /// <summary>
        /// The option service.
        /// </summary>
        private readonly IOptionService optionService;

        /// <summary>
        /// The file URL resolver.
        /// </summary>
        private readonly IMediaFileUrlResolver fileUrlResolver;

        /// <summary>
        /// The master page service.
        /// </summary>
        private readonly IMasterPageService masterPageService;

        /// <summary>
        /// The tag service.
        /// </summary>
        private readonly Module.Pages.Services.ITagService tagService;

        /// <summary>
        /// The access control service.
        /// </summary>
        private readonly IAccessControlService accessControlService;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The page service.
        /// </summary>
        private readonly Module.Pages.Services.IPageService pageService;

        /// <summary>
        /// The security service
        /// </summary>
        private readonly ISecurityService securityService;

        /// <summary>
        /// The category service
        /// </summary>
        private ICategoryService categoryService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PagePropertiesService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="urlService">The URL service.</param>
        /// <param name="optionService">The option service.</param>
        /// <param name="fileUrlResolver">The file URL resolver.</param>
        /// <param name="masterPageService">The master page service.</param>
        /// <param name="tagService">The tag service.</param>
        /// <param name="accessControlService">The access control service.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="pageService">The page service.</param>
        /// <param name="securityService">The security service.</param>
        public PagePropertiesService(
            IRepository repository,
            IUrlService urlService,
            IOptionService optionService,
            IMediaFileUrlResolver fileUrlResolver,
            IMasterPageService masterPageService,
            Module.Pages.Services.ITagService tagService,
            IAccessControlService accessControlService,
            IUnitOfWork unitOfWork,
            Module.Pages.Services.IPageService pageService,
            ISecurityService securityService,
            ICategoryService categoryService)
        {
            this.repository = repository;
            this.urlService = urlService;
            this.optionService = optionService;
            this.fileUrlResolver = fileUrlResolver;
            this.masterPageService = masterPageService;
            this.tagService = tagService;
            this.accessControlService = accessControlService;
            this.unitOfWork = unitOfWork;
            this.securityService = securityService;
            this.pageService = pageService;
            this.categoryService = categoryService;
        }

        /// <summary>
        /// Gets the specified page properties.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>GetPagePropertiesResponse</c> with a page properties data.
        /// </returns>
        public GetPagePropertiesResponse Get(GetPagePropertiesRequest request)
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

            var response = query
                .Select(page => new GetPagePropertiesResponse
                    {
                        Data = new PagePropertiesModel
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
                                IsArchived = page.IsArchived,
                                IsMasterPage = page.IsMasterPage,
                                LanguageGroupIdentifier = page.LanguageGroupIdentifier,
                                ForceAccessProtocol = (ForceProtocolType)(int)page.ForceAccessProtocol,
                                LanguageId = page.Language != null ? page.Language.Id : (Guid?)null,
                                MainImageId = page.Image != null && !page.Image.IsDeleted ? page.Image.Id : (Guid?)null,
                                FeaturedImageId = page.FeaturedImage != null && !page.FeaturedImage.IsDeleted ? page.FeaturedImage.Id : (Guid?)null,
                                SecondaryImageId = page.SecondaryImage != null && !page.SecondaryImage.IsDeleted ? page.SecondaryImage.Id : (Guid?)null,
                                CustomCss = page.CustomCss,
                                CustomJavaScript = page.CustomJS,
                                UseCanonicalUrl = page.UseCanonicalUrl,
                                UseNoFollow = page.UseNoFollow,
                                UseNoIndex = page.UseNoIndex
                            },
                        MetaData = request.Data.IncludeMetaData
                            ? new MetadataModel
                            {
                                MetaTitle = page.MetaTitle,
                                MetaDescription = page.MetaDescription,
                                MetaKeywords = page.MetaKeywords
                            }
                            : null,
                        Layout = request.Data.IncludeLayout && !page.Layout.IsDeleted
                            ? new LayoutModel
                            {
                                Id = page.Layout.Id,
                                Version = page.Layout.Version,
                                CreatedBy = page.Layout.CreatedByUser,
                                CreatedOn = page.Layout.CreatedOn,
                                LastModifiedBy = page.Layout.ModifiedByUser,
                                LastModifiedOn = page.Layout.ModifiedOn,

                                Name = page.Layout.Name,
                                LayoutPath = page.Layout.LayoutPath,
                                PreviewUrl = page.Layout.PreviewUrl
                            }
                            : null,
                        MainImage = page.Image != null && !page.Image.IsDeleted && request.Data.IncludeImages
                            ? new ImageModel
                            {
                                Id = page.Image.Id,
                                Version = page.Image.Version,
                                CreatedBy = page.Image.CreatedByUser,
                                CreatedOn = page.Image.CreatedOn,
                                LastModifiedBy = page.Image.ModifiedByUser,
                                LastModifiedOn = page.Image.ModifiedOn,

                                Title = page.Image.Title,
                                Caption = page.Image.Caption,
                                Url = fileUrlResolver.EnsureFullPathUrl(page.Image.PublicUrl),
                                ThumbnailUrl = fileUrlResolver.EnsureFullPathUrl(page.Image.PublicThumbnailUrl)
                            }
                            : null,
                        FeaturedImage = page.FeaturedImage != null && !page.FeaturedImage.IsDeleted && request.Data.IncludeImages
                            ? new ImageModel
                            {
                                Id = page.FeaturedImage.Id,
                                Version = page.FeaturedImage.Version,
                                CreatedBy = page.FeaturedImage.CreatedByUser,
                                CreatedOn = page.FeaturedImage.CreatedOn,
                                LastModifiedBy = page.FeaturedImage.ModifiedByUser,
                                LastModifiedOn = page.FeaturedImage.ModifiedOn,

                                Title = page.FeaturedImage.Title,
                                Caption = page.FeaturedImage.Caption,
                                Url = fileUrlResolver.EnsureFullPathUrl(page.FeaturedImage.PublicUrl),
                                ThumbnailUrl = fileUrlResolver.EnsureFullPathUrl(page.FeaturedImage.PublicThumbnailUrl)
                            }
                            : null,
                        SecondaryImage = page.SecondaryImage != null && !page.SecondaryImage.IsDeleted && request.Data.IncludeImages
                            ? new ImageModel
                            {
                                Id = page.SecondaryImage.Id,
                                Version = page.SecondaryImage.Version,
                                CreatedBy = page.SecondaryImage.CreatedByUser,
                                CreatedOn = page.SecondaryImage.CreatedOn,
                                LastModifiedBy = page.SecondaryImage.ModifiedByUser,
                                LastModifiedOn = page.SecondaryImage.ModifiedOn,

                                Title = page.SecondaryImage.Title,
                                Caption = page.SecondaryImage.Caption,
                                Url = fileUrlResolver.EnsureFullPathUrl(page.SecondaryImage.PublicUrl),
                                ThumbnailUrl = fileUrlResolver.EnsureFullPathUrl(page.SecondaryImage.PublicThumbnailUrl)
                            }
                            : null,
                        Language = page.Language != null && !page.Language.IsDeleted && request.Data.IncludeLanguage
                            ? new LanguageModel
                            {
                                Id = page.Language.Id,
                                Version = page.Language.Version,
                                CreatedBy = page.Language.CreatedByUser,
                                CreatedOn = page.Language.CreatedOn,
                                LastModifiedBy = page.Language.ModifiedByUser,
                                LastModifiedOn = page.Language.ModifiedOn,

                                Name = page.Language.Name,
                                Code = page.Language.Code,
                            }
                            : null,
                    })
                .FirstOne();

            if (request.Data.IncludeTags)
            {
                response.Tags = LoadTags(response.Data.Id);
            }

            if (request.Data.IncludePageContents)
            {
                response.PageContents = LoadPageContents(response.Data.Id);
            }

            if (request.Data.IncludePageOptions)
            {
                // Get layout options, page options and merge them
                response.PageOptions = optionService
                    .GetMergedMasterPagesOptionValues(response.Data.Id, response.Data.MasterPageId, response.Data.LayoutId)
                    .Select(o => new OptionValueModel
                        {
                            Key = o.OptionKey,
                            Value = o.OptionValue,
                            DefaultValue = o.OptionDefaultValue,
                            Type = (Root.OptionType)(int)o.Type,
                            UseDefaultValue = o.UseDefaultValue,
                            CustomTypeIdentifier = o.CustomOption != null ? o.CustomOption.Identifier : null
                        })
                    .ToList();
            }

            if (request.Data.IncludeAccessRules)
            {
                response.AccessRules = LoadAccessRules(response.Data.Id);
            }

            if (request.Data.IncludePageTranslations
                && response.Data.LanguageGroupIdentifier.HasValue)
            {
                response.PageTranslations = repository
                    .AsQueryable<PageProperties>()
                    .Where(p => p.LanguageGroupIdentifier == response.Data.LanguageGroupIdentifier)
                    .OrderBy(p => p.Title)
                    .Select(p => new PageTranslationModel
                        {
                            Id = p.Id,
                            Title = p.Title,
                            PageUrl = p.PageUrl,
                            LanguageId = p.Language != null ? p.Language.Id : (Guid?)null,
                            LanguageCode = p.Language != null ? p.Language.Code : null,
                        })
                    .ToList();
            }

            response.Data.Categories = categoryService.GetSelectedCategoriesIds<PageProperties, PageCategory>(response.Data.Id).ToList();

            if (request.Data.IncludeCategories)
            {
                response.Categories = LoadCategories(response.Data.Id);
            }

            return response;
        }

        private IList<CategoryModel> LoadCategories(Guid blogPostId)
        {
            return (from page in repository.AsQueryable<PageProperties>()
                    from category in page.Categories
                    where page.Id == blogPostId
                    select new CategoryModel
                    {
                        Id = category.Category.Id,
                        Version = category.Version,
                        CreatedBy = category.CreatedByUser,
                        CreatedOn = category.CreatedOn,
                        LastModifiedBy = category.ModifiedByUser,
                        LastModifiedOn = category.ModifiedOn,
                        Name = category.Category.Name
                    })
                     .ToList();
        }

        /// <summary>
        /// Loads the access rules.
        /// </summary>
        /// <param name="pageId">The page identifier.</param>
        /// <returns>Page access rules collection.</returns>
        private List<AccessRuleModel> LoadAccessRules(Guid pageId)
        {
            return (from page in repository.AsQueryable<Module.Root.Models.Page>()
                    from accessRule in page.AccessRules
                    where page.Id == pageId
                    orderby accessRule.IsForRole, accessRule.Identity
                    select new AccessRuleModel
                    {
                        AccessLevel = (AccessLevel)(int)accessRule.AccessLevel,
                        Identity = accessRule.Identity,
                        IsForRole = accessRule.IsForRole
                    })
                    .ToList();
        }

        /// <summary>
        /// Loads the tags.
        /// </summary>
        /// <param name="pageId">The page identifier.</param>
        /// <returns>Page tags collection.</returns>
        private List<TagModel> LoadTags(Guid pageId)
        {
            return repository
                .AsQueryable<PageTag>(pageTag => pageTag.Page.Id == pageId && !pageTag.Tag.IsDeleted)
                .Select(media => new TagModel
                {
                    Id = media.Tag.Id,
                    Version = media.Tag.Version,
                    CreatedBy = media.Tag.CreatedByUser,
                    CreatedOn = media.Tag.CreatedOn,
                    LastModifiedBy = media.Tag.ModifiedByUser,
                    LastModifiedOn = media.Tag.ModifiedOn,

                    Name = media.Tag.Name
                }).ToList();
        }

        /// <summary>
        /// Loads the page contents.
        /// </summary>
        /// <param name="pageId">The page identifier.</param>
        /// <returns>Page contents collection.</returns>
        private List<PageContentModel> LoadPageContents(Guid pageId)
        {
            var results = repository
                 .AsQueryable<PageContent>(pageContent => pageContent.Page.Id == pageId && !pageContent.Content.IsDeleted)
                 .OrderBy(pageContent => pageContent.Order)
                 .Select(pageContent => new
                    {
                        Type = pageContent.Content.GetType(),
                        Model = new PageContentModel
                            {
                                Id = pageContent.Id,
                                Version = pageContent.Version,
                                CreatedBy = pageContent.CreatedByUser,
                                CreatedOn = pageContent.CreatedOn,
                                LastModifiedBy = pageContent.ModifiedByUser,
                                LastModifiedOn = pageContent.ModifiedOn,

                                ContentId = pageContent.Content.Id,
                                ParentPageContentId = pageContent.Parent != null ? pageContent.Parent.Id : (Guid?)null,
                                Name = pageContent.Content.Name,
                                RegionId = pageContent.Region.Id,
                                RegionIdentifier = pageContent.Region.RegionIdentifier,
                                Order = pageContent.Order,
                                IsPublished = pageContent.Content.Status == ContentStatus.Published
                            }
                    }).ToList();

            // Set content types
            results.ToList().ForEach(item => item.Model.ContentType = item.Type.ToContentTypeString());

            return results.Select(item => item.Model).ToList();
        }

        /// <summary>
        /// Posts the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PostPageResponse</c> with page data.</returns>
        public PostPagePropertiesResponse Post(PostPagePropertiesRequest request)
        {
            var result = Put(
                    new PutPagePropertiesRequest
                    {
                        Data = request.Data,
                        User = request.User
                    });

            return new PostPagePropertiesResponse { Data = result.Data };
        }

        /// <summary>
        /// Puts the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PutPageResponse</c> with created or updated item id.</returns>
        public PutPagePropertiesResponse Put(PutPagePropertiesRequest request)
        {
            if (request.Data.IsMasterPage)
            {
                accessControlService.DemandAccess(securityService.GetCurrentPrincipal(), RootModuleConstants.UserRoles.Administration);
            }
            else
            {
                accessControlService.DemandAccess(securityService.GetCurrentPrincipal(), RootModuleConstants.UserRoles.EditContent);
            }

            PageProperties pageProperties = null;

            var isNew = !request.Id.HasValue || request.Id.Value.HasDefaultValue();

            if (!isNew)
            {
                pageProperties =
                    repository.AsQueryable<PageProperties>(e => e.Id == request.Id.GetValueOrDefault())
                        .FetchMany(p => p.Options)
                        .Fetch(p => p.Layout)
                        .ThenFetchMany(l => l.LayoutOptions)
                        .FetchMany(p => p.MasterPages)
                        .FetchMany(f => f.AccessRules)
                        .ToList()
                        .FirstOrDefault();

                isNew = pageProperties == null;
            }
            UpdatingPagePropertiesModel beforeChange = null;
            if (isNew)
            {
                pageProperties = new PageProperties
                                     {
                                         Id = request.Id.GetValueOrDefault(),
                                         Status = PageStatus.Unpublished,
                                         AccessRules = new List<AccessRule>()
                                     };
            }
            else if (request.Data.Version > 0)
            {
                pageProperties.Version = request.Data.Version;
            }

            if (!isNew)
            {
                beforeChange = new UpdatingPagePropertiesModel(pageProperties);
            }

            if (!isNew && pageProperties.IsMasterPage != request.Data.IsMasterPage)
            {
                const string message = "IsMasterPage cannot be changed for updating page. It can be modified only when creating a page.";
                var logMessage = string.Format("{0} PageId: {1}", message, request.Id);
                throw new ValidationException(() => message, logMessage);
            }

            // Load master pages for updating page's master path and page's children master path
            IList<Guid> newMasterIds;
            IList<Guid> oldMasterIds;
            IList<Guid> childrenPageIds;
            IList<MasterPage> existingChildrenMasterPages;
            masterPageService.PrepareForUpdateChildrenMasterPages(pageProperties, request.Data.MasterPageId, out newMasterIds, out oldMasterIds, out childrenPageIds, out existingChildrenMasterPages);

            unitOfWork.BeginTransaction();

            if (!string.IsNullOrEmpty(request.Data.PageUrl) || string.IsNullOrEmpty(pageProperties.PageUrl))
            {
                var pageUrl = request.Data.PageUrl;
                if (string.IsNullOrEmpty(pageUrl) && !string.IsNullOrWhiteSpace(request.Data.Title))
                {
                    pageUrl = pageService.CreatePagePermalink(request.Data.Title, null, null, request.Data.LanguageId, request.Data.Categories);
                }
                else
                {
                    pageUrl = urlService.FixUrl(pageUrl);
                    pageService.ValidatePageUrl(pageUrl, request.Id);
                }

                pageProperties.PageUrl = pageUrl;
                pageProperties.PageUrlHash = pageUrl.UrlHash();
            }

            pageProperties.Title = request.Data.Title;
            pageProperties.Description = request.Data.Description;

            var newStatus = request.Data.IsMasterPage || request.Data.IsPublished ? PageStatus.Published : PageStatus.Unpublished;
            if (!request.Data.IsMasterPage && pageProperties.Status != newStatus)
            {
                accessControlService.DemandAccess(securityService.GetCurrentPrincipal(), RootModuleConstants.UserRoles.PublishContent);
            }

            pageProperties.Status = newStatus;
            pageProperties.PublishedOn = request.Data.IsPublished && !request.Data.PublishedOn.HasValue ? DateTime.Now : request.Data.PublishedOn;

            masterPageService.SetMasterOrLayout(pageProperties, request.Data.MasterPageId, request.Data.LayoutId);

            categoryService.CombineEntityCategories<PageProperties, PageCategory>(pageProperties, request.Data.Categories);

            pageProperties.IsArchived = request.Data.IsArchived;
            pageProperties.IsMasterPage = request.Data.IsMasterPage;
            pageProperties.LanguageGroupIdentifier = request.Data.LanguageGroupIdentifier;
            pageProperties.ForceAccessProtocol = (Core.DataContracts.Enums.ForceProtocolType)(int)request.Data.ForceAccessProtocol;
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
                tagService.SavePageTags(pageProperties, request.Data.Tags, out newTags);
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

            if (request.Data.PageOptions != null)
            {
                var options = request.Data.PageOptions.ToServiceModel();

                var pageOptions = pageProperties.Options != null ? pageProperties.Options.Distinct() : null;
                pageProperties.Options = optionService.SaveOptionValues(options, pageOptions, () => new PageOption { Page = pageProperties });
            }

            if (!isNew)
            {
                // Notify about page properties changing.
                var cancelEventArgs = Events.PageEvents.Instance.OnPagePropertiesChanging(beforeChange, new UpdatingPagePropertiesModel(pageProperties));
                if (cancelEventArgs.Cancel)
                {
                    throw new CmsApiValidationException(
                        cancelEventArgs.CancellationErrorMessages != null && cancelEventArgs.CancellationErrorMessages.Count > 0
                            ? string.Join(",", cancelEventArgs.CancellationErrorMessages)
                            : "Page properties saving was canceled.");
                }
            }

            repository.Save(pageProperties);

            //
            // If creating new page, page id is unknown when children pages are loaded, so Guid may be empty
            // Updating id to saved page's Id manually
            //
            if (isNew && childrenPageIds != null && childrenPageIds.Count == 1 && childrenPageIds[0].HasDefaultValue())
            {
                childrenPageIds[0] = pageProperties.Id;
            }

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

            return new PutPagePropertiesResponse { Data = pageProperties.Id };
        }

        /// <summary>
        /// Deletes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>DeletePageResponse</c> with success status.</returns>
        public DeletePagePropertiesResponse Delete(DeletePagePropertiesRequest request)
        {
            var model = new DeletePageViewModel
                    {
                        PageId = request.Id,
                        Version = request.Data.Version
                    };

            var result = pageService.DeletePage(model, securityService.GetCurrentPrincipal());

            return new DeletePagePropertiesResponse { Data = result };
        }
    }
}