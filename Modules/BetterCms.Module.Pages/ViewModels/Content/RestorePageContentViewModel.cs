using System;

namespace BetterCms.Module.Pages.ViewModels.Content
{
    /// <summary>
    /// Restore page content view model.
    /// </summary>
    public class RestorePageContentViewModel
    {
        /// <summary>
        /// Gets or sets the page content id.
        /// </summary>
        /// <value>
        /// The page content id.
        /// </value>
        public Guid PageContentId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user confirmed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if user confirmed; otherwise, <c>false</c>.
        /// </value>
        public bool IsUserConfirmed { get; set; }
        
        /// <summary>
        /// Determines, if child regions should be included to the results.
        /// </summary>
        /// <value>
        ///   <c>true</c> if child regions should be included to the results; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeChildRegions { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("PageContentId: {0}", PageContentId);
        }
    }
}