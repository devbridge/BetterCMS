using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap
{
    /// <summary>
    /// Response with sitemap data.
    /// </summary>
    [DataContract]
    public class GetSitemapResponse : ResponseBase<SitemapModel>
    {
    }
}
