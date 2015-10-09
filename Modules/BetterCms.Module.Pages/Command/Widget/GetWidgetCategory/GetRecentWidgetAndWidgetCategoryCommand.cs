using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts.Enums;

using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Widgets;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

using FluentNHibernate.Conventions;
using FluentNHibernate.Utils;

using NHibernate.Linq;

using CategoryEntity = BetterCms.Module.Root.Models.Category;

namespace BetterCms.Module.Pages.Command.Widget.GetWidgetCategory
{
    /// <summary>
    /// Command to get widget categories model.
    /// </summary>
    public class GetRecentWidgetAndWidgetCategoryCommand : CommandBase, ICommand<GetRecentWidgetAndWidgetCategoryRequest, SelectWidgetViewModel>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public SelectWidgetViewModel Execute(GetRecentWidgetAndWidgetCategoryRequest request)
        {
            IEnumerable<WidgetCategoryViewModel> categoriesFuture;

            // If re-loading fake category
            if (request.CategoryId.HasValue && request.CategoryId == default(Guid))
            {
                categoriesFuture = null;
            }
            else
            {
                var categoriesQuery = Repository.AsQueryable<CategoryEntity>().Where(c => !c.IsDeleted && !c.CategoryTree.IsDeleted);

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
            var widgetsQuery = Repository.AsQueryable<Root.Models.Widget>()
                                        .Where(f => !f.IsDeleted 
                                                && (f.Original == null || !f.Original.IsDeleted) 
                                                && (f.Status == ContentStatus.Published || f.Status == ContentStatus.Draft));

            var childContentsQuery = UnitOfWork.Session.Query<ChildContent>();
            var pageContentsQuery = UnitOfWork.Session.Query<PageContent>();

            var pageContentRecentWidgetsFuture = widgetsQuery.Where(t => pageContentsQuery.Any(z => z.Content.Id == t.Id))
                .Select(t => new RecentWidget
                {
                    Widget = t,
                    UsedOn = pageContentsQuery.Where(z => z.Content.Id == t.Id).Max(z => z.ModifiedOn)
                })
                .OrderByDescending(t => t.UsedOn).Take(6).ToFuture();


            var childContentRecentWidgetsFuture = widgetsQuery.Where(t => childContentsQuery.Any(z => z.Child.Id == t.Id))
                .Select(t => new RecentWidget
                {
                    Widget = t,
                    UsedOn = childContentsQuery.Where(z => z.Child.Id == t.Id).Max(z => z.ModifiedOn)
                })
                .OrderByDescending(t => t.UsedOn).Take(6).ToFuture();
            
            if (request.CategoryId.HasValue)
            {
                if (request.CategoryId.Value.HasDefaultValue())
                {
                    widgetsQuery = widgetsQuery.Where(c => c.Categories == null);
                }
                else
                {
                    widgetsQuery = widgetsQuery.Where(wc => wc.Categories.Any(c => c.Id == request.CategoryId.Value));
                }
            }

            if (!string.IsNullOrWhiteSpace(request.Filter))
            {
                var filter = request.Filter.ToLowerInvariant();
                widgetsQuery = widgetsQuery.Where(c => c.Name.ToLower().Contains(filter) || c.Categories.Any(a=>a.Category.Name.ToLower().Contains(filter)));
            }

            // Load all widgets
            var contentEntities = widgetsQuery.OrderBy(f => f.Name).ToFuture().ToList();
            var pageContentRecentWidgets = pageContentRecentWidgetsFuture.ToList();
            var childContentRecentWidgets = childContentRecentWidgetsFuture.ToList();

            // Load drafts for published widgets
            var ids = contentEntities.Where(c => c.Status == ContentStatus.Published).Select(c => c.Id).ToArray();
            List<Root.Models.Widget> drafts;
            if (ids.Length > 0)
            {
                drafts = Repository
                    .AsQueryable<Root.Models.Widget>()
                    .Where(c => ids.Contains(c.Original.Id) && c.Status == ContentStatus.Draft && !c.IsDeleted)
                    .Fetch(c => c.Categories)
                    .ToList();
            }
            else
            {
                drafts = new List<Root.Models.Widget>();
            }

            // Map to view models
            var contents = contentEntities.Select(f => CreateWidgetViewModel(f, drafts.FirstOrDefault(d => d.Original.Id == f.Id))).ToList();
            var recentWidgets = LeaveTheMostRecentWidgets(pageContentRecentWidgets, childContentRecentWidgets)
                                .Select(f => CreateWidgetViewModel(f.Widget, drafts.FirstOrDefault(d => d.Original.Id == f.Widget.Id))).ToList();

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

            categories.ForEach(c => c.Widgets = contents.Where(x => x.Categories.Any(cat => Guid.Parse(cat.Key) == c.CategoryId)).Distinct().ToList());

            // Move uncategorized contents to fake category
            var uncategorized = contents.Where(c => c.Categories == null || c.Categories.IsEmpty()).Distinct().ToList();

            // Workaround for deleted categories:
            uncategorized = contents.Where(c => (c.Categories == null || c.Categories.IsEmpty()) && !categories.Any(x => c.Categories.Any(cat => Guid.Parse(cat.Key) == x.CategoryId))).Concat(uncategorized).Distinct().ToList();

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

            return new SelectWidgetViewModel
                       {
                           WidgetCategories = categories,
                           RecentWidgets = recentWidgets
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
                result.Categories = draft.Categories != null ? draft.Categories.Select(c => new LookupKeyValue()
                {
                    Key = c.Category.Id.ToLowerInvariantString(),
                    Value = c.Category.Name
                }).ToList() : new List<LookupKeyValue>();
                result.Id = draft.Id;
                result.Version = draft.Version;
            }
            else
            {
                result.Name = widget.Name;
                result.Categories = widget.Categories != null ? widget.Categories.Select(c => new LookupKeyValue()
                {
                    Key = c.Category.Id.ToLowerInvariantString(),
                    Value = c.Category.Name
                }).ToList() : new List<LookupKeyValue>();                
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

        private IEnumerable<RecentWidget> LeaveTheMostRecentWidgets(IEnumerable<RecentWidget> childContentRecentWidgets, IEnumerable<RecentWidget> pageContentRecentWidgets)
        {
            IEnumerable<RecentWidget> widgets = childContentRecentWidgets.Concat(pageContentRecentWidgets);
            var cleanList = new List<RecentWidget>();
            foreach (var widget in widgets.OrderByDescending(t => t.UsedOn))
            {
                if (!(cleanList.Any(t => t.Widget.Id == widget.Widget.Id)))
                {
                    cleanList.Add(widget);
                    if (cleanList.Count == 6)
                    {
                        return cleanList;
                    }
                }
            }
            return cleanList;
        }

        private class RecentWidget
        {
            public Root.Models.Widget Widget { get; set; }
            public DateTime UsedOn { get; set; }
        }
    }
}