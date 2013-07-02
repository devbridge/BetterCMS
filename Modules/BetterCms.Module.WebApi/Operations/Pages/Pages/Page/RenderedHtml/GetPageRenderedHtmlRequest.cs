using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.RenderedHtml
{
    [Route("/page-html/{PageId}", Verbs = "GET")]
    [Route("/page-html/by-url/{PageUrl*}", Verbs = "GET")]
    public class GetPageRenderedHtmlRequest : RequestBase, IReturn<GetPageRenderedHtmlResponse>
    {
        /// <summary>
        /// Gets or sets the page id.
        /// </summary>
        /// <value>
        /// The page id.
        /// </value>
        public System.Guid PageId { get; set; }

        /// <summary>
        /// Gets or sets the page URL.
        /// </summary>
        /// <value>
        /// The page URL.
        /// </value>
        public string PageUrl { get; set; }
    }
}