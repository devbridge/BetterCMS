using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Redirects.Redirect
{
    [DataContract]
    public class GetRedirectResponse : ResponseBase<RedirectModel>
    {
    }
}