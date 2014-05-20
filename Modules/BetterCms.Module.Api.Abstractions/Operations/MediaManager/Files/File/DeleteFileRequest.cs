using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.MediaManager.Files.File
{
    /// <summary>
    /// Request for file update or creation.
    /// </summary>
    [Route("/files/{Id}", Verbs = "DELETE")]
    [DataContract]
    [Serializable]
    public class DeleteFileRequest : DeleteRequestBase, IReturn<DeleteFileResponse>
    {
    }
}