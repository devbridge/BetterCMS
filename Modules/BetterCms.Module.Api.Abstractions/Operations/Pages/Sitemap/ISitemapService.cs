using BetterCms.Module.Api.Operations.Pages.Sitemap.Nodes;
using BetterCms.Module.Api.Operations.Pages.Sitemap.Nodes.Node;
using BetterCms.Module.Api.Operations.Pages.Sitemap.Tree;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap
{
    public interface ISitemapService
    {
        GetSitemapsResponse Get(GetSitemapsRequest request);

        ISitemapTreeService Tree { get; }
        
        INodesService Nodes { get; }
        
        INodeService Node { get; }
    }
}