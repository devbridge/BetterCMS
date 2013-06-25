using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Pages.GetRedirects
{
    [DataContract]
    public class GetRedirectsResponse : ListResponseBase<RedirectModel>
    {
    }
}