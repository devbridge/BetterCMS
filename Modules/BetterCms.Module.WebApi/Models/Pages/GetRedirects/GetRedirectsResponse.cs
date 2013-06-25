using System.Runtime.Serialization;

using BetterCms.Module.WebApi.Models.Root;

namespace BetterCms.Module.WebApi.Models.Pages.GetRedirects
{
    [DataContract]
    public class GetRedirectsResponse : ListResponseBase<RedirectModel>
    {
    }
}