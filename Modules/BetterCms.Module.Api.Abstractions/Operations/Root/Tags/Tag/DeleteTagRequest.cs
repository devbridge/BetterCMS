using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Tags.Tag
{
    /// <summary>
    /// Request for tag delete operation.
    /// </summary>
    [Route("/tags/{Id}", Verbs = "DELETE")]
    [DataContract]
    [Serializable]
    public class DeleteTagRequest : DeleteRequestBase, IReturn<DeleteTagResponse>
    {
    }
}