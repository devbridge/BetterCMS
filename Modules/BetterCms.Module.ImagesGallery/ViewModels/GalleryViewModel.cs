using System.Collections.Generic;

namespace BetterCms.Module.ImagesGallery.ViewModels
{
    public class GalleryViewModel
    {
        /// <summary>
        /// Gets or sets the albums.
        /// </summary>
        /// <value>
        /// The albums.
        /// </value>
        public IList<AlbumViewModel> Albums { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to load CMS styles.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to load CMS styles; otherwise, <c>false</c>.
        /// </value>
        public bool LoadCmsStyles { get; set; }

        /// <summary>
        /// Gets or sets the count of images per section.
        /// </summary>
        /// <value>
        /// The count of images per section.
        /// </value>
        public int ImagesPerSection { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("AlbumsCount: {0}, LoadCmsStyles: {1}", Albums != null ? Albums.Count : 0, LoadCmsStyles);
        }
    }
}