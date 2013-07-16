using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap.Tree
{
    [Route("/sitemap-tree", Verbs = "GET")]
    public class GetSitemapTreeRequest : RequestBase, IReturn<GetSitemapTreeResponse>
    {
        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        /// <value>
        /// The node id.
        /// </value>
        public System.Guid? NodeId { get; set; }
    }
}