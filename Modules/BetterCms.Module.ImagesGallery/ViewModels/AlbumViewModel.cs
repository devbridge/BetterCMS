using System;

namespace BetterCms.Module.ImagesGallery.ViewModels
{
    public class AlbumViewModel
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the images count.
        /// </summary>
        /// <value>
        /// The images count.
        /// </value>
        public int ImagesCount { get; set; }

        /// <summary>
        /// Gets or sets the last update date.
        /// </summary>
        /// <value>
        /// The last update date.
        /// </value>
        public DateTime LastUpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the cover image URL.
        /// </summary>
        /// <value>
        /// The cover image URL.
        /// </value>
        public string CoverImageUrl { get; set; }
    }
}