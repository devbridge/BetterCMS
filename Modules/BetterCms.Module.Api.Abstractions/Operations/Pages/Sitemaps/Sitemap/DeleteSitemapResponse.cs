using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap
{
    /// <summary>
    /// Sitemap delete response.
    /// </summary>
    [DataContract]
    public class DeleteSitemapResponse : ResponseBase<bool>
    {
    }
}