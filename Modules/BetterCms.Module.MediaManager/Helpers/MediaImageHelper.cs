// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MediaImageHelper.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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