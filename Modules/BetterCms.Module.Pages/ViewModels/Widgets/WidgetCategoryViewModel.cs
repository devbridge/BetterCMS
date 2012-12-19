using System;
using System.Collections.Generic;

namespace BetterCms.Module.Pages.ViewModels.Widgets
{
    /// <summary>
    /// Advanced page content category view model
    /// </summary>
    public class WidgetCategoryViewModel
    {
        /// <summary>
        /// Gets or sets the category id.
        /// </summary>
        /// <value>
        /// The content id.
        /// </value>
        public Guid? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        /// <value>
        /// The name of the category.
        /// </value>
        public string CategoryName { get; set; }

        /// <summary>
        /// Gets or sets the list of widgets.
        /// </summary>
        /// <value>
        /// The list of widgets.
        /// </value>
        public IList<WidgetViewModel> Widgets { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Id: {0}, Name: {1}", CategoryId, CategoryName);
        }
    }
}