using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Nodes
{
    /// <summary>
    /// Sitemap node creation response.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PostSitemapNodeResponse : SaveResponseBase
    {
    }
}
