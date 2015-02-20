using System;
using System.Collections.Generic;
using System.Linq;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;

using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Operations.Pages.Widgets.Widget.HtmlContentWidget.Options;
using BetterCms.Module.Api.Operations.Root.Categories.Category;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.ViewModels.Option;

using ServiceStack.ServiceInterface;

using ISaveWidgetService = BetterCms.Module.Pages.Services.IWidgetService;
using HtmlContentWidgetEntity = BetterCms.Module.Pages.Models.HtmlContentWidget;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.HtmlContentWidget
{
    public class HtmlContentWidgetService : Service, IHtmlContentWidgetService
    {
        private readonly IRepository repository;

        private readonly IHtmlContentWidgetOptionsService optionsService;

        private readonly ISaveWidgetService widgetService;
        
        private readonly IOptionService optionService;

        public HtmlContentWidgetService(IRepository repository, IHtmlContentWidgetOptionsService optionsService,
            ISaveWidgetService widgetService, IOptionService optionService)
        {
            this.repository = repository;
            this.optionsService = optionsService;
            this.widgetService = widgetService;
            this.optionService = optionService;
        }

        public GetHtmlContentWidgetResponse Get(GetHtmlContentWidgetRequest request)
        {
            var model = repository
                .AsQueryable<Module.Pages.Models.HtmlContentWidget>(widget => widget.Id == request.WidgetId)
                .Select(widget => new HtmlContentWidgetModel
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
                        CustomCss = widget.CustomCss,
                        UseCustomCss = widget.UseCustomCss,
                        Html = widget.Html,
                        UseHtml = widget.UseHtml,
                        CustomJavaScript = widget.CustomJs,
                        UseCustomJavaScript = widget.UseCustomJs
                    })
                .FirstOne();

            var response = new GetHtmlContentWidgetResponse { Data = model };
            if (request.Data.IncludeOptions)
            {
                response.Options = WidgetOptionsHelper.GetWidgetOptionsList(repository, request.WidgetId);
            }

            if (request.Data.IncludeChildContentsOptions)
            {
                response.ChildContentsOptionValues = optionService
                    .GetChildContentsOptionValues(request.WidgetId)
                    .ToServiceModel();
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

        public PostHtmlContentWidgetResponse Post(PostHtmlContentWidgetRequest request)
        {
            var result = Put(new PutHtmlContentWidgetRequest
                             {
                                 Data = request.Data, 
                                 User = request.User
                             });

            return new PostHtmlContentWidgetResponse { Data = result.Data };
        }

        public PutHtmlContentWidgetResponse Put(PutHtmlContentWidgetRequest request)
        {
            HtmlContentWidgetEntity widget;
            HtmlContentWidgetEntity originalWidget;

            var model = request.Data.ToServiceModel();
            if (request.Id.HasValue)
            {
                model.Id = request.Id.Value;
            }

            IList<ContentOptionValuesViewModel> childContents = null;
            if (request.Data.ChildContentsOptionValues != null)
            {
                childContents = request.Data.ChildContentsOptionValues.ToViewModel();
            }
            widgetService.SaveHtmlContentWidget(model, childContents, out widget, out originalWidget, false, true);

            return new PutHtmlContentWidgetResponse { Data = widget.Id };
        }

        public DeleteHtmlContentWidgetResponse Delete(DeleteHtmlContentWidgetRequest request)
        {
            var result = widgetService.DeleteWidget(request.Id, request.Data.Version);

            return new DeleteHtmlContentWidgetResponse { Data = result };
        }

        IHtmlContentWidgetOptionsService IHtmlContentWidgetService.Options
        {
            get
            {
                return optionsService;
            }
        }
    }
}