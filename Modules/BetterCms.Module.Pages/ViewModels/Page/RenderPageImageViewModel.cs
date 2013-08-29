namespace BetterCms.Module.Pages.ViewModels.Page
{
    public class RenderPageImageViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderPageImageViewModel" /> class.
        /// </summary>
        /// <param name="image">The image.</param>
        public RenderPageImageViewModel(MediaManager.Models.MediaImage image = null)
        {
            if (image != null)
            {
                Id = image.Id;
                Version = image.Version;
                Title = image.Title;
                Caption = image.Caption;
                PublicUrl = image.PublicUrl;
                PublicThumbnailUrl = image.PublicThumbnailUrl;
                Height = image.Height;
                Width = image.Width;
            }
        }

        /// <summary>
        /// Gets or sets the image id.
        /// </summary>
        /// <value>
        /// The image id.
        /// </value>
        public System.Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the image version.
        /// </summary>
        /// <value>
        /// The image version.
        /// </value>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the image title.
        /// </summary>
        /// <value>
        /// The image title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the image  caption.
        /// </summary>
        /// <value>
        /// The image caption.
        /// </value>
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets the image size.
        /// </summary>
        /// <value>
        /// The image size.
        /// </value>
        public string Size { get; set; }

        /// <summary>
        /// Gets or sets the image public URL.
        /// </summary>
        /// <value>
        /// The image  public URL.
        /// </value>
        public string PublicUrl { get; set; }

        /// <summary>
        /// Gets or sets the image thumbnail public URL.
        /// </summary>
        /// <value>
        /// The image thumbnail public URL.
        /// </value>
        public string PublicThumbnailUrl { get; set; }

        /// <summary>
        /// Gets or sets the image width.
        /// </summary>
        /// <value>
        /// The image width.
        /// </value>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the image height.
        /// </summary>
        /// <value>
        /// The image height.
        /// </value>
        public int Height { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, Id: {1}, Version: {2}, Title: {3}, Caption: {4}", base.ToString(), Id, Version, Title, Caption);
        }
    }
}