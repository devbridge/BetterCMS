using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Widgets;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using NHibernate.Linq;
using NHibernate;

namespace BetterCms.Module.Pages.Command.Widget.GetHtmlContentWidgetForEdit
{
    public class GetHtmlContentWidgetForEditCommand : CommandBase, ICommand<Guid?, EditHtmlContentWidgetViewModel>
    {
        /// <summary>
        /// The category service
        /// </summary>
        private readonly ICategoryService categoryService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetHtmlContentWidgetForEditCommand" /> class.
        /// </summary>
        /// <param name="categoryService">The category service.</param>
        public GetHtmlContentWidgetForEditCommand(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="widgetId">The html content widget id.</param>
        /// <returns>View model to </returns>
        public EditHtmlContentWidgetViewModel Execute(Guid? widgetId)
        {
             var categories = categoryService.GetCategories();

             var model = widgetId == null 
                ? new EditHtmlContentWidgetViewModel() 
                : Repository.AsQueryable<HtmlContentWidget>()
                          .Select(
                              c =>
                              new EditHtmlContentWidgetViewModel
                                  {
                                      Id = c.Id,
                                      Version = c.Version,
                                      CategoryId = c.Category != null ? c.Category.Id : (Guid?)null,
                                      Name = c.Name,
                                      PageContent = c.Html,
                                      EnableCustomHtml = c.UseHtml,
                                      EnableCustomCSS = c.UseCustomCss,
                                      CustomCSS = c.CustomCss,
                                      EnableCustomJS = c.UseCustomJs,
                                      CustomJS = c.CustomJs,
                                      WidgetType = WidgetType.HtmlContent,
                                      PreviewImageUrl = null
                                  })
                           .FirstOrDefault(c => c.Id == widgetId);

            if (model == null)
            {
                model = new EditHtmlContentWidgetViewModel();
            }

            model.Categories = categories.ToList();
            
            return model;
        }
    }
}