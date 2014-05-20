using BetterCms.Module.Api.Operations.Blog.Authors;
using BetterCms.Module.Api.Operations.Blog.Authors.Author;
using BetterCms.Module.Api.Operations.Blog.BlogPosts;
using BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost;
using BetterCms.Module.Api.Operations.Blog.BlogPosts.Settings;

namespace BetterCms.Module.Api.Operations.Blog
{
    public interface IBlogApiOperations
    {
        IBlogPostsService BlogPosts { get; }
        
        IBlogPostService BlogPost { get; }
        
        IAuthorsService Authors { get; }
        
        IAuthorService Author { get; }

        IBlogPostsSettingsService Settings { get; }
    }
}