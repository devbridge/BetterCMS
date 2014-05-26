using BetterCms.Module.Api.Operations.Blog.Authors;
using BetterCms.Module.Api.Operations.Blog.Authors.Author;
using BetterCms.Module.Api.Operations.Blog.BlogPosts;
using BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost;
using BetterCms.Module.Api.Operations.Blog.BlogPosts.Settings;

namespace BetterCms.Module.Api.Operations.Blog
{
    public class DefaultBlogApiOperations : IBlogApiOperations
    {
        public DefaultBlogApiOperations(IBlogPostsService blogPosts, IBlogPostService blogPost, IAuthorsService authors,
            IAuthorService author, IBlogPostsSettingsService settings)
        {
            BlogPost = blogPost;
            BlogPosts = blogPosts;
            Author = author;
            Authors = authors;
            Settings = settings;
        }

        public IBlogPostsService BlogPosts { get; private set; }

        public IBlogPostService BlogPost { get; private set; }

        public IAuthorsService Authors { get; private set; }

        public IAuthorService Author { get; private set; }

        public IBlogPostsSettingsService Settings { get; private set; }
    }
}