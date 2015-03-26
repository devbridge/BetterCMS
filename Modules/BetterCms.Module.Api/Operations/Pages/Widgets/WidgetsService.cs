using System;
using System.Linq;

using BetterModules.Core.DataAccess;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root.Categories.Category;
using BetterCms.Module.Root.Services;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Widgets
{
    public class WidgetsService : Service, IWidgetsService
    {
        private readonly IRepository repository;
        private readonly ICategoryService categoryService;

        public WidgetsService(IRepository repository, ICategoryService categoryService)
        {
            this.repository = repository;
            this.categoryService = categoryService;
        }

        public GetWidgetsResponse Get(GetWidgetsRequest request)
        {
            request.Data.SetDefaultOrder("Name");

            var query = repository
                .AsQueryable<Module.Root.Models.Widget>()
                .Where(widget => widget.Original == null);

            if (!request.Data.IncludeUnpublished)
            {
                query = query.Where(widget => widget.Status == ContentStatus.Published);
            }

            query.ApplyCategoriesFilter(categoryService, request.Data);

            var listResponse = query
                 .Select(widget => new WidgetModel
                 {
                     Id = widget.Id,
                     Version = widget.Version,
                     CreatedBy = widget.CreatedByUser,
                     CreatedOn = widget.CreatedOn,
                     LastModifiedBy = widget.ModifiedByUser,
                     LastModifiedOn = widget.ModifiedOn,

                     Name = widget.Name,
                     IsPublished = widget.Status == ContentStatus.Published,
                     PublishedOn = widget.PublishedOn,
                     PublishedByUser = widget.PublishedByUser,
                     OriginalWidgetType = widget.GetType()
                 }).ToDataListResponse(request);

            // Set content types
            listResponse.Items.ToList().ForEach(
                item =>
                {
                    item.WidgetType = item.OriginalWidgetType.ToContentTypeString();

                    item.Categories = (from pagePr in repository.AsQueryable<Module.Root.Models.Widget>()
                                       from category in pagePr.Categories
                                       where pagePr.Id == item.Id && !category.IsDeleted
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

            return new GetWidgetsResponse
                       {
                           Data = listResponse
                       };
        }
    }
}