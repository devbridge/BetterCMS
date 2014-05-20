using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties
{
    /// <summary>
    /// Page delete request for REST.
    /// </summary>
    [Route("/page-properties/{Id}", Verbs = "DELETE")]
    [DataContract]
    [Serializable]
    public class DeletePagePropertiesRequest : DeleteRequestBase, IReturn<DeletePagePropertiesResponse>
    {
    }
}