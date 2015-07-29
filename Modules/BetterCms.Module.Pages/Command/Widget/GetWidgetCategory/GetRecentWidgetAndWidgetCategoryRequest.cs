using System;

namespace BetterCms.Module.Pages.Command.Widget.GetWidgetCategory
{
    public class GetRecentWidgetAndWidgetCategoryRequest
    {
        /// <summary>
        /// Gets or sets the search query for the widget filtering.
        /// </summary>
        /// <value>
        /// The search query for widget filtering.
        /// </value>
        public string Filter { get; set; }

        /// <summary>
        /// Gets or sets the category id.
        /// </summary>
        /// <value>
        /// The category id.
        /// </value>
        public Guid? CategoryId { get; set; }
    }
}