using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.GetRedirects
{
    [DataContract]
    public class GetRedirectsResponse : ListResponseBase<RedirectModel>
    {
    }
}