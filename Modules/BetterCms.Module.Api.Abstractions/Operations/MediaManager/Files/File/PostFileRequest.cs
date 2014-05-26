using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.MediaManager.Files.File
{
    /// <summary>
    /// Request for file update or creation.
    /// </summary>
    [Route("/files", Verbs = "POST")]
    [DataContract]
    [Serializable]
    public class PostFileRequest : RequestBase<File.SaveFileModel>, IReturn<PostFileResponse>
    {
    }
}