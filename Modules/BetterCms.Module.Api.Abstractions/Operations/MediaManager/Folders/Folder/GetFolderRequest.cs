using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.MediaManager.Folders.Folder
{
    [Route("/folders/{FolderId}", Verbs = "GET")]
    [DataContract]
    [Serializable]
    public class GetFolderRequest : RequestBase<GetFolderModel>, IReturn<GetFolderResponse>
    {
        [DataMember]
        public Guid FolderId
        {
            get; set;
        }
    }
}