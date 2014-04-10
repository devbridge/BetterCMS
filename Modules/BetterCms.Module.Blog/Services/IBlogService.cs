using System.Security.Principal;

using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.ViewModels.Blog;

namespace BetterCms.Module.Blog.Services
{
    public interface IBlogService
    {
        /// <summary>
        /// Creates the blog URL from the given blog title.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <returns>Created blog URL</returns>
        string CreateBlogPermalink(string title);

        /// <summary>
        /// Saves the blog post.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="principal">The principal.</param>
        /// <returns>
        /// Saved blog post entity
        /// </returns>
        BlogPost SaveBlogPost(BlogPostViewModel model, IPrincipal principal);
    }
}