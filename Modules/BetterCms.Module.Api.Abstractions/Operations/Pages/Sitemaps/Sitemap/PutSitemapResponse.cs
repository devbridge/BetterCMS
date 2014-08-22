using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap
{
    /// <summary>
    /// Page save response.
    /// </summary>
    [Serializable]
    [DataContract]
    public class PutSitemapResponse : SaveResponseBase
    {
    }
}