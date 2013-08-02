namespace BetterCms.Module.Api.Operations.Blog.BlogPosts
{
    public interface IBlogPostsService
    {
        GetBlogPostsResponse Get(GetBlogPostsRequest request);
    }
}