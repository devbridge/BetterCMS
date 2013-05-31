using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Widgets;
using BetterCms.Module.Root.Mvc;

using NHibernate.Linq;

using CategoryEntity = BetterCms.Module.Root.Models.Category;

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
                var categoriesQuery = Repository.AsQueryable<CategoryEntity>();

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
            var widgetsQuery = Repository.AsQueryable<Root.Models.Widget>().Where(f => !f.IsDeleted && f.Original == null && (f.Status == ContentStatus.Published || f.Status == ContentStatus.Draft));

            if (request.CategoryId.HasValue)
            {
                if (request.CategoryId.Value.HasDefaultValue())
                {
                    widgetsQuery = widgetsQuery.Where(c => c.Category == null);
                }
                else
                {
                    widgetsQuery = widgetsQuery.Where(c => c.Category.Id == request.CategoryId.Value);
                }
            }

            if (!string.IsNullOrWhiteSpace(request.Filter))
            {
                widgetsQuery = widgetsQuery.Where(c => c.Name.ToLower().Contains(request.Filter.ToLowerInvariant()));
            }

            // Load all widgets
            var contentEntities = widgetsQuery.OrderBy(f => f.Name).ToFuture().ToList();

            // Load drafts for published widgets
            var ids = contentEntities.Where(c => c.Status == ContentStatus.Published).Select(c => c.Id).ToArray();
            List<Root.Models.Widget> drafts;
            if (ids.Length > 0)
            {
                drafts = Repository
                    .AsQueryable<Root.Models.Widget>()
                    .Where(c => ids.Contains(c.Original.Id) && c.Status == ContentStatus.Draft && !c.IsDeleted)
                    .Fetch(c => c.Category)
                    .ToList();
            }
            else
            {
                drafts = new List<Root.Models.Widget>();
            }

            // Map to view models
            var contents = contentEntities.Select(f => CreateWidgetViewModel(f, drafts.FirstOrDefault(d => d.Original.Id == f.Id)));

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
            var uncategorized = contents.Where(c => c.CategoryId == null);

            // Workaround for deleted categories:
            uncategorized = contents.Where(c => c.CategoryId != null && !categories.Any(x => x.CategoryId == c.CategoryId)).Concat(uncategorized);

            if (uncategorized.Any())
            {
                var category = new WidgetCategoryViewModel
                    {
                        CategoryName = PagesGlobalization.AddPageContent_WidgetTab_UncategorizedWidget_Title,
                        Widgets = uncategorized.ToList()
                    };
                categories.Add(category);
            }

            // Remove empty categories
            categories = categories.Where(c => c.Widgets.Any()).ToList();

            return new GetWidgetCategoryResponse
                       {
                           WidgetCategories = categories
                       };
        }

        private WidgetViewModel CreateWidgetViewModel(Root.Models.Widget widget, Root.Models.Widget draft)
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

            result.PreviewImageUrl = widget.PreviewUrl;
            result.Status = Status(widget, draft);
            result.OriginalId = widget.Id;
            result.OriginalVersion = widget.Version;

            if (draft != null && !result.Status.Equals(ContentStatus.Published.ToString()))
            {
                result.Name = draft.Name;
                result.CategoryId = draft.Category != null ? draft.Category.Id : (Guid?)null;
                result.Id = draft.Id;
                result.Version = draft.Version;
            }
            else
            {
                result.Name = widget.Name;
                result.CategoryId = widget.Category != null ? widget.Category.Id : (Guid?)null;
                result.Id = widget.Id;
                result.Version = widget.Version;
            }

            return result;
        }

        private string Status(Root.Models.Widget widget, Root.Models.Widget draft)
        {
            if (widget.Status == ContentStatus.Published && draft != null)
            {
                return ContentStatus.Published.ToString() + "/" + ContentStatus.Draft.ToString();
            }
            
            if (widget.Status == ContentStatus.Draft)
            {
                return ContentStatus.Draft.ToString();
            }

            return ContentStatus.Published.ToString();
        }
    }
}