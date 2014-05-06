using BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Properties;
using BetterCms.Module.Api.Operations.Pages.Contents.Content.BlogPostContent;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost
{
    public interface IBlogPostService
    {
        GetBlogPostResponse Get(GetBlogPostRequest request);
        
        PutBlogPostResponse Put(PutBlogPostRequest request);

        IBlogPostPropertiesService Properties { get; }

        IBlogPostContentService Content { get; }
    }
}