using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost
{
    [DataContract]
    public class GetBlogPostResponse : ResponseBase<BlogPostModel>
    {
    }
}