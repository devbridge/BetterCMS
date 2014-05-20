using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties
{
    /// <summary>
    /// Page creation response.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PostPagePropertiesResponse : SaveResponseBase
    {
    }
}
