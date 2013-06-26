using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;

using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.SiteSettings;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;

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
            gridOptions.SetDefaultSortingOptions("WidgetName");

            var query = Repository.AsQueryable<Root.Models.Widget>()
                          .Where(f => !f.IsDeleted && (f.Status == ContentStatus.Published || f.Status == ContentStatus.Draft));

            var modelQuery = query.Select(f => new SiteSettingWidgetItemViewModel
                                  {
                                      Id = f.Id,
                                      OriginalId = f.Status == ContentStatus.Draft && f.Original != null && f.Original.Status == ContentStatus.Published ? f.Original.Id : f.Id,
                                      Version = f.Version,
                                      OriginalVersion = f.Status == ContentStatus.Draft && f.Original != null && f.Original.Status == ContentStatus.Published ? f.Original.Version : f.Version,
                                      WidgetName = f.Name,
                                      CategoryName = (!f.Category.IsDeleted) ? f.Category.Name : null,
                                      WidgetEntityType = f.GetType(),
                                      IsPublished = f.Status == ContentStatus.Published || (f.Original != null && f.Original.Status == ContentStatus.Published),
                                      HasDraft = f.Status == ContentStatus.Draft
                                  });

            if (!string.IsNullOrWhiteSpace(gridOptions.SearchQuery))
            {
                var searchQuery = gridOptions.SearchQuery.ToLowerInvariant();
                modelQuery = modelQuery.Where(f => f.CategoryName.ToLower().Contains(searchQuery) || f.WidgetName.ToLower().Contains(searchQuery));
            }

            modelQuery = modelQuery.ToList()
                .GroupBy(g => g.OriginalId)
                .Select(grp => grp.OrderByDescending(p => p.HasDraft).First())
                .AsQueryable();

            var count = modelQuery.ToRowCountFutureValue();
            var widgets = modelQuery.AddSortingAndPaging(gridOptions).ToList();

            widgets.ForEach(
                item =>
                    {
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
                    });

            return new SiteSettingWidgetListViewModel(widgets, gridOptions, count.Value);
        }
    }
}