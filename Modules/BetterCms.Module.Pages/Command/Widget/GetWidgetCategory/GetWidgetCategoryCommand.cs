using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Widgets;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.Widget.GetWidgetCategory
{
    /// <summary>
    /// Command to get widget categories model.
    /// </summary>
    public class GetWidgetCategoryCommand : CommandBase, ICommand<GetWidgetCategoryRequest, GetWidgetCategoryResponse>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public GetWidgetCategoryResponse Execute(GetWidgetCategoryRequest request)
        {
            IEnumerable<WidgetCategoryViewModel> categoriesFuture;

            // If re-loading fake category
            if (request.CategoryId.HasValue && request.CategoryId == default(Guid))
            {
                categoriesFuture = null;
            }
            else
            {
                var categoriesQuery = Repository.AsQueryable<Category>();

                if (request.CategoryId.HasValue)
                {
                    categoriesQuery = categoriesQuery.Where(c => c.Id == request.CategoryId.Value);
                }

                categoriesFuture = categoriesQuery
                    .OrderBy(c => c.Name)
                    .Select(c => new WidgetCategoryViewModel
                                    {
                                        CategoryId = c.Id,
                                        CategoryName = c.Name

                                    })
                    .ToFuture();
            }
            
            // Load list of contents
            var contentsQuery = Repository.AsQueryable<Root.Models.Widget>();

            if (request.CategoryId.HasValue)
            {
                if (request.CategoryId.Value.HasDefaultValue())
                {
                    contentsQuery = contentsQuery.Where(c => c.Category == null);
                }
                else
                {
                    contentsQuery = contentsQuery.Where(c => c.Category.Id == request.CategoryId.Value);
                }
            }

            if (!string.IsNullOrWhiteSpace(request.Filter))
            {
                contentsQuery = contentsQuery.Where(c => c.Name.ToLower().Contains(request.Filter.ToLowerInvariant()));
            }

            var contents = contentsQuery.OrderBy(f => f.Name).ToFuture().ToList().Select(CreateWidgetViewModel);

            List<WidgetCategoryViewModel> categories;
            // Load list of categories and move contents to categories.
            if (categoriesFuture != null)
            {
                categories = categoriesFuture.ToList();
            }
            else
            {
                categories = new List<WidgetCategoryViewModel>();
            }

            categories.ForEach(c => c.Widgets = contents.Where(x => x.CategoryId == c.CategoryId).ToList());

            // Move uncategorized contents to fake category
            var uncategorized = contents.Where(c => c.CategoryId == null).ToList();
            if (uncategorized.Any())
            {
                var category = new WidgetCategoryViewModel
                    {
                        CategoryName = PagesGlobalization.AddPageContent_WidgetTab_UncategorizedWidget_Title,
                        Widgets = uncategorized
                    };
                categories.Add(category);
            }

            // Remove empty categories
            categories = categories.Where(c => c.Widgets.Any()).ToList();

            return new GetWidgetCategoryResponse { WidgetCategories = categories };
        }

        private WidgetViewModel CreateWidgetViewModel(Root.Models.Widget widget)
        {
            WidgetViewModel result;
            if (widget is HtmlContentWidget)
            {
                HtmlContentWidget htmlContentWidget = (HtmlContentWidget)widget;
                result = new HtmlContentWidgetViewModel
                             {
                                 Name = htmlContentWidget.Name,
                                 PageContent = htmlContentWidget.Html,
                                 CustomCSS = htmlContentWidget.CustomCss,
                                 EnableCustomCSS = htmlContentWidget.UseCustomCss,
                                 CustomJS =  htmlContentWidget.CustomJs,
                                 EnableCustomJS = htmlContentWidget.UseCustomJs,                                 
                                 WidgetType = WidgetType.HtmlContent
                             };
            }
            else if (widget is ServerControlWidget)
            {
                ServerControlWidget serverControlWidget = (ServerControlWidget)widget;
                result = new ServerControlWidgetViewModel
                             {
                                 Url = serverControlWidget.Url,
                                 WidgetType = WidgetType.ServerControl
                             };
            }
            else
            {
                result = new WidgetViewModel
                             {
                                 WidgetType = null
                             };
            }

            result.Id = widget.Id;
            result.Name = widget.Name;
            // TODO: set preview image URL when it will be available.
            //result.PreviewImageUrl = widget.
            result.Version = widget.Version;
            result.CategoryId = widget.Category != null ? widget.Category.Id : (Guid?)null;
            
            return result;
        }
    }
}