using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap
{
    /// <summary>
    /// Request to save sitemap.
    /// </summary>
    [Serializable]
    [DataContract]
    public class PutSitemapRequest : PutRequestBase<SaveSitemapModel>
    {
    }
}