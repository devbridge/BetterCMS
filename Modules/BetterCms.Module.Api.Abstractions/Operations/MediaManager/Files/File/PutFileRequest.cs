using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.MediaManager.Files.File
{
    /// <summary>
    /// Request for tag update or creation.
    /// </summary>
    [Route("/files/{Id}", Verbs = "PUT")]
    [DataContract]
    [Serializable]
    public class PutFileRequest : PutRequestBase<SaveFileModel>, IReturn<PutFileResponse>
    {
    }
}