using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.MediaManager.Files.File
{
    /// <summary>
    /// Request for upload file from the stream.
    /// </summary>
    [DataContract]
    [Serializable]
    public class UploadFileRequest : RequestBase<UploadFileModel>, IReturn<UploadFileResponse>
    {
    }
}
