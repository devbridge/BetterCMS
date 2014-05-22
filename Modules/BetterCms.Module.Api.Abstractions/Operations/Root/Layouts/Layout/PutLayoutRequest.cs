using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout
{
    /// <summary>
    /// Request for layout update or creation.
    /// </summary>
    [Route("/layouts/{Id}", Verbs = "PUT")]
    [DataContract]
    [Serializable]
    public class PutLayoutRequest : PutRequestBase<SaveLayoutModel>, IReturn<PutLayoutResponse>
    {
    }
}