
using BetterCms.Module.MediaManager.Api.Events;

namespace BetterCms.Module.Navigation.Api.Events
{
    /// <summary>
    /// Attachable page events container
    /// </summary>
    public class MediaManagerEvents
    {
        /// <summary>
        /// Delegate to handle a blog post creation event.
        /// </summary>
        public delegate void NewMediaUploadedEventHandler(MediaEventArgs args);

        /// <summary>
        /// Occurs when blog post is created.
        /// </summary>
        public event NewMediaUploadedEventHandler NewMediaUploaded;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaManagerEvents" /> class.
        /// </summary>
        public MediaManagerEvents()
        {
            
        }

        /// <summary>
        /// Called when a blog is created.
        /// </summary>
        public void OnNewMediaUploaded(Media media)
        {
            if (NewMediaUploaded != null)
            {
                NewMediaUploaded(new MediaEventArgs(media));
            }
        }      
    }
}
