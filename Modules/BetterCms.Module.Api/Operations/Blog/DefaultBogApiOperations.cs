using BetterCms.Module.Api.Operations.Blog.Authors;
using BetterCms.Module.Api.Operations.Blog.Authors.Author;
using BetterCms.Module.Api.Operations.Blog.BlogPosts;
using BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost;

namespace BetterCms.Module.Api.Operations.Blog
{
    public class DefaultBlogApiOperations : IBlogApiOperations
    {
        public DefaultBlogApiOperations(IBlogPostsService blogPosts, IBlogPostService blogPost, IAuthorsService authors,
            IAuthorService author)
        {
            BlogPost = blogPost;
            BlogPosts = blogPosts;
            Author = author;
            Authors = authors;
        }

        public IBlogPostsService BlogPosts { get; private set; }

        public IBlogPostService BlogPost { get; private set; }

        public IAuthorsService Authors { get; private set; }

        public IAuthorService Author { get; private set; }
    }
}