using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap.Nodes.Node
{
    [Route("/sitemap-nodes/{NodeId}", Verbs = "GET")]
    [DataContract]
    public class GetSitemapNodeRequest : RequestBase<GetSitemapNodeModel>, IReturn<SitemapNodeModel>
    {
    }
}