using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap.Nodes.Node
{
    [Route("/sitemap-nodes/{NodeId}", Verbs = "GET")]
    public class GetSitemapNodeRequest : RequestBase, IReturn<SitemapNodeModel>
    {
        /// <summary>
        /// Gets or sets the sitemap node id.
        /// </summary>
        /// <value>
        /// The sitemap node id.
        /// </value>
        public System.Guid NodeId { get; set; }
    }
}