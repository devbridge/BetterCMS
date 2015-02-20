using System;
using System.Linq;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;

using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Operations.Pages.Widgets.Widget.ServerControlWidget.Options;
using BetterCms.Module.Api.Operations.Root.Categories.Category;

using ServiceStack.ServiceInterface;

using ISaveWidgetService = BetterCms.Module.Pages.Services.IWidgetService;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.ServerControlWidget
{
    public class ServerControlWidgetService : Service, IServerControlWidgetService
    {
        private readonly IRepository repository;

        private readonly IServerControlWidgetOptionsService optionsService;

        private readonly ISaveWidgetService widgetService;

        public ServerControlWidgetService(IRepository repository, IServerControlWidgetOptionsService optionsService, ISaveWidgetService widgetService)
        {
            this.repository = repository;
            this.optionsService = optionsService;
            this.widgetService = widgetService;
        }

        public GetServerControlWidgetResponse Get(GetServerControlWidgetRequest request)
        {
            var model = repository
                .AsQueryable<Module.Pages.Models.ServerControlWidget>(widget => widget.Id == request.WidgetId)
                .Select(widget => new ServerControlWidgetModel
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
                        WidgetUrl = widget.Url,
                        PreviewUrl = widget.PreviewUrl
                    })
                .FirstOne();

            var response = new GetServerControlWidgetResponse { Data = model };
            if (request.Data.IncludeOptions)
            {
                response.Options = WidgetOptionsHelper.GetWidgetOptionsList(repository, request.WidgetId);
            }

            if (request.Data.IncludeCategories)
            {
                response.Categories = (from pagePr in repository.AsQueryable<Module.Root.Models.Widget>()
                                       from category in pagePr.Categories
                                       where pagePr.Id == request.WidgetId && !category.IsDeleted
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
            }

            return response;
        }

        public PostServerControlWidgetResponse Post(PostServerControlWidgetRequest request)
        {
            var result = Put(new PutServerControlWidgetRequest { Data = request.Data, User = request.User });

            return new PostServerControlWidgetResponse { Data = result.Data };
        }

        public PutServerControlWidgetResponse Put(PutServerControlWidgetRequest request)
        {
            var model = request.Data.ToServiceModel();
            if (request.Id.HasValue)
            {
                model.Id = request.Id.Value;
            }

            var widget = widgetService.SaveServerControlWidget(model, false, true);

            return new PutServerControlWidgetResponse { Data = widget.Id };
        }

        public DeleteServerControlWidgetResponse Delete(DeleteServerControlWidgetRequest request)
        {
            var result = widgetService.DeleteWidget(request.Id, request.Data.Version);

            return new DeleteServerControlWidgetResponse { Data = result };
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