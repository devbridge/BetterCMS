using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.MediaManager.Api.DataModels
{
    [DataContract]
    public class MediaImageModel
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public int Version { get; set; }

        [DataMember]
        public string PublicUrl { get; set; }

        [DataMember]
        public string PublicThumbnailUrl { get; set; }

        [DataMember]
        public string Caption { get; set; }
    }
}