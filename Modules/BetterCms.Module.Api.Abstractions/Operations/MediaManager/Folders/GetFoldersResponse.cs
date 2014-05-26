using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.MediaManager.Folders.Folder;

namespace BetterCms.Module.Api.Operations.MediaManager.Folders
{
    [DataContract]
    [Serializable]
    public class GetFoldersResponse : ListResponseBase<FolderModel>
    {
    }
}