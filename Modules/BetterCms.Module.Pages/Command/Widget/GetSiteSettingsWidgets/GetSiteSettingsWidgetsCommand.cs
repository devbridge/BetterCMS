using System.Linq;

using BetterCms.Core.Mvc;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.SiteSettings;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;

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
            var query =
                Repository.AsQueryable<Root.Models.Widget>()
                          .Where(f => f.IsDeleted == false)
                          .Select(
                              f =>
                              new SiteSettingWidgetItemViewModel
                                  {
                                      Id = f.Id,
                                      Version = f.Version,
                                      WidgetName = f.Name,
                                      CategoryName = f.Category.Name,
                                      WidgetEntityType = f.GetType()
                                  });

            if (gridOptions != null)
            {
                gridOptions.SetDefaultSortingOptions("WidgetName");

                if (!string.IsNullOrWhiteSpace(gridOptions.SearchQuery))
                {
                    var searchQuery = gridOptions.SearchQuery.ToLowerInvariant();
                    query = query.Where(f => f.CategoryName.ToLower().Contains(searchQuery) || f.WidgetName.ToLower().Contains(searchQuery));
                }
            }
            
            var count = query.ToRowCountFutureValue();

            var widgets = query.AddSortingAndPaging(gridOptions).ToFuture().ToList();

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