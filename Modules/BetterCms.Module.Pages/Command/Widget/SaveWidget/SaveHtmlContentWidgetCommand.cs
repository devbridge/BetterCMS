using System;
using System.Linq;

using BetterCms.Api;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Widgets;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;

namespace BetterCms.Module.Pages.Command.Widget.SaveWidget
{
    public class SaveHtmlContentWidgetCommand : SaveWidgetCommandBase<EditHtmlContentWidgetViewModel>
    {
        public virtual IContentService ContentService { get; set; }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override SaveWidgetResponse Execute(EditHtmlContentWidgetViewModel request)
        {
            UnitOfWork.BeginTransaction();
            var widgetContent = GetHtmlContentWidgetFromRequest(request);
            HtmlContentWidget widget = (HtmlContentWidget)ContentService.SaveContentWithStatusUpdate(widgetContent, request.DesirableStatus);
            Repository.Save(widget);

            UnitOfWork.Commit();

            // Notify.
            if (widget.Status != ContentStatus.Preview)
            {
                if (request.Id == default(Guid))
                {
                    PagesApiContext.Events.OnWidgetCreated(widget);
                }
                else
                {
                    PagesApiContext.Events.OnWidgetUpdated(widget);
                }
            }

            return new SaveWidgetResponse
                    {
                        Id = widget.Id,
                        WidgetName = request.Name, 
                        CategoryName = widgetContent.Category != null ? widgetContent.Category.Name : null,
                        Version = widget.Version,
                        WidgetType = WidgetType.HtmlContent.ToString(),
                        IsPublished = widget.Status == ContentStatus.Published,
                        HasDraft = widget.Status == ContentStatus.Draft || widget.History != null && widget.History.Any(f => f.Status == ContentStatus.Draft),
                        DesirableStatus = request.DesirableStatus,
                        PreviewOnPageContentId = request.PreviewOnPageContentId
                    };
        }

        private HtmlContentWidget GetHtmlContentWidgetFromRequest(EditHtmlContentWidgetViewModel request)
        {
            HtmlContentWidget content = new HtmlContentWidget();
            content.Id = request.Id;

            if (request.CategoryId.HasValue && !request.CategoryId.Value.HasDefaultValue())
            {
                content.Category = Repository.AsProxy<Category>(request.CategoryId.Value);
            }
            else
            {
                content.Category = null;
            }

            content.Name = request.Name;
            content.Html = request.PageContent ?? string.Empty;
            content.UseHtml = request.EnableCustomHtml;
            content.UseCustomCss = request.EnableCustomCSS;
            content.CustomCss = request.CustomCSS;            
            content.UseCustomJs = request.EnableCustomJS;
            content.CustomJs = request.CustomJS;           
            content.Version = request.Version;
            content.EditInSourceMode = request.EditInSourceMode;

            return content;
        }
    }
}