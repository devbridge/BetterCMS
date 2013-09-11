namespace BetterCms.Module.MediaManager.ViewModels.History
{
    /// <summary>
    /// View model for rendering media version properties
    /// </summary>
    public class MediaPreviewPropertyViewModel
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether property is URL.
        /// </summary>
        /// <value>
        ///   <c>true</c> if property is URL; otherwise, <c>false</c>.
        /// </value>
        public bool IsUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether property is image URL.
        /// </summary>
        /// <value>
        ///   <c>true</c> if property is image URL; otherwise, <c>false</c>.
        /// </value>
        public bool IsImageUrl { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, Title: {1}, Value: {2}, IsUrl: {3}, IsImageUrl: {4}", Title, Value, IsUrl, IsImageUrl);
        }
    }
}