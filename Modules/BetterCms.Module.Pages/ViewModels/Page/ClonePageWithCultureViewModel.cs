using System;

namespace BetterCms.Module.Pages.ViewModels.Page
{
    public class ClonePageWithCultureViewModel : ClonePageViewModel
    {
        /// <summary>
        /// Gets or sets the culture id.
        /// </summary>
        /// <value>
        /// The culture id.
        /// </value>
        public Guid CultureId { get; set; }

        /// <summary>
        /// Gets or sets the name of the culture.
        /// </summary>
        /// <value>
        /// The name of the culture.
        /// </value>
        public string CultureName { get; set; }
    }
}