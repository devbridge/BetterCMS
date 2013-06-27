using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Blog.GetBlogPosts
{
    [DataContract]
    public class GetBlogPostsResponse : ListResponseBase<BlogPostModel>
    {
    }
}