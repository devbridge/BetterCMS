using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.MediaManager.MediaTree
{
    [Route("/media-tree", Verbs = "GET")]
    [DataContract]
    [Serializable]
    public class GetMediaTreeRequest : RequestBase<GetMediaTreeModel>, IReturn<GetMediaTreeResponse>
    {
    }
}