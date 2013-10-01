using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Module.Root.ViewModels.Option;
using BetterCms.Module.Root.ViewModels.Security;

namespace BetterCms.Module.Pages.ViewModels.Content
{
    /// <summary>
    /// Edit page content options view model.
    /// </summary>
    public class PageContentOptionsViewModel : IAccessSecuredViewModel
    {
        /// <summary>
        /// Gets or sets the list of page content options.
        /// </summary>
        /// <value>
        /// The list of page content options.
        /// </value>
        public IList<OptionValueEditViewModel> OptionValues { get; set; }

        /// <summary>
        /// Gets or sets the custom options.
        /// </summary>
        /// <value>
        /// The custom options.
        /// </value>
        public List<CustomOptionViewModel> CustomOptions { get; set; }

        /// <summary>
        /// Gets or sets the page content id.
        /// </summary>
        /// <value>
        /// The page content id.
        /// </value>
        public Guid PageContentId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether dialog should be opened in the read only mode.
        /// </summary>
        /// <value>
        /// <c>true</c> if dialog should be opened in the read only mode; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("WidgetOptions: " + string.Join(", ", OptionValues ?? Enumerable.Empty<OptionValueEditViewModel>()));
        }
    }
}