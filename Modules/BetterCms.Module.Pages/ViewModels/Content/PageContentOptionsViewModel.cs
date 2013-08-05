using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Module.Pages.ViewModels.Option;

namespace BetterCms.Module.Pages.ViewModels.Content
{
    /// <summary>
    /// Edit page content options view model.
    /// </summary>
    public class PageContentOptionsViewModel : IOptionValuesContainer
    {      
        /// <summary>
        /// Gets or sets the list of page content options.
        /// </summary>
        /// <value>
        /// The list of page content options.
        /// </value>
        public IList<OptionValueViewModel> OptionValues { get; set; }

        /// <summary>
        /// Gets or sets the page content id.
        /// </summary>
        /// <value>
        /// The page content id.
        /// </value>
        public Guid PageContentId { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("WidgetOptions: " + string.Join(", ", OptionValues ?? Enumerable.Empty<OptionValueViewModel>()));
        }
    }
}