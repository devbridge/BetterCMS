using BetterCms.Api;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.Pages.Models;

namespace BetterCms.Module.Blog.Api.Events
{
    /// <summary>
    /// Attachable blog events container
    /// </summary>
    public partial class BlogsApiEvents : EventsBase
    {
        /// <summary>
        /// Occurs when blog post is created.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<BlogPost>> BlogCreated;

        /// <summary>
        /// Occurs when blog post is updated.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<BlogPost>> BlogUpdated;

        /// <summary>
        /// Occurs when blog post is deleted.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<BlogPost>> BlogDeleted;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogEvents" /> class.
        /// </summary>
        public BlogsApiEvents()
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
                BlogCreated(new SingleItemEventArgs<BlogPost>(blog));
            }
        }

        /// <summary>
        /// Called when a blog is created.
        /// </summary>
        public void OnBlogUpdated(BlogPost blog)
        {
            if (BlogUpdated != null)
            {
                BlogUpdated(new SingleItemEventArgs<BlogPost>(blog));
            }
        }

        /// <summary>
        /// Called when a blog is deleted.
        /// </summary>
        public void OnBlogDeleted(BlogPost blog)
        {
            if (BlogDeleted != null)
            {
                BlogDeleted(new SingleItemEventArgs<BlogPost>(blog));
            }
        }

        private void OnPageDeleted(SingleItemEventArgs<PageProperties> args)
        {
            if (args != null && args.Item is BlogPost)
            {
                OnBlogDeleted((BlogPost)args.Item);
            }
        }
    }
}
