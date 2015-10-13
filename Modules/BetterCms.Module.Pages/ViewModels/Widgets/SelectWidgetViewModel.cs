using System;
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

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return String.Format(
                "{0}, Categories: {1}, RecentWidgets: {2}",
                base.ToString(),
                WidgetCategories != null ? WidgetCategories.Count : 0,
                RecentWidgets != null ? RecentWidgets.Count : 0);
        }
    }
}