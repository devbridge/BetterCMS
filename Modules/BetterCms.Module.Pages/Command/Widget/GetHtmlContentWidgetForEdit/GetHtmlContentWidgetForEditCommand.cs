using System;
using System.Linq;

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions.DataTier;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Helpers;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Widgets;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.ViewModels.Option;

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
        /// The options service
        /// </summary>
        private readonly IOptionService optionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetHtmlContentWidgetForEditCommand" /> class.
        /// </summary>
        /// <param name="categoryService">The category service.</param>
        /// <param name="contentService">The content service.</param>
        /// <param name="optionService">The option service.</param>
        public GetHtmlContentWidgetForEditCommand(ICategoryService categoryService, IContentService contentService, IOptionService optionService)
        {
            this.categoryService = categoryService;
            this.contentService = contentService;
            this.optionService = optionService;
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

                    model.Options = htmlContentWidget.ContentOptions.Distinct()
                        .Select(f =>
                                new OptionViewModel
                                    {
                                        Type = f.Type,
                                        OptionDefaultValue = optionService.ClearFixValueForEdit(f.Type, f.DefaultValue),
                                        OptionKey = f.Key,
                                        CanDeleteOption = f.IsDeletable,
                                        CustomOption = f.CustomOption != null
                                           ? new CustomOptionViewModel
                                             {
                                                 Identifier = f.CustomOption.Identifier, 
                                                 Title = f.CustomOption.Title,
                                                 Id = f.CustomOption.Id
                                             }
                                           : null
                                    })
                        .OrderBy(o => o.OptionKey)
                        .ToList();
                    optionService.SetCustomOptionValueTitles(model.Options);

                    if (htmlContentWidget.ContentRegions != null)
                    {
                        model.LastDynamicRegionNumber = RegionHelper.GetLastDynamicRegionNumber(
                            htmlContentWidget.ContentRegions.Select(cr => cr.Region.RegionIdentifier),
                            RegionHelper.WidgetDynamicRegionIdentifierPrefix);
                    }
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
            model.CustomOptions = optionService.GetCustomOptions();
            model.CanDestroyDraft = model.CurrentStatus == ContentStatus.Draft && model.HasPublishedContent;

            return model;
        }
    }
}