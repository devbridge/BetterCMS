using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.MediaManager.Files.File
{
    /// <summary>
    /// Request for tag update or creation.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PutFileRequest : PutRequestBase<SaveFileModel>
    {
    }
}