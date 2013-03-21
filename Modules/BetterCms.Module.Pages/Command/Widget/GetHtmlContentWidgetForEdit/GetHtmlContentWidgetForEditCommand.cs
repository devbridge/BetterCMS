using System;
using System.Linq;

using BetterCms.Core.Exceptions.DataTier;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Widgets;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;

namespace BetterCms.Module.Pages.Command.Widget.GetHtmlContentWidgetForEdit
{
    public class GetHtmlContentWidgetForEditCommand : CommandBase, ICommand<Guid?, EditHtmlContentWidgetViewModel>
    {
        /// <summary>
        /// The content service.
        /// </summary>
        private readonly IContentService contentService;

        /// <summary>
        /// The category service
        /// </summary>
        private readonly ICategoryService categoryService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetHtmlContentWidgetForEditCommand" /> class.
        /// </summary>
        /// <param name="categoryService">The category service.</param>
        /// <param name="contentService">The content service.</param>
        public GetHtmlContentWidgetForEditCommand(ICategoryService categoryService, IContentService contentService)
        {
            this.categoryService = categoryService;
            this.contentService = contentService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="widgetId">The html content widget id.</param>
        /// <returns>View model to </returns>
        public EditHtmlContentWidgetViewModel Execute(Guid? widgetId)
        {
            EditHtmlContentWidgetViewModel model = null;

            var categories = categoryService.GetCategories();

            if (widgetId.HasValue && widgetId.Value != Guid.Empty)
            {
                var htmlContentWidget = contentService.GetContentForEdit(widgetId.Value) as HtmlContentWidget;

                if (htmlContentWidget != null)
                {
                    model = new EditHtmlContentWidgetViewModel
                                {
                                    Id = htmlContentWidget.Id,
                                    Version = htmlContentWidget.Version,
                                    CategoryId = htmlContentWidget.Category != null ? htmlContentWidget.Category.Id : (Guid?)null,
                                    Name = htmlContentWidget.Name,
                                    PageContent = htmlContentWidget.Html,
                                    EnableCustomHtml = htmlContentWidget.UseHtml,
                                    EnableCustomCSS = htmlContentWidget.UseCustomCss,
                                    EditInSourceMode = htmlContentWidget.EditInSourceMode,
                                    CustomCSS = htmlContentWidget.CustomCss,
                                    EnableCustomJS = htmlContentWidget.UseCustomJs,
                                    CustomJS = htmlContentWidget.CustomJs,
                                    WidgetType = WidgetType.HtmlContent,
                                    PreviewImageUrl = null,
                                    CurrentStatus = htmlContentWidget.Status,
                                    HasPublishedContent = htmlContentWidget.Original != null
                                };
                }

                if (model == null)
                {
                    throw new EntityNotFoundException(typeof(HtmlContentWidget), widgetId.Value);
                }
            }
            else
            {
                model = new EditHtmlContentWidgetViewModel();
            }

            model.Categories = categories.ToList();

            return model;
        }
    }
}