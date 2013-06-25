using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Blog.GetBlogPosts
{
    [DataContract]
    public class GetBlogPostsResponse : ListResponseBase<BlogPostModel>
    {
    }
}