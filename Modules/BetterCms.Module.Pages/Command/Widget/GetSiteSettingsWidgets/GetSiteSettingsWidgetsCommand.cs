using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;

using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.SiteSettings;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;

using MvcContrib.Sorting;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.Widget.GetSiteSettingsWidgets
{
    /// <summary>
    /// Gets a widget list for the site settings dialog.
    /// </summary>
    public class GetSiteSettingsWidgetsCommand : CommandBase, ICommand<SearchableGridOptions, SiteSettingWidgetListViewModel>
    {
        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="gridOptions">The request.</param>
        /// <returns>A list of paged\sorted widgets.</returns>
        public SiteSettingWidgetListViewModel Execute(SearchableGridOptions gridOptions)
        {
            gridOptions = gridOptions ?? new SearchableGridOptions();

            var query = Repository.AsQueryable<Root.Models.Widget>()
                          .Where(f => f.IsDeleted == false && f.Original == null && (f.Status == ContentStatus.Published || f.Status == ContentStatus.Draft));

            gridOptions.SetDefaultSortingOptions("WidgetName");

            if (gridOptions.Column == "Status")
            {
                query = query.OrderBy(gridOptions.Column, gridOptions.Direction);
            }

            var modelQuery = query.Select(f => new SiteSettingWidgetItemViewModel
                                  {
                                      Id = f.Id,
                                      Version = f.Version,
                                      WidgetName = f.Name,
                                      CategoryName = (!f.Category.IsDeleted) ? f.Category.Name : null,
                                      WidgetEntityType = f.GetType(),
                                      IsPublished = f.Status == ContentStatus.Published,
                                      HasDraft = f.Status == ContentStatus.Draft
                                  });

            if (!string.IsNullOrWhiteSpace(gridOptions.SearchQuery))
            {
                var searchQuery = gridOptions.SearchQuery.ToLowerInvariant();
                modelQuery = modelQuery.Where(f => f.CategoryName.ToLower().Contains(searchQuery) || f.WidgetName.ToLower().Contains(searchQuery));
            }
            
            var count = modelQuery.ToRowCountFutureValue();

            if (gridOptions.Column == "Status")
            {
                modelQuery = modelQuery.AddPaging(gridOptions);
            }
            else
            {
                modelQuery = modelQuery.AddSortingAndPaging(gridOptions);
            }

            // Load widgets
            var widgets = modelQuery.ToFuture().ToList();

            // Load drafts
            var ids = widgets.Where(c => c.IsPublished).Select(c => c.Id).ToArray();
            List<Root.Models.Widget> drafts;
            if (ids.Length > 0)
            {
                drafts = Repository
                    .AsQueryable<Root.Models.Widget>()
                    .Fetch(c => c.Category)
                    .Where(c => ids.Contains(c.Original.Id) && c.Status == ContentStatus.Draft && !c.IsDeleted)
                    .ToList();
            }
            else
            {
                drafts = new List<Root.Models.Widget>();
            }

            widgets.ForEach(
                item =>
                    {
                        var draft = drafts.LastOrDefault(d => d.Original.Id == item.Id);
                        if (typeof(ServerControlWidget).IsAssignableFrom(item.WidgetEntityType))
                        {
                            item.WidgetType = WidgetType.ServerControl;
                        }
                        else if (typeof(HtmlContentWidget).IsAssignableFrom(item.WidgetEntityType))
                        {
                            item.WidgetType = WidgetType.HtmlContent;
                        }
                        else
                        {
                            item.WidgetType = null;
                        }
                        if (draft != null)
                        {                            
                            item.CategoryName = draft.Category != null && !draft.Category.IsDeleted ? draft.Category.Name : "";
                            item.WidgetName = draft.Name;
                            item.HasDraft = true;
                        }
                    });

            if (gridOptions.Column == "Status")
            {
                if (gridOptions.Direction == SortDirection.Ascending)
                {
                    widgets = widgets
                        .OrderByDescending(w => w.HasDraft && !w.IsPublished)
                        .ThenBy(w => w.IsPublished)
                        .ToList();
                }
                else
                {
                    widgets = widgets
                       .OrderByDescending(w => w.IsPublished && !w.HasDraft)
                       .ThenBy(w => !w.HasDraft)
                       .ToList();
                }
            }

            return new SiteSettingWidgetListViewModel(widgets, gridOptions, count.Value);
        }
    }
}