using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.Blog.Api.DataModels
{
    [DataContract]
    public class BlogPostModel
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public int Version { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public DateTime CreatedOn { get; set; }
    }
}