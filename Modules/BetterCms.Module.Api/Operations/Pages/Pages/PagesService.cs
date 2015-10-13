using System;
using System.Collections.Generic;
using System.Linq;

using BetterModules.Core.DataAccess;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Security;

using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Pages.Pages.Page;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties;
using BetterCms.Module.Api.Operations.Pages.Pages.Search;
using BetterCms.Module.Api.Operations.Root;

using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Services;

using NHibernate.Linq;

using ServiceStack.ServiceInterface;

using AccessLevel = BetterCms.Module.Api.Operations.Root.AccessLevel;

namespace BetterCms.Module.Api.Operations.Pages.Pages
{
    /// <summary>
    /// Default pages service for CRUD.
    /// </summary>
    public class PagesService : Service, IPagesService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The option service.
        /// </summary>
        private readonly IOptionService optionService;

        /// <summary>
        /// The file URL resolver.
        /// </summary>
        private readonly IMediaFileUrlResolver fileUrlResolver;

        /// <summary>
        /// The search pages service.
        /// </summary>
        private readonly ISearchPagesService searchPagesService;

        /// <summary>
        /// The access control service.
        /// </summary>
        private readonly IAccessControlService accessControlService;

        /// <summary>
        /// The page service.
        /// </summary>
        private readonly IPageService pageService;

        private readonly ICategoryService categoryService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PagesService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="optionService">The option service.</param>
        /// <param name="fileUrlResolver">The file URL resolver.</param>
        /// <param name="searchPagesService">The search pages service.</param>
        /// <param name="accessControlService">The access control service.</param>
        /// <param name="pageService">The page service.</param>
        public PagesService(
            IRepository repository,
            IOptionService optionService,
            IMediaFileUrlResolver fileUrlResolver,
            ISearchPagesService searchPagesService,
            IAccessControlService accessControlService,
            IPageService pageService,
            ICategoryService categoryService)
        {
            this.repository = repository;
            this.optionService = optionService;
            this.fileUrlResolver = fileUrlResolver;
            this.searchPagesService = searchPagesService;
            this.accessControlService = accessControlService;
            this.pageService = pageService;
            this.categoryService = categoryService;
        }

        /// <summary>
        /// Gets the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetPagesResponse</c> with page list.</returns>
        public GetPagesResponse Get(GetPagesRequest request)
        {
            request.Data.SetDefaultOrder("Title");

            var query = repository
                .AsQueryable<PageProperties>();

            if (!request.Data.IncludeArchived)
            {
                query = query.Where(b => !b.IsArchived);
            }

            if (!request.Data.IncludeUnpublished)
            {
                query = query.Where(b => b.Status == PageStatus.Published);
            }

            if (!request.Data.IncludeMasterPages)
            {
                query = query.Where(b => !b.IsMasterPage);
            }

            query = query.ApplyPageTagsFilter(request.Data)
                            .ApplyCategoriesFilter(categoryService, request.Data);

            if (request.User != null && !string.IsNullOrWhiteSpace(request.User.Name))
            {
                var principal = new ApiPrincipal(request.User);
                IEnumerable<Guid> deniedPages = accessControlService.GetPrincipalDeniedObjects<PageProperties>(principal, false);
                foreach (var deniedPageId in deniedPages)
                {
                    var id = deniedPageId;
                    query = query.Where(f => f.Id != id);
                }
            }

            var includeMetadata = request.Data.IncludeMetadata;
            var listResponse = query
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
                        MainImageId = page.Image != null && !page.Image.IsDeleted ? page.Image.Id : (Guid?)null,
                        MainImageUrl = page.Image != null && !page.Image.IsDeleted ? page.Image.PublicUrl : null,
                        MainImageThumbnauilUrl = page.Image != null && !page.Image.IsDeleted ? page.Image.PublicThumbnailUrl : null,
                        MainImageThumbnailUrl = page.Image != null && !page.Image.IsDeleted ? page.Image.PublicThumbnailUrl : null,
                        MainImageCaption = page.Image != null && !page.Image.IsDeleted ? page.Image.Caption : null,
                        SecondaryImageId = page.SecondaryImage != null && !page.SecondaryImage.IsDeleted ? page.SecondaryImage.Id : (Guid?)null,
                        SecondaryImageUrl = page.SecondaryImage != null && !page.SecondaryImage.IsDeleted ? page.SecondaryImage.PublicUrl : null,
                        SecondaryImageThumbnailUrl = page.SecondaryImage != null && !page.SecondaryImage.IsDeleted ? page.SecondaryImage.PublicThumbnailUrl : null,
                        SecondaryImageCaption = page.SecondaryImage != null && !page.SecondaryImage.IsDeleted ? page.SecondaryImage.Caption : null,
                        FeaturedImageId = page.FeaturedImage != null && !page.FeaturedImage.IsDeleted ? page.FeaturedImage.Id : (Guid?)null,
                        FeaturedImageUrl = page.FeaturedImage != null && !page.FeaturedImage.IsDeleted ? page.FeaturedImage.PublicUrl : null,
                        FeaturedImageThumbnailUrl = page.FeaturedImage != null && !page.FeaturedImage.IsDeleted ? page.FeaturedImage.PublicThumbnailUrl : null,
                        FeaturedImageCaption = page.FeaturedImage != null && !page.FeaturedImage.IsDeleted ? page.FeaturedImage.Caption : null,
                        IsArchived = page.IsArchived,
                        IsMasterPage = page.IsMasterPage,
                        LanguageId = page.Language != null ? page.Language.Id : (Guid?)null,
                        LanguageCode = page.Language != null ? page.Language.Code : null,
                        LanguageGroupIdentifier = page.LanguageGroupIdentifier,
                        Metadata = includeMetadata
                            ? new MetadataModel
                                  {
                                      MetaDescription = page.MetaDescription,
                                      MetaTitle = page.MetaTitle,
                                      MetaKeywords = page.MetaKeywords,
                                      UseNoFollow = page.UseNoFollow,
                                      UseNoIndex = page.UseNoIndex,
                                      UseCanonicalUrl = page.UseCanonicalUrl
                                  } : null
                    }).ToDataListResponse(request);

            foreach (var model in listResponse.Items)
            {
                model.MainImageUrl = fileUrlResolver.EnsureFullPathUrl(model.MainImageUrl);
                model.MainImageThumbnauilUrl = fileUrlResolver.EnsureFullPathUrl(model.MainImageThumbnauilUrl);
                model.MainImageThumbnailUrl = fileUrlResolver.EnsureFullPathUrl(model.MainImageThumbnailUrl);

                model.SecondaryImageUrl = fileUrlResolver.EnsureFullPathUrl(model.SecondaryImageUrl);
                model.SecondaryImageThumbnailUrl = fileUrlResolver.EnsureFullPathUrl(model.SecondaryImageThumbnailUrl);

                model.FeaturedImageUrl = fileUrlResolver.EnsureFullPathUrl(model.FeaturedImageUrl);
                model.FeaturedImageThumbnailUrl = fileUrlResolver.EnsureFullPathUrl(model.FeaturedImageThumbnailUrl);
            }

            if (listResponse.Items.Count > 0
                && (request.Data.IncludePageOptions || request.Data.IncludeTags || request.Data.IncludeAccessRules || request.Data.IncludeCategories))
            {
                LoadInnerCollections(listResponse, request.Data.IncludePageOptions, request.Data.IncludeTags, request.Data.IncludeAccessRules, request.Data.IncludeCategories);
            }

            return new GetPagesResponse
            {
                Data = listResponse
            };
        }

        private void LoadInnerCollections(DataListResponse<PageModel> response, bool includeOptions, bool includeTags, bool includeAccessRules, bool includeCategories)
        {
            var pageIds = response.Items.Select(i => i.Id).Distinct().ToArray();

            IEnumerable<TagModel> tagsFuture;
            if (includeTags)
            {
                tagsFuture = repository.AsQueryable<PageTag>(pt => pageIds.Contains(pt.Page.Id))
                        .Select(pt => new TagModel { PageId = pt.Page.Id, Tag = pt.Tag.Name })
                        .OrderBy(o => o.Tag)
                        .ToFuture();
            }
            else
            {
                tagsFuture = null;
            }

            IEnumerable<AccessRuleModelEx> rulesFuture;
            if (includeAccessRules)
            {
                rulesFuture = (from page in repository.AsQueryable<Module.Root.Models.Page>()
                               from accessRule in page.AccessRules
                               where pageIds.Contains(page.Id)
                               orderby accessRule.IsForRole, accessRule.Identity
                               select new AccessRuleModelEx
                                      {
                                          AccessRule = new AccessRuleModel
                                          {
                                              AccessLevel = (AccessLevel)(int)accessRule.AccessLevel,
                                              Identity = accessRule.Identity,
                                              IsForRole = accessRule.IsForRole
                                          },
                                          PageId = page.Id
                                      })
                    .ToFuture();
            }
            else
            {
                rulesFuture = null;
            }

            if (tagsFuture != null)
            {
                var tags = tagsFuture.ToList();
                response.Items.ToList().ForEach(page =>
                {
                    page.Tags = tags
                        .Where(tag => tag.PageId == page.Id)
                        .Select(tag => tag.Tag)
                        .ToList();
                });
            }

            if (rulesFuture != null)
            {
                var rules = rulesFuture.ToList();
                response.Items.ToList().ForEach(page =>
                {
                    page.AccessRules = rules
                        .Where(rule => rule.PageId == page.Id)
                        .Select(rule => rule.AccessRule)
                        .ToList();
                });
            }

            if (includeOptions)
            {
                response.Items.ForEach(
                    page =>
                    {
                        page.Options = optionService
                            .GetMergedMasterPagesOptionValues(page.Id, page.MasterPageId, page.LayoutId)
                            .Select(o => new OptionValueModel
                                {
                                    Key = o.OptionKey,
                                    Value = o.OptionValue,
                                    DefaultValue = o.OptionDefaultValue,
                                    Type = ((Root.OptionType)(int)o.Type),
                                    UseDefaultValue = o.UseDefaultValue,
                                    CustomTypeIdentifier = o.CustomOption != null ? o.CustomOption.Identifier : null
                                })
                            .ToList();
                    });
            }

            if (includeCategories)
            {
                response.Items.ForEach( page =>
                                        {
                                            page.Categories = (from pagePr in repository.AsQueryable<PageProperties>()
                                                               from category in pagePr.Categories
                                                               where pagePr.Id == page.Id && !category.IsDeleted
                                                               select new CategoryModel
                                                                {
                                                                    Id = category.Category.Id,
                                                                    Version = category.Version,
                                                                    CreatedBy = category.CreatedByUser,
                                                                    CreatedOn = category.CreatedOn,
                                                                    LastModifiedBy = category.ModifiedByUser,
                                                                    LastModifiedOn = category.ModifiedOn,
                                                                    Name = category.Category.Name
                                                                }).ToList();
                                        });
            }

        }

        private class TagModel
        {
            public Guid PageId { get; set; }

            public string Tag { get; set; }
        }

        private class AccessRuleModelEx
        {
            public AccessRuleModel AccessRule { get; set; }
            public Guid PageId { get; set; }
        }

        SearchPagesResponse IPagesService.Search(SearchPagesRequest request)
        {
            return searchPagesService.Get(request);
        }
    }
}