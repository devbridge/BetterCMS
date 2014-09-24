using System;
using System.IO;

using BetterCms.Core.Exceptions;
using BetterCms.Module.MediaManager.Helpers;
using BetterCms.Module.MediaManager.Models;

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

            archivedImage.ThumbnailUri = mediaFileService.GetFileUri(MediaType.Image, folderName, ThumbnailImageFilePrefix + Path.GetFileNameWithoutExtension(fileName) + ".png");
            archivedImage.PublicThumbnailUrl = mediaFileService.GetPublicFileUrl(MediaType.Image, folderName, ThumbnailImageFilePrefix + Path.GetFileNameWithoutExtension(fileName) + ".png");
        }

        /// <summary>
        /// Sets pathes for new original image.
        /// </summary>
        /// <param name="newOriginalImage">The new original image object.</param>
        /// <param name="folderName">The folder name.</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="archivedImageOriginalUri">The original Uri for archived image.</param>
        /// <param name="archivedImagePublicOriginalUrl">The public original Url for archived image.</param>
        public void SetPathForNewOriginal(MediaImage newOriginalImage, string folderName, string fileName, Uri archivedImageOriginalUri = null, string archivedImagePublicOriginalUrl = "")
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

            newOriginalImage.ThumbnailUri = mediaFileService.GetFileUri(MediaType.Image, folderName, ThumbnailImageFilePrefix + Path.GetFileNameWithoutExtension(fileName) + ".png");
            newOriginalImage.PublicThumbnailUrl = mediaFileService.GetPublicFileUrl(MediaType.Image, folderName, ThumbnailImageFilePrefix + Path.GetFileNameWithoutExtension(fileName) + ".png");
        }
    }
}