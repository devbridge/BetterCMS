using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts.Enums;

using BetterCms.Module.Pages.Helpers;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Widgets;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.ViewModels.Option;

using BetterModules.Core.Exceptions.DataTier;
using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Pages.Command.Widget.GetHtmlContentWidgetForEdit
{
    public class GetHtmlContentWidgetForEditCommand : CommandBase, ICommand<Guid?, EditHtmlContentWidgetViewModel>
    {
        /// <summary>
        /// The content service.
        /// </summary>
        private readonly IContentService contentService;

        /// <summary>
        /// The CMS configuration
        /// </summary>
        private readonly ICmsConfiguration cmsConfiguration;

        /// <summary>
        /// The category service
        /// </summary>
        private readonly ICategoryService categoryService;

        /// <summary>
        /// The options service
        /// </summary>
        private readonly IOptionService optionService;

        /// <summary>
        /// The language service
        /// </summary>
        private readonly ILanguageService languageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetHtmlContentWidgetForEditCommand" /> class.
        /// </summary>
        /// <param name="categoryService">The category service.</param>
        /// <param name="contentService">The content service.</param>
        /// <param name="optionService">The option service.</param>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        /// <param name="languageService"></param>
        public GetHtmlContentWidgetForEditCommand(ICategoryService categoryService, IContentService contentService, IOptionService optionService, ICmsConfiguration cmsConfiguration, ILanguageService languageService)
        {
            this.categoryService = categoryService;
            this.contentService = contentService;
            this.optionService = optionService;
            this.cmsConfiguration = cmsConfiguration;
            this.languageService = languageService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="widgetId">The html content widget id.</param>
        /// <returns>View model to </returns>
        public EditHtmlContentWidgetViewModel Execute(Guid? widgetId)
        {
            EditHtmlContentWidgetViewModel model = null;

            var languagesFuture = cmsConfiguration.EnableMultilanguage ? languageService.GetLanguagesLookupValues() : new List<LookupKeyValue>();
            var languages = cmsConfiguration.EnableMultilanguage ? languagesFuture.ToList() : new List<LookupKeyValue>();

            if (widgetId.HasValue && widgetId.Value != Guid.Empty)
            {
                var htmlContentWidget = contentService.GetContentForEdit(widgetId.Value) as HtmlContentWidget;

                if (htmlContentWidget != null)
                {
                    model = new EditHtmlContentWidgetViewModel
                                {
                                    Id = htmlContentWidget.Id,
                                    Version = htmlContentWidget.Version,                                    
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
                                           : null,
                                        Translations = cmsConfiguration.EnableMultilanguage ? 
                                            f.Translations.Select(x => new OptionTranslationViewModel
                                            {
                                                LanguageId = x.Language.Id.ToString(),
                                                OptionValue = optionService.ClearFixValueForEdit(f.Type, x.Value)
                                            }).ToList() : null
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
            model.ShowLanguages = cmsConfiguration.EnableMultilanguage && languages.Any();
            model.Languages = languages;
            model.Categories = categoryService.GetSelectedCategories<Root.Models.Widget, WidgetCategory>(widgetId).ToList();
            model.CustomOptions = optionService.GetCustomOptions();
            model.CanDestroyDraft = model.CurrentStatus == ContentStatus.Draft && model.HasPublishedContent;
            model.CategoriesFilterKey = Root.Models.Widget.CategorizableItemKeyForWidgets;

            return model;
        }
    }
}