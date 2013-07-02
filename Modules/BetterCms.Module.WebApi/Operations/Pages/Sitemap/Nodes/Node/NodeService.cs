using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap.Nodes.Node
{
    public class NodeService : Service, INodeService
    {
        public GetSitemapNodeResponse Get(GetSitemapNodeRequest request)
        {
            // TODO: need implementation
            return new GetSitemapNodeResponse
                       {
                           Data = new SitemapNodeModel
                                      {
                                          Id = request.NodeId
                                      }
                       };
        }
    }
}