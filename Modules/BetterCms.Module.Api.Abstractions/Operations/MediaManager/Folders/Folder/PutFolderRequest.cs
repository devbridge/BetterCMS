using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.MediaManager.Folders.Folder
{
    /// <summary>
    /// Request for tag update or creation.
    /// </summary>
    [Route("/folders/{Id}", Verbs = "PUT")]
    [DataContract]
    [Serializable]
    public class PutFolderRequest : PutRequestBase<SaveFolderModel>, IReturn<PutFolderResponse>
    {
    }
}