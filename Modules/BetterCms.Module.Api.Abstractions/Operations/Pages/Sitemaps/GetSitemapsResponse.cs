using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps
{
    /// <summary>
    /// Response for sitemaps list.
    /// </summary>
    [DataContract]
    public class GetSitemapsResponse : ListResponseBase<SitemapModel>
    {
    }
}