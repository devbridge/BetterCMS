using System;
using System.Runtime.Serialization;

using BetterCms.Module.Blog.Models;

namespace BetterCms.Module.Blog.Api.DataContracts.Models
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

        public static BlogPostModel FromEntity(BlogPost blog)
        {
            return new BlogPostModel
            {
                Id = blog.Id,
                CreatedOn = blog.CreatedOn,
                Title = blog.Title,
                Version = blog.Version
            };
        }
        
        public static BlogPost ToEntity(BlogPostModel model)
        {
            return new BlogPost
            {
                Id = model.Id,
                CreatedOn = model.CreatedOn,
                Title = model.Title,
                Version = model.Version
            };
        }
    }
}