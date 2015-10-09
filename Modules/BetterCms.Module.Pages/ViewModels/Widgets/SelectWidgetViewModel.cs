using System.Collections.Generic;

namespace BetterCms.Module.Pages.ViewModels.Widgets
{
    public class SelectWidgetViewModel
    {
        public SelectWidgetViewModel()
        {
            WidgetCategories = new List<WidgetCategoryViewModel>();
        }

        /// <summary>
        /// Gets or sets the list of the widget categories.
        /// </summary>
        /// <value>
        /// The list of the widget categories.
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