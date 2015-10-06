using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap
{
    /// <summary>
    /// Sitemap delete request.
    /// </summary>
    [Serializable]
    [DataContract]
    public class DeleteSitemapRequest : DeleteRequestBase
    {
    }
}