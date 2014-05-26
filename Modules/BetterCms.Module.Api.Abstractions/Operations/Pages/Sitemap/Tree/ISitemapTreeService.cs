using System;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap.Tree
{
    [Obsolete("Use everything from BetterCms.Module.Api.Operations.Pages.Sitemaps name space.")]
    public interface ISitemapTreeService
    {
        GetSitemapTreeResponse Get(GetSitemapTreeRequest request);
    }
}