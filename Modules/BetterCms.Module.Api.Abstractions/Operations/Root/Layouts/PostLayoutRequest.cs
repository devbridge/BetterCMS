using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root.Layouts.Layout;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Layouts
{
    /// <summary>
    /// Request for layout creation.
    /// </summary>
    [Route("/layouts", Verbs = "POST")]
    [DataContract]
    [Serializable]
    public class PostLayoutRequest : RequestBase<SaveLayoutModel>, IReturn<PostLayoutResponse>
    {
    }
}