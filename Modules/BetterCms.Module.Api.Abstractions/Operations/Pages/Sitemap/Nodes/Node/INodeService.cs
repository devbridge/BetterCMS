namespace BetterCms.Module.Api.Operations.Pages.Sitemap.Nodes.Node
{
    public interface INodeService
    {
        GetSitemapNodeResponse Get(GetSitemapNodeRequest request);
    }
}