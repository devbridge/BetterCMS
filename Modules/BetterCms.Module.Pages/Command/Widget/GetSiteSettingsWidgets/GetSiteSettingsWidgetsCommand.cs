using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Filter;
using BetterCms.Module.Pages.ViewModels.SiteSettings;

using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Pages.Command.Widget.GetSiteSettingsWidgets
{
    /// <summary>
    /// Gets a widget list for the site settings dialog.
    /// </summary>
    public class GetSiteSettingsWidgetsCommand : CommandBase, ICommand<WidgetsFilter, SiteSettingWidgetListViewModel>
    {
        private readonly IWidgetService widgetService;

        public GetSiteSettingsWidgetsCommand(IWidgetService widgetService)
        {
            this.widgetService = widgetService;
        }

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="gridOptions">The request.</param>
        /// <returns>A list of paged\sorted widgets.</returns>
        public SiteSettingWidgetListViewModel Execute(WidgetsFilter gridOptions)
        {
            return widgetService.GetFilteredWidgetsList(gridOptions);
        }
    }
}