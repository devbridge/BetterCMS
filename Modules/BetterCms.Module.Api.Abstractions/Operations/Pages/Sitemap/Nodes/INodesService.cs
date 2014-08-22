using System;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap.Nodes
{
    [Obsolete("Use everything from BetterCms.Module.Api.Operations.Pages.Sitemaps name space.")]
    public interface INodesService
    {
        GetSitemapNodesResponse Get(GetSitemapNodesRequest request);
    }
}