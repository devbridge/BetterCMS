using System;

using BetterCms.Core.DataContracts.Enums;

namespace BetterCms.Module.Blog.Commands.SaveBlogPost
{
    public class SaveBlogPostCommandResponse
    {
        /// <summary>
        /// Gets or sets the blog id.
        /// </summary>
        /// <value>
        /// The blog id.
        /// </value>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the page content id.
        /// </summary>
        /// <value>
        /// The page content id.
        /// </value>
        public virtual Guid PageContentId { get; set; }

        /// <summary>
        /// Gets or sets the entity version.
        /// </summary>
        /// <value>
        /// The entity version.
        /// </value>
        public virtual int Version { get; set; }

        /// <summary>
        /// Gets or sets the page URL.
        /// </summary>
        /// <value>
        /// The page URL.
        /// </value>
        public virtual string PageUrl { get; set; }

        /// <summary>
        /// Gets or sets the blog title.
        /// </summary>
        /// <value>
        /// The blog title.
        /// </value>
        public virtual string Title { get; set; }

        /// <summary>
        /// Gets or sets the date the blog post is modified on.
        /// </summary>
        /// <value>
        /// The date the blog post is modified on.
        /// </value>
        public string ModifiedOn { get; set; }
        
        /// <summary>
        /// Gets or sets the date the blog post is created on.
        /// </summary>
        /// <value>
        /// The date the blog post is created on.
        /// </value>
        public string CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the user, name which last modified the blog post.
        /// </summary>
        /// <value>
        /// The user name, which last modified the blog post.
        /// </value>
        public string ModifiedByUser { get; set; }

        /// <summary>
        /// Gets or sets the page status.
        /// </summary>
        /// <value>
        /// The page status.
        /// </value>
        public PageStatus PageStatus { get; set; }

        /// <summary>
        /// Gets or sets the desirable status.
        /// </summary>
        /// <value>
        /// The desirable status.
        /// </value>
        public ContentStatus DesirableStatus { get; set; }
    }
}