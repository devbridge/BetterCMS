using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataContracts.Enums;

using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Pages.Pages.Search;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Services;

using NHibernate.Linq;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Pages
{
    public class PagesService : Service, IPagesService
    {
        private readonly IRepository repository;
        
        private readonly IOptionService optionService;

        private readonly IMediaFileUrlResolver fileUrlResolver;
        
        private readonly ISearchPagesService searchPagesService;

        public PagesService(IRepository repository, IOptionService optionService, IMediaFileUrlResolver fileUrlResolver,
            ISearchPagesService searchPagesService)
        {
            this.repository = repository;
            this.optionService = optionService;
            this.fileUrlResolver = fileUrlResolver;
            this.searchPagesService = searchPagesService;
        }

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

            query = query.ApplyPageTagsFilter(request.Data);

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
                        LayoutId = page.Layout != null && !page.Layout.IsDeleted ? page.Layout.Id : Guid.Empty,
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
            }

            if (listResponse.Items.Count > 0 && (request.Data.IncludePageOptions || request.Data.IncludeTags))
            {
                LoadOptionsAndTags(listResponse, request.Data.IncludePageOptions, request.Data.IncludeTags);
            }

            return new GetPagesResponse
            {
                Data = listResponse
            };
        }

        private void LoadOptionsAndTags(DataListResponse<PageModel> response, bool includeOptions, bool includeTags)
        {
            var pageIds = response.Items.Select(i => i.Id).Distinct().ToArray();

            IEnumerable<LayoutWithOption> layoutOptionsFuture = null;
            IEnumerable<PageWithOption> pageOptionsFuture = null;

            if (includeOptions)
            {
                var layoutIds = response.Items.Select(i => i.LayoutId).Distinct().ToArray();
                layoutOptionsFuture = repository
                    .AsQueryable<LayoutOption>(l => layoutIds.Contains(l.Layout.Id))
                    .Select(layout => new LayoutWithOption { LayoutId = layout.Layout.Id, Option = layout })
                    .ToFuture();

                pageOptionsFuture = repository
                    .AsQueryable<PageOption>(p => pageIds.Contains(p.Page.Id))
                    .Select(page => new PageWithOption { PageId = page.Page.Id, Option = page })
                    .ToFuture();
            }

            if (includeTags)
            {
                var tags = repository
                    .AsQueryable<PageTag>(pt => pageIds.Contains(pt.Page.Id))
                    .Select(pt => new { PageId = pt.Page.Id, TagName = pt.Tag.Name })
                    .OrderBy(o => o.TagName)
                    .ToFuture()
                    .ToList();

                response.Items.ToList().ForEach(page => { page.Tags = tags.Where(tag => tag.PageId == page.Id).Select(tag => tag.TagName).ToList(); });
            }

            if (includeOptions)
            {
                var layoutOptions = layoutOptionsFuture.ToList();
                var pageOptions = pageOptionsFuture.ToList();

                response.Items.ForEach(
                    page =>
                        {
                            var options = layoutOptions.Where(lo => lo.LayoutId == page.LayoutId).Select(lo => lo.Option).ToList();
                            var optionValues = pageOptions.Where(po => po.PageId == page.Id).Select(po => po.Option).ToList();

                            if (options.Count > 0 || optionValues.Count > 0)
                            {
                                page.Options =
                                    optionService.GetMergedOptionValuesForEdit(options, optionValues)
                                                 .Select(
                                                     o =>
                                                     new OptionModel
                                                         {
                                                             Key = o.OptionKey,
                                                             Value = o.OptionValue,
                                                             DefaultValue = o.OptionDefaultValue,
                                                             Type = ((Root.OptionType)(int)o.Type)
                                                         })
                                                 .ToList();
                            }
                        });
            }
        }

        private class LayoutWithOption
        {
            public Guid LayoutId { get; set; }

            public LayoutOption Option { get; set; }
        }
        
        private class PageWithOption
        {
            public Guid PageId { get; set; }

            public PageOption Option { get; set; }
        }

        SearchPagesResponse IPagesService.Search(SearchPagesRequest request)
        {
            return searchPagesService.Get(request);
        }
    }
}