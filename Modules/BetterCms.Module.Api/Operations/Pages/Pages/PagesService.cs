using System;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext.Fetching;
using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;

using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
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

        public PagesService(IRepository repository, IOptionService optionService)
        {
            this.repository = repository;
            this.optionService = optionService;
        }

        public GetPagesResponse Get(GetPagesRequest request)
        {
            request.Data.SetDefaultOrder("Title");

            var query = repository
                .AsQueryable<Module.Pages.Models.PageProperties>();

            if (!request.Data.IncludeArchived)
            {
                query = query.Where(b => !b.IsArchived);
            }

            if (!request.Data.IncludeUnpublished)
            {
                query = query.Where(b => b.Status == PageStatus.Published);
            }

            query = query.ApplyTagsFilter(
                request.Data,
                tagName => { return page => page.PageTags.Any(pageTag => pageTag.Tag.Name == tagName && !pageTag.IsDeleted && !pageTag.Tag.IsDeleted); });

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
                        IsArchived = page.IsArchived
                    }).ToDataListResponse(request);

            if (request.Data.IncludePageOptions && listResponse.Items.Count > 0)
            {
                LoadOptions(listResponse);
            }

            return new GetPagesResponse
            {
                Data = listResponse
            };
        }

        private void LoadOptions(DataListResponse<PageModel> response)
        {
            var layoutIds = response.Items.Select(i => i.LayoutId).Distinct().ToArray();
            var layoutOptionsFuture =
                repository.AsQueryable<LayoutOption>(l => layoutIds.Contains(l.Layout.Id))
                          .Select(layout => new { LayoutId = layout.Layout.Id, Option = layout })
                          .ToFuture();

            var pageIds = response.Items.Select(i => i.Id).Distinct().ToArray();
            var pageOptions =
                repository.AsQueryable<PageOption>(p => pageIds.Contains(p.Page.Id)).Select(page => new { PageId = page.Page.Id, Option = page }).ToFuture().ToList();

            var layoutOptions = layoutOptionsFuture.ToList();

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
}