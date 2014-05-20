namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.Settings
{
    public interface IBlogPostsSettingsService
    {
        GetBlogPostsSettingsResponse GetSettings(GetBlogPostsSettingsRequest request);

        PutBlogPostsSettingsResponse PutSettings(PutBlogPostsSettingsRequest request);
    }
}