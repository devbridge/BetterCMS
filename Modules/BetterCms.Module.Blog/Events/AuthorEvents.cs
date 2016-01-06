using BetterCms.Module.Blog.Models;

using BetterModules.Events;

// ReSharper disable CheckNamespace
namespace BetterCms.Events
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Author events container.
    /// </summary>
    public partial class BlogEvents : EventsBase<BlogEvents>
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
