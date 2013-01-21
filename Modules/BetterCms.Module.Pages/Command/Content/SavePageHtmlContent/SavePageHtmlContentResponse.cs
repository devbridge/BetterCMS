using System;

namespace BetterCms.Module.Pages.Command.Content.SavePageHtmlContent
{
    public class SavePageHtmlContentResponse
    {
        /// <summary>
        /// Gets or sets the content id.
        /// </summary>
        /// <value>
        /// The page content id.
        /// </value>
        public Guid ContentId { get; set; }

        /// <summary>
        /// Gets or sets the page content id.
        /// </summary>
        /// <value>
        /// The page content id.
        /// </value>
        public Guid PageContentId { get; set; }       
    }
}