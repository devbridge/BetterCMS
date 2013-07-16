using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap.Nodes
{
    [Route("/sitemap-nodes", Verbs = "GET")]
    public class GetSitemapNodesRequest : ListRequestBase, IReturn<GetSitemapNodesResponse>
    {
    }
}