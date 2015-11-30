// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultMediaImageVersionPathService.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.MediaManager.Helpers;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Models.Enum;

namespace BetterCms.Module.MediaManager.Services
{

    /// <summary>
    /// Service for correct pathes setting.
    /// </summary>
    public class DefaultMediaImageVersionPathService : IMediaImageVersionPathService
    {
        /// <summary>
        /// The thumbnail image file prefix.
        /// </summary>
        private const string ThumbnailImageFilePrefix = "t_";

        private readonly IMediaFileService mediaFileService;

        public DefaultMediaImageVersionPathService(IMediaFileService mediaFileService)
        {
            this.mediaFileService = mediaFileService;
        }

        /// <summary>
        /// Sets pathes for archive image.
        /// </summary>
        /// <param name="archivedImage">The archived image object.</param>
        /// <param name="folderName">The folder name.</param>
        /// <param name="fileName">The file name.</param>
        public void SetPathForArchive(MediaImage archivedImage, string folderName, string fileName)
        {
            archivedImage.FileUri = mediaFileService.GetFileUri(MediaType.Image, folderName, fileName);
            archivedImage.PublicUrl = mediaFileService.GetPublicFileUrl(MediaType.Image, folderName, fileName);

            if (!archivedImage.IsEdited())
            {
                archivedImage.OriginalUri = mediaFileService.GetFileUri(MediaType.Image, folderName, MediaImageHelper.OriginalImageFilePrefix + fileName);
                archivedImage.PublicOriginallUrl = mediaFileService.GetPublicFileUrl(MediaType.Image, folderName, MediaImageHelper.OriginalImageFilePrefix + fileName);
            }
            var imageType = ImageHelper.GetImageType(archivedImage.OriginalFileExtension);
            if (imageType == ImageType.Raster)
            {
                archivedImage.ThumbnailUri = mediaFileService.GetFileUri(MediaType.Image, folderName, ThumbnailImageFilePrefix + Path.GetFileNameWithoutExtension(fileName) + ".png");
                archivedImage.PublicThumbnailUrl = mediaFileService.GetPublicFileUrl(MediaType.Image, folderName, ThumbnailImageFilePrefix + Path.GetFileNameWithoutExtension(fileName) + ".png");
            }
            else
            {
                archivedImage.ThumbnailUri = mediaFileService.GetFileUri(MediaType.Image, folderName, ThumbnailImageFilePrefix + Path.GetFileName(fileName));
                archivedImage.PublicThumbnailUrl = mediaFileService.GetPublicFileUrl(MediaType.Image, folderName, ThumbnailImageFilePrefix + Path.GetFileName(fileName));
            }
        }

        /// <summary>
        /// Sets pathes for new original image.
        /// </summary>
        /// <param name="newOriginalImage">The new original image object.</param>
        /// <param name="folderName">The folder name.</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="archivedImageOriginalUri">The original Uri for archived image.</param>
        /// <param name="archivedImagePublicOriginalUrl">The public original Url for archived image.</param>
        public void SetPathForNewOriginal(MediaImage newOriginalImage, string folderName, string fileName, ImageType imageType, Uri archivedImageOriginalUri = null, string archivedImagePublicOriginalUrl = "")
        {
            newOriginalImage.FileUri = mediaFileService.GetFileUri(MediaType.Image, folderName, fileName);
            newOriginalImage.PublicUrl = mediaFileService.GetPublicFileUrl(MediaType.Image, folderName, fileName);

            
            if (!newOriginalImage.IsEdited())
            {
                newOriginalImage.OriginalUri = mediaFileService.GetFileUri(MediaType.Image, folderName, MediaImageHelper.OriginalImageFilePrefix + fileName);
                newOriginalImage.PublicOriginallUrl = mediaFileService.GetPublicFileUrl(MediaType.Image, folderName, MediaImageHelper.OriginalImageFilePrefix + fileName);
            }
            else
            {
                if(archivedImageOriginalUri == null || archivedImagePublicOriginalUrl == null)
                    throw new CmsException("Not valid Url or Uri for original image");

                newOriginalImage.OriginalUri = archivedImageOriginalUri;
                newOriginalImage.PublicOriginallUrl = archivedImagePublicOriginalUrl;
            }
            if (imageType == ImageType.Raster)
            {
                newOriginalImage.ThumbnailUri = mediaFileService.GetFileUri(MediaType.Image, folderName, ThumbnailImageFilePrefix + Path.GetFileNameWithoutExtension(fileName) + ".png");
                newOriginalImage.PublicThumbnailUrl = mediaFileService.GetPublicFileUrl(MediaType.Image, folderName, ThumbnailImageFilePrefix + Path.GetFileNameWithoutExtension(fileName) + ".png");
            }
            else
            {
                newOriginalImage.ThumbnailUri = mediaFileService.GetFileUri(MediaType.Image, folderName, ThumbnailImageFilePrefix + Path.GetFileName(fileName));
                newOriginalImage.PublicThumbnailUrl = mediaFileService.GetPublicFileUrl(MediaType.Image, folderName, ThumbnailImageFilePrefix + Path.GetFileName(fileName));
            }
        }
    }
}