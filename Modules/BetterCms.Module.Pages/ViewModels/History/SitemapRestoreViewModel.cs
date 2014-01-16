using System;

namespace BetterCms.Module.Pages.ViewModels.History
{
    /// <summary>
    /// Restore sitemap view model.
    /// </summary>
    public class SitemapRestoreViewModel
    {
        public Guid SitemapVersionId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user confirmed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if user confirmed; otherwise, <c>false</c>.
        /// </value>
        public bool IsUserConfirmed { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("SitemapVersionId: {0}", SitemapVersionId);
        }
    }
}