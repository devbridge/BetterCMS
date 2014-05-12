using System;
using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes.Node
{
    // TODO: double check the route.
    [Route("/sitemaps/{SitemapId}/nodes/{NodeId}", Verbs = "GET")]
    [Serializable]
    [DataContract]
    public class GetSitemapNodeRequest : IReturn<SitemapNodeModel>
    {
        [DataMember]
        public Guid SitemapId { get; set; } // TODO: not used.

        [DataMember]
        public Guid NodeId { get; set; }
    }
}