namespace BetterCms.Module.Api.Operations.MediaManager.Files.File
{
    public class GetFileModel
    {
        /// <summary>
        /// Gets or sets the file id.
        /// </summary>
        /// <value>
        /// The file id.
        /// </value>
        public System.Guid FileId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include tags to response.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include tags to response; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeTags { get; set; }
    }
}