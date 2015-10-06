using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Properties
{
    [DataContract]
    [System.Serializable]
    public class GetBlogPostPropertiesRequest : RequestBase<GetBlogPostPropertiesModel>
    {
        [DataMember]
        public System.Guid BlogPostId
        {
            get; set;
        }
    }
}