using System;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Api.Operations.Pages.Widgets.Widget.ServerControlWidget.Options;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.ServerControlWidget
{
    public class ServerControlWidgetService : Service, IServerControlWidgetService
    {
        private readonly IRepository repository;

        private readonly IServerControlWidgetOptionsService optionsService;

        public ServerControlWidgetService(IRepository repository, IServerControlWidgetOptionsService optionsService)
        {
            this.repository = repository;
            this.optionsService = optionsService;
        }

        public GetServerControlWidgetResponse Get(GetServerControlWidgetRequest request)
        {
            var model = repository
                .AsQueryable<Module.Pages.Models.ServerControlWidget>(widget => widget.Id == request.WidgetId)
                .Select(widget => new ServerControlWidgetModel()
                    {
                        Id = widget.Id,
                        Version = widget.Version,
                        CreatedBy = widget.CreatedByUser,
                        CreatedOn = widget.CreatedOn,
                        LastModifiedBy = widget.ModifiedByUser,
                        LastModifiedOn = widget.ModifiedOn,

                        Name = widget.Name,
                        IsPublished = widget.Status == ContentStatus.Published,
                        PublishedOn = widget.Status == ContentStatus.Published ? widget.PublishedOn : null,
                        PublishedByUser = widget.Status == ContentStatus.Published ? widget.PublishedByUser : null,
                        CategoryId = widget.Category != null && !widget.Category.IsDeleted ? widget.Category.Id : (Guid?)null,
                        CategoryName = widget.Category != null && !widget.Category.IsDeleted ? widget.Category.Name : null,
                        WidgetUrl = widget.Url,
                        PreviewUrl = widget.PreviewUrl
                    })
                .FirstOne();

            return new GetServerControlWidgetResponse { Data = model };
        }

        IServerControlWidgetOptionsService IServerControlWidgetService.Options
        {
            get
            {
                return optionsService;
            }
        }
    }
}