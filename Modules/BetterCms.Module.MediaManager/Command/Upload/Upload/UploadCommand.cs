// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UploadCommand.cs" company="Devbridge Group LLC">
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

using BetterCms.Core.Exceptions;

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.MediaManager.Command.Upload.Upload
{
    public class UploadCommand : CommandBase, ICommand<UploadFileRequest, MediaFile>
    {
        public IMediaFileService MediaFileService { get; set; }

        public IMediaImageService MediaImageService { get; set; }

        public IMediaVideoService MediaVideoService { get; set; }

        public IMediaAudioService MediaAudioService { get; set; }
        
        public ICmsConfiguration CmsConfiguration { get; set; }

        public MediaFile Execute(UploadFileRequest request)
        {
            var maxLength = CmsConfiguration.Storage.MaximumFileNameLength > 0 ? CmsConfiguration.Storage.MaximumFileNameLength : 100;

            // Fix for IIS express + IE (if full path is returned)
            var fileName = Path.GetFileName(request.FileName);
            if (fileName.Length > maxLength)
            {
                fileName = string.Concat(Path.GetFileNameWithoutExtension(fileName.Substring(0, maxLength)), Path.GetExtension(fileName));
            }

            if (request.Type == MediaType.File || request.Type == MediaType.Audio || request.Type == MediaType.Video)
            {
                var media = MediaFileService.UploadFile(request.Type, request.RootFolderId, fileName, request.FileLength, request.FileStream);

                Events.MediaManagerEvents.Instance.OnMediaFileUploaded(media);

                return media;
            }

            if (request.Type == MediaType.Image)
            {
                var media = MediaImageService.UploadImage(
                    request.RootFolderId,
                    fileName,
                    request.FileLength,
                    request.FileStream,
                    request.ReuploadMediaId,
                    request.ShouldOverride);
                
                Events.MediaManagerEvents.Instance.OnMediaFileUploaded(media);

                return media;
            }

            throw new CmsException(string.Format("A given media type {0} is not supported to upload.", request.Type));
        }
    }

}