using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Redirects.Redirect
{
    [DataContract]
    [Serializable]
    public class GetRedirectResponse : ResponseBase<RedirectModel>
    {
    }
}