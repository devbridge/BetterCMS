using System;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap.Nodes.Node
{
    [Obsolete("Use everything from BetterCms.Module.Api.Operations.Pages.Sitemaps name space.")]
    public interface INodeService
    {
        GetSitemapNodeResponse Get(GetSitemapNodeRequest request);
    }
}