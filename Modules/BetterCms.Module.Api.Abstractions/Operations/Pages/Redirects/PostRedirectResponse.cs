using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Redirects
{
    /// <summary>
    /// Redirect creation response.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PostRedirectResponse : SaveResponseBase
    {
    }
}
