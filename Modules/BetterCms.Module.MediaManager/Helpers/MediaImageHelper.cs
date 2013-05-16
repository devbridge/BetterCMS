using System;
using System.IO;

namespace BetterCms.Module.MediaManager.Helpers
{
    public static class MediaImageHelper
    {
        /// <summary>
        /// The image file name pattern containing version number.
        /// </summary>
        public const string VersionedImageFileNamePattern = "{0}_{1}.{2}";

        /// <summary>
        /// The original image file prefix.
        /// </summary>
        public const string OriginalImageFilePrefix = "o_";

        /// <summary>
        /// Creates the file name of the versioned file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="version">The version.</param>
        /// <returns> Updated filename </returns>
        public static string CreateVersionedFileName(string fileName, int version)
        {
            var extension = Path.GetExtension(fileName);
            if (!string.IsNullOrWhiteSpace(extension))
            {
                fileName = fileName.Substring(0, fileName.LastIndexOf(extension, StringComparison.InvariantCulture));
                extension = extension.Trim('.');
            }

            return string.Format(VersionedImageFileNamePattern, fileName, version, extension);
        }
    }
}