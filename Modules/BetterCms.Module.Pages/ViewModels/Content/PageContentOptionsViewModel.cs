using System.Collections.Generic;
using System.Linq;

namespace BetterCms.Module.Pages.ViewModels.Content
{
    /// <summary>
    /// Edit page content options view model.
    /// </summary>
    public class PageContentOptionsViewModel
    {      
        /// <summary>
        /// Gets or sets the list of page content options.
        /// </summary>
        /// <value>
        /// The list of page content options.
        /// </value>
        public IList<PageContentOptionViewModel> WidgetOptions { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("WidgetOptions: " + string.Join(", ", WidgetOptions ?? Enumerable.Empty<PageContentOptionViewModel>()));
        }
    }
}