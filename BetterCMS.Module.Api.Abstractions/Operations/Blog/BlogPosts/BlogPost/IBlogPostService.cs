namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost
{
    public interface IBlogPostService
    {
        GetBlogPostResponse Get(GetBlogPostRequest request);
    }
}