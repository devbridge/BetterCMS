using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.MediaManager.Folders.Folder
{
    /// <summary>
    /// Request for tag update or creation.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PutFolderRequest : PutRequestBase<SaveFolderModel>
    {
    }
}