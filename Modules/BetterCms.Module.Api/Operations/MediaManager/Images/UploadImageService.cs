// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UploadImageService.cs" company="Devbridge Group LLC">
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
using System.IO;
using System.Linq;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Core.Exceptions.Api;

using BetterCms.Module.Api.Operations.MediaManager.Images.Image;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Services;

namespace BetterCms.Module.Api.Operations.MediaManager.Images
{
    /// <summary>
    /// The upload image service.
    /// </summary>
    public class UploadImageService: IUploadImageService
    {
        private readonly IRepository repository;

        private readonly IMediaImageService mediaImageService;

        private readonly ICmsConfiguration configuration;

        public UploadImageService(IRepository repository, IMediaImageService mediaImageService, ICmsConfiguration configuration)
        {
            this.repository = repository;
            this.mediaImageService = mediaImageService;
            this.configuration = configuration;
        }

        /// <summary>
        /// Upload image from the stream.
        /// </summary>
        /// <param name="request">The upload image request.</param>
        /// <returns>The upload image response.</returns>
        public UploadImageResponse Post(UploadImageRequest request)
        {
            MediaFolder parentFolder = null;
            if (request.Data.FolderId.HasValue)
            {
                parentFolder = repository.AsQueryable<MediaFolder>()
                    .Where(c => c.Id == request.Data.FolderId.Value && !c.IsDeleted)
                    .FirstOne();

                if (parentFolder.Type != Module.MediaManager.Models.MediaType.Image)
                {
                    throw new CmsApiValidationException("Folder must be type of an image.");
                }
            }

            var maxLength = configuration.Storage.MaximumFileNameLength > 0 ? configuration.Storage.MaximumFileNameLength : 100;
            // Fix for IIS express + IE (if full path is returned)
            var fileName = Path.GetFileName(request.Data.FileName);
            if (fileName.Length > maxLength)
            {
                fileName = string.Concat(Path.GetFileNameWithoutExtension(fileName.Substring(0, maxLength)), Path.GetExtension(fileName));
            }

            var mediaImage = new MediaImage
            {
                Id = request.Data.Id.GetValueOrDefault(),
                Type = Module.MediaManager.Models.MediaType.Image,
                Caption = request.Data.Caption,
                Title = request.Data.Title ?? fileName,
                Description = request.Data.Description,
                Size = request.Data.FileStream.Length,
                Folder = parentFolder,
                OriginalFileName = fileName,
                OriginalFileExtension = Path.GetExtension(fileName)
            };

            var savedImage = mediaImageService.UploadImageWithStream(request.Data.FileStream, mediaImage, request.Data.WaitForUploadResult);

            if (savedImage != null)
            {
                Events.MediaManagerEvents.Instance.OnMediaFileUploaded(savedImage);
            }

            return new UploadImageResponse
            {
                Data = savedImage.Id,
            };
        }
    }
}