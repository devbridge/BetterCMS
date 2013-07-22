namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.RenderedHtml
{
    public class GetPageRenderedHtmlModel
    {
        /// <summary>
        /// Gets or sets the page id.
        /// </summary>
        /// <value>
        /// The page id.
        /// </value>
        public System.Guid? PageId { get; set; }

        /// <summary>
        /// Gets or sets the page URL.
        /// </summary>
        /// <value>
        /// The page URL.
        /// </value>
        public string PageUrl { get; set; }
    }
}