using System;

namespace BetterCms.Module.Pages.Command.Page.SavePagePublishStatus
{
    public class SavePagePublishStatusRequest
    {
        /// <summary>
        /// Gets or sets the page id.
        /// </summary>
        /// <value>
        /// The page id.
        /// </value>
        public Guid PageId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether page is published.
        /// </summary>
        /// <value>
        /// <c>true</c> if page is published; otherwise, <c>false</c>.
        /// </value>
        public bool IsPublished { get; set; }
    }
}