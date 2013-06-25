using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.MediaManager
{
    [DataContract]
    public class FolderWithMediasModel : FolderModel
    {
        [DataMember(Order = 510, Name = "childMedias")]
        public IList<MediaModelBase> ChildMedias { get; set; }
    }
}