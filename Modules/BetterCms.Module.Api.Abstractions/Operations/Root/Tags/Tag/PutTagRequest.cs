using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Tags.Tag
{
    /// <summary>
    /// Request for tag update or creation.
    /// </summary>
    [Route("/tags/{Id}", Verbs = "PUT")]
    [DataContract]
    [Serializable]
    public class PutTagRequest : PutRequestBase<SaveTagModel>, IReturn<PutTagResponse>
    {
    }
}