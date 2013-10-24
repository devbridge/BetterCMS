using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts
{
    [DataContract]
    public class GetBlogPostsResponse : ListResponseBase<BlogPostModel>
    {
    }
}