using System;
using System.Runtime.Serialization;

using BetterCms.Core.DataContracts.Enums;

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
        public string Description { get; set; }

        [DataMember]
        public DateTime CreatedOn { get; set; }

        [DataMember]
        public PageStatus Status { get; set; }

        [DataMember]
        public DateTime ActivationDate { get; set; }
        
        [DataMember]
        public DateTime? ExpirationDate { get; set; }

        [DataMember]
        public Guid? CategoryId { get; set; }

        [DataMember]
        public string CategoryName { get; set; }

        [DataMember]
        public Guid? AuthorId { get; set; }
        
        [DataMember]
        public string AuthorName { get; set; }
        
        [DataMember]
        public Guid? MainImageId { get; set; }
        
        [DataMember]
        public Guid? FeaturedImageId { get; set; }
        
        [DataMember]
        public Guid? SecondaryImageId { get; set; }

        [DataMember]
        public string MainImagePublicUrl { get; set; }
        
        [DataMember]
        public string FeaturedImagePublicUrl { get; set; }
        
        [DataMember]
        public string SecondaryImagePublicUrl { get; set; }
    }
}