using System;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Widgets
{
    public class WidgetsService : Service, IWidgetsService
    {
        private readonly IRepository repository;

        public WidgetsService(IRepository repository)
        {
            this.repository = repository;
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
                     CategoryId = widget.Category != null && !widget.Category.IsDeleted ? widget.Category.Id : (Guid?)null,
                     CategoryName = widget.Category != null && !widget.Category.IsDeleted ?  widget.Category.Name : null,
                     OriginalWidgetType = widget.GetType()
                 }).ToDataListResponse(request);

            // Set content types
            listResponse.Items.ToList().ForEach(item => item.WidgetType = item.OriginalWidgetType.ToContentTypeString());

            return new GetWidgetsResponse
                       {
                           Data = listResponse
                       };
        }
    }
}