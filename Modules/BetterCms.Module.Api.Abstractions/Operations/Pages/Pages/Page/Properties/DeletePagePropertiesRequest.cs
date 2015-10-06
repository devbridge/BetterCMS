using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties
{
    /// <summary>
    /// Page delete request for REST.
    /// </summary>
    [DataContract]
    [Serializable]
    public class DeletePagePropertiesRequest : DeleteRequestBase
    {
    }
}