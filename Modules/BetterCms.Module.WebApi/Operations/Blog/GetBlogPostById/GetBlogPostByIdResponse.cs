using System.Runtime.Serialization;

using BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost;

namespace BetterCms.Module.Api.Operations.Blog.GetBlogPostById
{
    [DataContract]
    public class GetBlogPostByIdResponse : ResponseBase<BlogPostModel>
    {
    }
}