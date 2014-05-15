using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.MediaManager.Folders
{
    /// <summary>
    /// Folder list request.
    /// </summary>
    [Route("/folders", Verbs = "GET")]
    [DataContract]
    [Serializable]
    public class GetFoldersRequest : RequestBase<GetFolderModel>, IReturn<GetFoldersResponse>
    {
    }
}
