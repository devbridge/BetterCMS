using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts
{
    [DataContract]
    public class GetBlogPostsResponse : ListResponseBase<BlogPostModel>
    {
    }
}