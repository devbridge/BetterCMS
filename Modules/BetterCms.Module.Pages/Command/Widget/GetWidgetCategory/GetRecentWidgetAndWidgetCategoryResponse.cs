using System.Collections.Generic;

using BetterCms.Module.Pages.ViewModels.Widgets;

namespace BetterCms.Module.Pages.Command.Widget.GetWidgetCategory
{
    public class GetRecentWidgetAndWidgetCategoryResponse
    {
        /// <summary>
        /// Gets or sets the list of the widget categories.
        /// </summary>
        /// <value>
        /// The widget category list.
        /// </value>
        public IList<WidgetCategoryViewModel> WidgetCategories { get; set; }

        /// <summary>
        /// Gets or sets the recent widgets.
        /// </summary>
        /// <value>
        /// The recent widgets.
        /// </value>
        public IList<WidgetViewModel> RecentWidgets { get; set; }
    }
}