using BetterCms.Api;
using BetterCms.Module.Blog.Models;

namespace BetterCms.Module.Blog.Api.Events
{
    /// <summary>
    /// Attachable blog events container
    /// </summary>
    public class BlogEvents
    {
        /// <summary>
        /// Delegate to handle a blog post creation event.
        /// </summary>
        /// <param name="args">The <see cref="Pages.Api.Events.PagePropertiesEventArgs" /> instance containing the event data.</param>
        public delegate void BlogCreatedEventHandler(BlogPostEventArgs args);

        /// <summary>
        /// Delegate to handle a blog post update event.
        /// </summary>
        /// <param name="args">The <see cref="Pages.Api.Events.PagePropertiesEventArgs" /> instance containing the event data.</param>
        public delegate void BlogUpdatedEventHandler(BlogPostEventArgs args);

        /// <summary>
        /// Delegate to handle a blog post deletion event.
        /// </summary>
        /// <param name="args">The <see cref="Pages.Api.Events.PagePropertiesEventArgs" /> instance containing the event data.</param>
        public delegate void BlogDeletedEventHandler(BlogPostEventArgs args);

        /// <summary>
        /// Occurs when blog post is created.
        /// </summary>
        public event BlogCreatedEventHandler BlogCreated;

        /// <summary>
        /// Occurs when blog post is updated.
        /// </summary>
        public event BlogUpdatedEventHandler BlogUpdated;

        /// <summary>
        /// Occurs when blog post is deleted.
        /// </summary>
        public event BlogDeletedEventHandler BlogDeleted;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogEvents" /> class.
        /// </summary>
        public BlogEvents()
        {
            PagesApiContext.Events.PageDeleted += OnPageDeleted;
        }

        /// <summary>
        /// Called when a blog is created.
        /// </summary>
        public void OnBlogCreated(BlogPost blog)
        {
            if (BlogCreated != null)
            {
                BlogCreated(new BlogPostEventArgs(blog));
            }
        }

        /// <summary>
        /// Called when a blog is created.
        /// </summary>
        public void OnBlogUpdated(BlogPost blog)
        {
            if (BlogUpdated != null)
            {
                BlogUpdated(new BlogPostEventArgs(blog));
            }
        }

        /// <summary>
        /// Called when a blog is deleted.
        /// </summary>
        public void OnBlogDeleted(BlogPost blog)
        {
            if (BlogDeleted != null)
            {
                BlogDeleted(new BlogPostEventArgs(blog));
            }
        }

        private void OnPageDeleted(Pages.Api.Events.PagePropertiesEventArgs args)
        {
            if (args.Page is BlogPost)
            {
                OnBlogDeleted((BlogPost)args.Page);
            }
        }
    }
}
