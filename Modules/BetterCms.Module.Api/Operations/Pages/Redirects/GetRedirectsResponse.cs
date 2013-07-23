using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.Redirects
{
    [DataContract]
    public class GetRedirectsResponse : ListResponseBase<RedirectModel>
    {
    }
}