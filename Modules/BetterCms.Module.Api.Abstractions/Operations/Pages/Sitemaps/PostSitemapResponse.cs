using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps
{
    /// <summary>
    /// Sitemap creation response.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PostSitemapResponse : SaveResponseBase
    {
    }
}