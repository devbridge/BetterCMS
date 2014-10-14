using System;
using System.IO;

using BetterCms.Core.Exceptions;

namespace BetterCms.Module.MediaManager.Helpers
{
    public static class MediaImageHelper
    {
        /// <summary>
        /// The image file name pattern containing version number.
        /// </summary>
        public const string VersionedImageFileNamePattern = "{0}_{1}.{2}";

        /// <summary>
        /// The image file name pattern containing version number.
        /// </summary>
        public const string OverridedImageFileNamePattern = "{0}_{1}_{2}.{3}";

        /// <summary>
        /// The image file name pattern containing version number without  extension.
        /// </summary>
        public const string VersionedImageFileWithoutExtensionNamePattern = "{0}_{1}";

        /// <summary>
        /// The image file name pattern containing version number without  extension.
        /// </summary>
        public const string OverridedImageFileWithoutExtensionNamePattern = "{0}_{1}_{2}";

        public const string HistoricalVersionedFileNamePattern = "{0}.{1}";

        /// <summary>
        /// The public image file name pattern.
        /// </summary>
        public const string PublicImageFileNamePattern = "{0}.{1}";

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

        /// <summary>
        /// Creates the file name of the versioned file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="extension">Extension of the file.</param>
        /// <param name="version">The version.</param>
        /// <returns> Updated filename </returns>
        public static string CreateVersionedFileName(string fileName, string extension, int version)
        {
            var currentExtension = Path.GetExtension(fileName);
            if (!string.IsNullOrWhiteSpace(currentExtension))
            {
                fileName = fileName.Substring(0, fileName.LastIndexOf(currentExtension, StringComparison.InvariantCulture));
            }

            if (!string.IsNullOrWhiteSpace(extension))
            {
                extension = extension.Trim('.');
                return string.Format(VersionedImageFileNamePattern, fileName, version, extension);
            }

            return string.Format(VersionedImageFileWithoutExtensionNamePattern, fileName, version);
        }

        public static string CreateHistoricalVersionedFileName(string fileName, string extension)
        {
            var currentExtension = Path.GetExtension(fileName);
            if (!string.IsNullOrWhiteSpace(currentExtension))
            {
                currentExtension = currentExtension.Trim('.');
                return string.Format(HistoricalVersionedFileNamePattern, Guid.NewGuid().ToString("N"), currentExtension);
            }

            if (!string.IsNullOrWhiteSpace(extension))
            {
                extension = extension.Trim('.');
                return string.Format(HistoricalVersionedFileNamePattern, Guid.NewGuid().ToString("N"), extension);
            }

            throw new CmsException("Extension cann't be null or empty");
        }

        public static string CreateNotOverridedFileName(string fileName, string extension, int version)
        {
            var currentExtension = Path.GetExtension(fileName);
            if (!string.IsNullOrWhiteSpace(currentExtension))
            {
                fileName = fileName.Substring(0, fileName.LastIndexOf(currentExtension, StringComparison.InvariantCulture));
            }

            if (!string.IsNullOrWhiteSpace(extension))
            {
                extension = extension.Trim('.');
                return string.Format(OverridedImageFileNamePattern, fileName, version, "no", extension);
            }

            return string.Format(OverridedImageFileWithoutExtensionNamePattern, fileName, version, "no");
        }

        /// <summary>
        /// Creates public file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="extension">Extension of the file.</param>
        /// <returns>Public file name.</returns>
        public static string CreatePublicFileName(string fileName, string extension)
        {
            var currentExtension = Path.GetExtension(fileName);
            if (!string.IsNullOrWhiteSpace(currentExtension))
            {
                fileName = fileName.Substring(0, fileName.LastIndexOf(currentExtension, StringComparison.InvariantCulture));
            }

            if (!string.IsNullOrWhiteSpace(extension))
            {
                extension = extension.Trim('.');
                return string.Format(PublicImageFileNamePattern, fileName, extension);
            }

            return fileName;
        }
    }
}