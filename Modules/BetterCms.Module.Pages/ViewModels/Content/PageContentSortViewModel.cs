using System.Collections.Generic;

namespace BetterCms.Module.Pages.ViewModels.Content
{
    using System;

    /// <summary>
    /// Edit page content options view model.
    /// </summary>
    public class PageContentSortViewModel
    {
        /// <summary>
        /// Gets or sets page id.
        /// </summary>
        /// <value>
        /// The page id.
        /// </value>
        public Guid PageId { get; set; }

        /// <summary>
        /// Gets or sets the list of page content ids.
        /// </summary>
        /// <value>
        /// The list of page content ids and versions.
        /// </value>
        public IList<ContentSortViewModel> PageContents { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("PageId: {0}", PageId);
        }
    }
}