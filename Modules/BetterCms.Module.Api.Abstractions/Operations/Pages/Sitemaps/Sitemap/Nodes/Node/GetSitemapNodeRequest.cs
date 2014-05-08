using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes.Node
{
    // TODO: double check the route.
    [Route("/sitemaps/{SitemapId}/nodes/{NodeId}", Verbs = "GET")]
    [DataContract]
    public class GetSitemapNodeRequest : IReturn<SitemapNodeModel>
    {
        [DataMember]
        public System.Guid SitemapId { get; set; } // TODO: not used.

        [DataMember]
        public System.Guid NodeId { get; set; }
    }
}