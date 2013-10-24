using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Redirects
{
    [DataContract]
    public class GetRedirectsResponse : ListResponseBase<RedirectModel>
    {
    }
}