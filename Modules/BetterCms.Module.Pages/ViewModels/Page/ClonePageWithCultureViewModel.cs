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

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, CultureId: {1}, CultureName: {2}", base.ToString(), CultureId, CultureName);
        }
    }
}