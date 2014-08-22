using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout
{
    /// <summary>
    /// Request for layout delete operation.
    /// </summary>
    [Route("/layouts/{Id}", Verbs = "DELETE")]
    [DataContract]
    [Serializable]
    public class DeleteLayoutRequest : DeleteRequestBase, IReturn<DeleteLayoutResponse>
    {
    }
}