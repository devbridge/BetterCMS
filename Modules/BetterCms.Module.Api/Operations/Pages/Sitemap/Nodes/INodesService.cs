namespace BetterCms.Module.Api.Operations.Pages.Sitemap.Nodes
{
    public interface INodesService
    {
        GetSitemapNodesResponse Get(GetSitemapNodesRequest request);
    }
}