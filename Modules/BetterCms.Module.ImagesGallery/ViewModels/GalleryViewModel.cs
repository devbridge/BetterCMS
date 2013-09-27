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
    }
}