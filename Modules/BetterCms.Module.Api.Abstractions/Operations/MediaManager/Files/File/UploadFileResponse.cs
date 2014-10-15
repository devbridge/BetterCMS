using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.MediaManager.Files.File
{
    /// <summary>
    /// Uploading file response.
    /// </summary>
    [DataContract]
    [Serializable]
    public class UploadFileResponse : SaveResponseBase
    {
    }
}
