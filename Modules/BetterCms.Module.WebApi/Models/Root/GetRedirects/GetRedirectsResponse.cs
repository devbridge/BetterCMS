using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Root.GetRedirects
{
    [DataContract]
    public class GetRedirectsResponse : ListResponseBase<RedirectModel>
    {
    }
}