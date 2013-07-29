using BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Properties;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts
{
    public interface IBlogPostsService
    {
        GetBlogPostsResponse Get(GetBlogPostsRequest request);

        IBlogPostPropertiesService Properties { get; }
    }
}