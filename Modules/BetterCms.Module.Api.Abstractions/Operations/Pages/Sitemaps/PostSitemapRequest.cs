using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap;
using BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes.Node;


namespace BetterCms.Module.Api.Operations.Pages.Sitemaps
{
    /// <summary>
    /// Request for sitemap creation.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PostSitemapRequest : RequestBase<SaveSitemapModel>
    {
    }
}