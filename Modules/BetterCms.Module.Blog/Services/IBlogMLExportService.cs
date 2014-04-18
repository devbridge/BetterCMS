using System.Collections.Generic;

using BetterCms.Module.Blog.Models;

namespace BetterCms.Module.Blog.Services
{
    public interface IBlogMLExportService
    {
        /// <summary>
        /// Exports the blog posts to blogML string.
        /// </summary>
        /// <param name="posts">The posts.</param>
        /// <returns>string in blogML format</returns>
        string ExportBlogPosts(List<BlogPost> posts);
    }
}