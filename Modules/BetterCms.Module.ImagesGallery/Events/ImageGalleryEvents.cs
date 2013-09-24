using BetterCms.Module.ImagesGallery.Models;

// ReSharper disable CheckNamespace
namespace BetterCms.Events
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Attachable image gallery events container
    /// </summary>
    public class ImageGalleryEvents : EventsBase<ImageGalleryEvents>
    {
        /// <summary>
        /// Occurs when an album is created.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Album>> AlbumCreated;

        /// <summary>
        /// Occurs when an album is updated.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Album>> AlbumUpdated;

        /// <summary>
        /// Occurs when an album is deleted.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Album>> AlbumDeleted;

        /// <summary>
        /// Called when album is created.
        /// </summary>
        /// <param name="album">The album.</param>
        public void OnAlbumCreated(Album album)
        {
            if (AlbumCreated != null)
            {
                AlbumCreated(new SingleItemEventArgs<Album>(album));
            }
        }
        
        /// <summary>
        /// Called when album is updates.
        /// </summary>
        /// <param name="album">The album.</param>
        public void OnAlbumUpdated(Album album)
        {
            if (AlbumUpdated != null)
            {
                AlbumUpdated(new SingleItemEventArgs<Album>(album));
            }
        }

        /// <summary>
        /// Called when album is deleted.
        /// </summary>
        /// <param name="album">The album.</param>
        public void OnAlbumDeleted(Album album)
        {
            if (AlbumDeleted != null)
            {
                AlbumDeleted(new SingleItemEventArgs<Album>(album));
            }
        }
    }
}
