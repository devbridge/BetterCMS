using System;

using BetterCms.Module.Blog.Models;
using BetterCms.Module.Pages.Models;

namespace BetterCms.Module.Blog.Api.Events
{
    /// <summary>
    /// Blog Article Event Arguments
    /// </summary>
    public class BlogPostEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlogPostEventArgs" /> class.
        /// </summary>
        /// <param name="blogPost">The page.</param>
        public BlogPostEventArgs(BlogPost blogPost)
        {
            BlogPost = blogPost;
        }

        /// <summary>
        /// Gets or sets the created page.
        /// </summary>
        /// <value>
        /// The page.
        /// </value>
        public BlogPost BlogPost { get; set; }
    }
}
