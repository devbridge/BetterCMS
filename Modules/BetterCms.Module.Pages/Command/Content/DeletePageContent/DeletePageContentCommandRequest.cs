using System;

namespace BetterCms.Module.Pages.Command.Content.DeletePageContent
{
    public class DeletePageContentCommandRequest
    {
        /// <summary>
        /// Gets or sets the page content id.
        /// </summary>
        /// <value>
        /// The page content id.
        /// </value>
        public Guid PageContentId { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int PageContentVersion { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int ContentVersion { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user has confirmed the deletion of content with dynamic regions.
        /// </summary>
        /// <value>
        /// <c>true</c> if user has confirmed the deletion of content with dynamic regions; otherwise, <c>false</c>.
        /// </value>
        public bool IsUserConfirmed { get; set; }
    }
}