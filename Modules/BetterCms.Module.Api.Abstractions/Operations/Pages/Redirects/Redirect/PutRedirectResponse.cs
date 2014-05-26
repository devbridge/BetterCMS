using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Redirects.Redirect
{
    /// <summary>
    /// Redirect update response.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PutRedirectResponse : SaveResponseBase
    {
    }
}
