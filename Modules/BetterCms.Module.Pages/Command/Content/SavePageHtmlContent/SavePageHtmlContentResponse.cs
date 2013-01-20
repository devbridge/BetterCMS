using System;

using BetterCms.Module.Pages.Command.Base;

namespace BetterCms.Module.Pages.Command.Content.SavePageHtmlContent
{
    public class SavePageHtmlContentResponse : ISaveContentResponse
    {
        /// <summary>
        /// Gets or sets the content id.
        /// </summary>
        /// <value>
        /// The page content id.
        /// </value>
        public Guid ContentId { get; set; }

        public Guid PageContentId { get; set; }       
    }
}