using System;

using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.ViewModels.Author;

namespace BetterCms.Module.Blog.ViewModels.Blog
{
    public class RenderBlogPostViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderBlogPostViewModel" /> class.
        /// </summary>
        /// <param name="blogPost">The blog post.</param>
        /// <param name="content">The content.</param>
        public RenderBlogPostViewModel(BlogPost blogPost = null, BlogPostContent content = null)
        {
            if (content != null)
            {
                ActivationDate = content.ActivationDate;
                ExpirationDate = content.ExpirationDate;
            }

            if (blogPost != null)
            {
                if (blogPost.Author != null)
                {
                    Author = new RenderBlogPostAuthorViewModel(blogPost.Author);
                }

                if (content == null)
                {
                    ActivationDate = blogPost.ActivationDate;
                    ExpirationDate = blogPost.ExpirationDate;
                }
            }
        }

        /// <summary>
        /// Gets or sets the blog post author.
        /// </summary>
        /// <value>
        /// The blog post author.
        /// </value>
        public RenderBlogPostAuthorViewModel Author { get; set; }

        /// <summary>
        /// Gets or sets the blog post activation date.
        /// </summary>
        /// <value>
        /// The blog post activation date.
        /// </value>
        public DateTime ActivationDate { get; set; }

        /// <summary>
        /// Gets or sets the blog post expiration date.
        /// </summary>
        /// <value>
        /// The blog post expiration date.
        /// </value>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, ActivationDate: {1}, ExpirationDate: {2}", base.ToString(), ActivationDate, ExpirationDate);
        }
    }
}