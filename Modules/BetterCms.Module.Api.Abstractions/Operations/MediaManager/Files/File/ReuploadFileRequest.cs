using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.MediaManager.Files.File
{
    /// <summary>
    /// Request for file re-upload.
    /// </summary>
    [DataContract]
    [Serializable]
    public class ReuploadFileRequest : PutRequestBase<ReuploadFileModel>
    {
    }
}
