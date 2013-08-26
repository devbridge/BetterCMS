using System;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.HtmlContentWidget
{
    public class HtmlContentWidgetService : Service, IHtmlContentWidgetService
    {
        private readonly IRepository repository;

        public HtmlContentWidgetService(IRepository repository)
        {
            this.repository = repository;
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
                        CategoryId = widget.Category != null && !widget.Category.IsDeleted ? widget.Category.Id : (Guid?)null,
                        CategoryName = widget.Category != null && !widget.Category.IsDeleted ? widget.Category.Name : null,
                        CustomCss = widget.CustomCss,
                        UseCustomCss = widget.UseCustomCss,
                        Html = widget.Html,
                        UseHtml = widget.UseHtml,
                        CustomJavaScript = widget.CustomJs,
                        UseCustomJavaScript = widget.UseCustomJs
                    })
                .FirstOne();

            return new GetHtmlContentWidgetResponse { Data = model };
        }
    }
}