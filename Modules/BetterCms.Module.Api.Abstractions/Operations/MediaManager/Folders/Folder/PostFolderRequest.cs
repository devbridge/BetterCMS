using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.MediaManager.Folders.Folder
{
    /// <summary>
    /// Request for folder update or creation.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PostFolderRequest : RequestBase<Folder.SaveFolderModel>
    {
    }
}