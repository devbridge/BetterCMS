using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Redirects.Redirect
{
    /// <summary>
    /// Redirect delete request for REST.
    /// </summary>
    [DataContract]
    [Serializable]
    public class DeleteRedirectRequest : DeleteRequestBase
    {
    }
}