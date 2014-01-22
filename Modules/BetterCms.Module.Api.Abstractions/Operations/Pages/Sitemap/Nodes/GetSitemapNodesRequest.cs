using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap.Nodes
{
    [Route("/sitemap-nodes/{SitemapId}", Verbs = "GET")]
    [DataContract]
    public class GetSitemapNodesRequest : RequestBase<DataOptions>, IReturn<GetSitemapNodesResponse>
    {
        [DataMember]
        public System.Guid SitemapId { get; set; }
    }
}