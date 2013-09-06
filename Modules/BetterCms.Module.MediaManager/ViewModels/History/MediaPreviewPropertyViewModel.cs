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
    }
}