using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.Blog.Api.DataModels
{
    [DataContract]
    public class AuthorModel
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public int Version { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public DateTime CreatedOn { get; set; }

        [DataMember]
        public DateTime ModifiedOn { get; set; }

        [DataMember]
        public string CreatedByUser { get; set; }

        [DataMember]
        public string ModifiedByUser { get; set; }

        // image fields
        [DataMember]
        public Guid? ImageId { get; set; }
        
        [DataMember]
        public int ImageVersion { get; set; }
        
        [DataMember]
        public string ImageCaption { get; set; }

        [DataMember]
        public string ImagePublicUrl { get; set; }
        
        [DataMember]
        public string ImagePublicThumbnailUrl { get; set; }
    }
}