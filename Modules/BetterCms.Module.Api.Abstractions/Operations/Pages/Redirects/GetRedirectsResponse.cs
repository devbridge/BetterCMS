using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Redirects
{
    [DataContract]
    [Serializable]
    public class GetRedirectsResponse : ListResponseBase<RedirectModel>
    {
    }
}