using System.IO;

namespace BetterCms.Module.MediaManager.Command.Files.DownloadFile
{
    /// <summary>
    /// Response data for download command.
    /// </summary>
    public class DownloadFileCommandResponse
    {
        /// <summary>
        /// Gets or sets the file stream.
        /// </summary>
        /// <value>
        /// The file stream.
        /// </value>
        public Stream FileStream { get; set; }

        /// <summary>
        /// Gets or sets the type of the content.
        /// </summary>
        /// <value>
        /// The type of the content.
        /// </value>
        public string ContentMimeType { get; set; }

        /// <summary>
        /// Gets or sets the name of the file download.
        /// </summary>
        /// <value>
        /// The name of the file download.
        /// </value>
        public string FileDownloadName { get; set; }

        /// <summary>
        /// Gets or sets the redirect URL.
        /// </summary>
        /// <value>
        /// The redirect URL.
        /// </value>
        public string RedirectUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has no access.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has no access; otherwise, <c>false</c>.
        /// </value>
        public bool HasNoAccess { get; set; }
    }
}