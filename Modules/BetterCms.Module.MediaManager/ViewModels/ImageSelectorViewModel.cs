using System;

namespace BetterCms.Module.MediaManager.ViewModels
{
    public class ImageSelectorViewModel
    {
        /// <summary>
        /// Gets or sets the image id.
        /// </summary>
        /// <value>
        /// The image id.
        /// </value>
        public virtual Guid? ImageId { get; set; }
        
        /// <summary>
        /// Gets or sets the folder id.
        /// </summary>
        /// <value>
        /// The folder id.
        /// </value>
        public virtual Guid? FolderId { get; set; }

        /// <summary>
        /// Gets or sets the image version.
        /// </summary>
        /// <value>
        /// The image version.
        /// </value>
        public virtual int ImageVersion { get; set; }

        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>
        /// <value>
        /// The image URL.
        /// </value>
        public virtual string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the thumbnail URL.
        /// </summary>
        /// <value>
        /// The thumbnail URL.
        /// </value>
        public virtual string ThumbnailUrl { get; set; }

        /// <summary>
        /// Gets or sets the image tooltip.
        /// </summary>
        /// <value>
        /// The image tooltip.
        /// </value>
        public virtual string ImageTooltip { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("ImageId: {0}, Url: {1}, Thumbnail: {2}", ImageId, ImageUrl, ThumbnailUrl);
        }
    }
}