using System;

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Root.Mvc.Grids;

namespace BetterCms.Module.Blog.ViewModels.Blog
{
    public class SiteSettingBlogPostViewModel : IEditableGridItem
    {
        /// <summary>
        /// Gets or sets the blog id.
        /// </summary>
        /// <value>
        /// The blog id.
        /// </value>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the entity version.
        /// </summary>
        /// <value>
        /// The entity version.
        /// </value>
        public virtual int Version { get; set; }

        /// <summary>
        /// Gets or sets the blog title.
        /// </summary>
        /// <value>
        /// The blog title.
        /// </value>
        public virtual string Title { get; set; }

        /// <summary>
        /// Gets or sets the date the blog post is created on.
        /// </summary>
        /// <value>
        /// The date the blog post is created on.
        /// </value>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the date the blog post is modified on.
        /// </summary>
        /// <value>
        /// The date the blog post is modified on.
        /// </value>
        public DateTime ModifiedOn { get; set; }

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
        /// Gets or sets the page url.
        /// </summary>
        /// <value>
        /// The page url.
        /// </value>
        public string PageUrl { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Id: {0}, Version: {1}, Title: {2}", Id, Version, Title);
        }
    }
}