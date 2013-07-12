using System;

using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.Vimeo.Models
{
    [Serializable]
    public class Video : MediaFile
    {
        public virtual string VideoId { get; set; }

        public virtual string ThumbnailUrl { get; set; }

        public virtual string AuthorName { get; set; }

        public virtual string AuthorUrl { get; set; }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public virtual Video Clone()
        {
            return CopyDataTo(new Video());
        }

        /// <summary>
        /// Copies the data to.
        /// </summary>
        /// <param name="media">The media.</param>
        /// <returns></returns>
        public virtual Video CopyDataTo(Video media)
        {
            var copy = (Video)base.CopyDataTo(media);

            copy.ThumbnailUrl = ThumbnailUrl;
            copy.AuthorName = AuthorName;
            copy.AuthorUrl = AuthorUrl;

            return copy;
        }
    }
}