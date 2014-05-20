namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.Settings
{
    public interface IBlogPostsSettingsService
    {
        GetBlogPostsSettingsResponse Get(GetBlogPostsSettingsRequest request);

        PutBlogPostsSettingsResponse Put(PutBlogPostsSettingsRequest request);
    }
}