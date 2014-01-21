using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap
{
    [DataContract]
    public class GetSitemapsResponse : ListResponseBase<SitemapModel>
    {
    }
}