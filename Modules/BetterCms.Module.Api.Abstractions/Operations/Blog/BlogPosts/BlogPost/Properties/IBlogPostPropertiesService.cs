namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Properties
{
    public interface IBlogPostPropertiesService
    {
        GetBlogPostPropertiesResponse Get(GetBlogPostPropertiesRequest request);

        PostBlogPostPropertiesResponse Post(PostBlogPostPropertiesRequest request);

        PutBlogPostPropertiesResponse Put(PutBlogPostPropertiesRequest request);

        DeleteBlogPostPropertiesResponse Delete(DeleteBlogPostPropertiesRequest request);
    }
}