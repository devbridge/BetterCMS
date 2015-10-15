using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.ViewModels.Security;

namespace BetterCms.Module.Root.ViewModels.Option
{
    /// <summary>
    /// Edit content option values view model.
    /// </summary>
    public class ContentOptionValuesViewModel : IAccessSecuredViewModel
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
        /// Gets or sets the option values container id.
        /// </summary>
        /// <value>
        /// The option values container id.
        /// </value>
        public Guid OptionValuesContainerId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether dialog should be opened in the read only mode.
        /// </summary>
        /// <value>
        /// <c>true</c> if dialog should be opened in the read only mode; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Gets or sets the languages.
        /// </summary>
        /// <value>
        /// The languages.
        /// </value>
        public List<LookupKeyValue> Languages { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show languages.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show languages]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowLanguages { get; set; }

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