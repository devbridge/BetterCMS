using BetterCms.Module.Api.Operations.Pages.Sitemap.Nodes;
using BetterCms.Module.Api.Operations.Pages.Sitemap.Nodes.Node;
using BetterCms.Module.Api.Operations.Pages.Sitemap.Tree;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap
{
    public class SitemapService : Service, ISitemapService
    {
        private readonly ISitemapTreeService treeService;

        private readonly INodesService nodesService;

        private readonly INodeService nodeService;

        public SitemapService(ISitemapTreeService treeService, INodeService nodeService, INodesService nodesService)
        {
            this.treeService = treeService;
            this.nodeService = nodeService;
            this.nodesService = nodesService;
        }

        ISitemapTreeService ISitemapService.Tree
        {
            get
            {
                return treeService;
            }
        }

        INodesService ISitemapService.Nodes
        {
            get
            {
                return nodesService;
            }
        }

        INodeService ISitemapService.Node
        {
            get
            {
                return nodeService;
            }
        }
    }
}