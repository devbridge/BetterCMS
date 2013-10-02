using System;
using System.Collections.Generic;

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
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the folder id.
        /// </summary>
        /// <value>
        /// The folder id.
        /// </value>
        public Guid FolderId { get; set; }

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
        public DateTime? LastUpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the cover image URL.
        /// </summary>
        /// <value>
        /// The cover image URL.
        /// </value>
        public string CoverImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the list of image view models.
        /// </summary>
        /// <value>
        /// The list of image view models.
        /// </value>
        public List<ImageViewModel> Images { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to load CMS styles.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to load CMS styles; otherwise, <c>false</c>.
        /// </value>
        public bool LoadCmsStyles { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Title: {0}, Url: {1}, ImagesCount: {2}", Title, Url, ImagesCount);
        }
    }
}