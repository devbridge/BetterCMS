using System;
using System.Collections.Generic;

namespace BetterCms.Module.ImagesGallery.ViewModels
{
    public class AlbumViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AlbumViewModel" /> class.
        /// </summary>
        public AlbumViewModel()
        {
            Images = new List<ImageViewModel>();
        }

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
        /// Gets or sets a value indicating whether header should be rendered.
        /// </summary>
        /// <value>
        ///   <c>true</c> if header should be rendered; otherwise, <c>false</c>.
        /// </value>
        public bool RenderHeader { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to render back URL.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to render back URL; otherwise, <c>false</c>.
        /// </value>
        public bool RenderBackUrl { get; set; }

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
            return string.Format("Title: {0}, Url: {1}, ImagesCount: {2}", Title, Url, ImagesCount);
        }
    }
}