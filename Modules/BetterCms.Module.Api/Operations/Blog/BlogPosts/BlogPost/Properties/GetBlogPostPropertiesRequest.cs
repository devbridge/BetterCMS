using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Properties
{
    [Route("/blog-post-properties/{BlogPostId}", Verbs = "GET")]
    [DataContract]
    public class GetBlogPostPropertiesRequest : RequestBase<GetBlogPostPropertiesModel>, IReturn<GetBlogPostPropertiesResponse>
    {
    }
}