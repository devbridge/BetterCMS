
using BetterCms.Api;
using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.MediaManager.Api.Events
{
    /// <summary>
    /// Attachable page events container
    /// </summary>
    public class MediaManagerEvents : EventsBase
    {
        /// <summary>
        /// Occurs when a media file is uploaded.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<MediaFile>> MediaFileUploaded;

        /// <summary>
        /// Occurs when a media file is updated.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<MediaFile>> MediaFileUpdated;

        /// <summary>
        /// Occurs when a media file is deleted.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<MediaFile>> MediaFileDeleted;

        /// <summary>
        /// Occurs when a media folder is created.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<MediaFolder>> MediaFolderCreated;

        /// <summary>
        /// Occurs when a media folder is updated.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<MediaFolder>> MediaFolderUpdated;

        /// <summary>
        /// Occurs when a media folder is deleted.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<MediaFolder>> MediaFolderDeleted;
        
        /// <summary>
        /// Called when a blog is created.
        /// </summary>
        public void OnMediaFileUploaded(MediaFile mediaFile)
        {
            if (MediaFileUploaded != null)
            {
                MediaFileUploaded(new SingleItemEventArgs<MediaFile>(mediaFile));
            }
        }

        public void OnMediaFileDeleted(MediaFile mediaFile)
        {
            if (MediaFileDeleted != null)
            {
                MediaFileDeleted(new SingleItemEventArgs<MediaFile>(mediaFile));
            }
        }

        public void OnMediaFileUpdated(MediaFile mediaFile)
        {
            if (MediaFileUpdated != null)
            {
                MediaFileUpdated(new SingleItemEventArgs<MediaFile>(mediaFile));
            }
        }

        public void OnMediaFolderCreated(MediaFolder mediaFolder)
        {
            if (MediaFolderCreated != null)
            {
                MediaFolderCreated(new SingleItemEventArgs<MediaFolder>(mediaFolder));
            }
        }

        public void OnMediaFolderUpdated(MediaFolder mediaFolder)
        {
            if (MediaFolderUpdated != null)
            {
                MediaFolderUpdated(new SingleItemEventArgs<MediaFolder>(mediaFolder));
            }
        }

        public void OnMediaFolderDeleted(MediaFolder mediaFolder)
        {
            if (MediaFolderDeleted != null)
            {
                MediaFolderDeleted(new SingleItemEventArgs<MediaFolder>(mediaFolder));
            }
        }
    }
}
