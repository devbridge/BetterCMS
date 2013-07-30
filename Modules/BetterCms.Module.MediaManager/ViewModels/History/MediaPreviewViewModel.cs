namespace BetterCms.Module.MediaManager.ViewModels.History
{
    /// <summary>
    /// Media preview model.
    /// </summary>
    public class MediaPreviewViewModel
    {
        /// <summary>
        /// Gets or sets the HTML for preview.
        /// </summary>
        /// <value>
        /// The HTML for preview.
        /// </value>
        public string HtmlForPreview { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("HtmlForPreview: {0}", HtmlForPreview);
        }
    }
}