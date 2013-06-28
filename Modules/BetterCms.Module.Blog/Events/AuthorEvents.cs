using BetterCms.Api;
using BetterCms.Module.Blog.Models;

namespace BetterCms.Module.Blog.Events
{
    /// <summary>
    /// Author events container.
    /// </summary>
    public partial class BlogsApiEvents : EventsBase
    {
        /// <summary>
        /// Occurs when an author is created.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Author>> AuthorCreated;

        /// <summary>
        /// Occurs when an author is updated.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Author>> AuthorUpdated;

        /// <summary>
        /// Occurs when an author is deleted.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Author>> AuthorDeleted;
        
        public void OnAuthorCreated(Author author)
        {
            if (AuthorCreated != null)
            {
                AuthorCreated(new SingleItemEventArgs<Author>(author));
            }
        }
        
        public void OnAuthorUpdated(Author author)
        {
            if (AuthorUpdated != null)
            {
                AuthorUpdated(new SingleItemEventArgs<Author>(author));
            }
        }

        public void OnAuthorDeleted(Author author)
        {
            if (AuthorDeleted != null)
            {
                AuthorDeleted(new SingleItemEventArgs<Author>(author));
            }
        }
    }
}
