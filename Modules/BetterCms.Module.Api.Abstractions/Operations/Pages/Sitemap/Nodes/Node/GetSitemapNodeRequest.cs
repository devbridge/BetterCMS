using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap.Nodes.Node
{
    [Route("/sitemap-node/{NodeId}", Verbs = "GET")]
    [DataContract]
    public class GetSitemapNodeRequest : IReturn<SitemapNodeModel>
    {
        [DataMember]
        public System.Guid NodeId { get; set; }
    }
}