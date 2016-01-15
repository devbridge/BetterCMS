// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UndoUploadCommand.cs" company="Devbridge Group LLC">
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
using BetterCms.Core.Exceptions;

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.MediaManager.Command.Upload.UndoUpload
{
    /// <summary>
    /// Undo file upload command.
    /// </summary>
    public class UndoUploadCommand : CommandBase, ICommand<UndoUploadRequest, bool>
    {
        /// <summary>
        /// Gets or sets the media file service.
        /// </summary>
        /// <value>
        /// The media file service.
        /// </value>
        public IMediaFileService MediaFileService { get; set; }

        /// <summary>
        /// Gets or sets the media image service.
        /// </summary>
        /// <value>
        /// The media image service.
        /// </value>
        public IMediaImageService MediaImageService { get; set; }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>true</c> on success, otherwise <c>false</c></returns>
        /// <exception cref="CmsException">if media format is not supported.</exception>
        public bool Execute(UndoUploadRequest request)
        {
            if (request.Type == MediaType.File || request.Type == MediaType.Audio || request.Type == MediaType.Video)
            {
                MediaFileService.RemoveFile(request.FileId, request.Version, doNotCheckVersion: true);
            }
            else if (request.Type == MediaType.Image)
            {
                MediaImageService.RemoveImageWithFiles(request.FileId, request.Version, doNotCheckVersion: true);
            }
            else
            {
                throw new CmsException(string.Format("A given media type {0} is not supported to upload.", request.Type));
            }
            
            return true;
        }
    }
}