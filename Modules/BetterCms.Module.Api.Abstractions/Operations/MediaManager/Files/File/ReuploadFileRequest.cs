using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.MediaManager.Files.File
{
    /// <summary>
    /// Request for file re-upload.
    /// </summary>
    [DataContract]
    [Serializable]
    public class ReuploadFileRequest : PutRequestBase<ReuploadFileModel>, IReturn<PutFileResponse>
    {
    }
}
