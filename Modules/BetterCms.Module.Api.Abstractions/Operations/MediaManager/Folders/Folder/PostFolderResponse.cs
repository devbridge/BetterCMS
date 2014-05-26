using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.MediaManager.Folders.Folder
{
    /// <summary>
    /// Folder creation response.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PostFolderResponse : SaveResponseBase
    {
    }
}