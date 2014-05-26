using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Properties
{
    [Route("/blog-post-properties/{BlogPostId}", Verbs = "GET")]
    [DataContract]
    [System.Serializable]
    public class GetBlogPostPropertiesRequest : RequestBase<GetBlogPostPropertiesModel>, IReturn<GetBlogPostPropertiesResponse>
    {
        [DataMember]
        public System.Guid BlogPostId
        {
            get; set;
        }
    }
}