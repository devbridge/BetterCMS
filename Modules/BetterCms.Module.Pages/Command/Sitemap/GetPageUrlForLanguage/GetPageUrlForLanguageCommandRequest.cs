using System;

namespace BetterCms.Module.Pages.Command.Sitemap.GetPageUrlForLanguage
{
    public class GetPageUrlForLanguageCommandRequest
    {
        /// <summary>
        /// Gets or sets the page identifier.
        /// </summary>
        /// <value>
        /// The page identifier.
        /// </value>
        public Guid PageId { get; set; }

        /// <summary>
        /// Gets or sets the language identifier.
        /// </summary>
        /// <value>
        /// The language identifier.
        /// </value>
        public Guid LanguageId { get; set; }
    }
}