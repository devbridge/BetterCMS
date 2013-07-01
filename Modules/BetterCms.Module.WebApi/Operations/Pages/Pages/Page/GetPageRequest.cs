using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page
{
    [Route("/pages/{PageId}", Verbs = "GET")]
    [Route("/pages/by-url/{PageUrl*}", Verbs = "GET")]
    public class GetPageRequest : RequestBase, IReturn<GetPageResponse>
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