using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.MediaManager.Files.File
{
    [DataContract]
    [System.Serializable]
    public class GetFileRequest : RequestBase<GetFileModel>
    {
        [DataMember]
        public System.Guid FileId
        {
            get; set;
        }
    }
}