namespace BetterCms.Module.Viddler.Services.Models
{
    /// <summary>
    /// Video details model.
    /// </summary>
    public class Video
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the thumbnail URL.
        /// </summary>
        /// <value>
        /// The thumbnail URL.
        /// </value>
        public string ThumbnailUrl { get; set; }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        public long Length { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is ready.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is ready; otherwise, <c>false</c>.
        /// </value>
        public bool IsReady { get; set; }

        /// <summary>
        /// Gets or sets the view count.
        /// </summary>
        /// <value>
        /// The view count.
        /// </value>
        public long ViewCount { get; set; }
    }
}