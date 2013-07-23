using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap.Nodes
{
    [Route("/sitemap-nodes", Verbs = "GET")]
    [DataContract]
    public class GetSitemapNodesRequest : RequestBase<DataOptions>, IReturn<GetSitemapNodesResponse>
    {
    }
}