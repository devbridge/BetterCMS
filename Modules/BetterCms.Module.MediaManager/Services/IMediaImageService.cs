// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMediaImageService.cs" company="Devbridge Group LLC">
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
using System.Drawing;
using System.IO;

using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.MediaManager.Services
{
    public interface IMediaImageService
    {
        MediaImage UploadImage(Guid rootFolderId, string fileName, long fileLength, Stream file, Guid reuploadMediaId, bool overrideUrl = true);

        MediaImage UploadImageWithStream(Stream fileStream, MediaImage image, bool waitForUploadResult = false);

        void RemoveImageWithFiles(Guid mediaImageId, int version, bool doNotCheckVersion = false, bool originalWasNotUploaded = false);

        void UpdateThumbnail(MediaImage mediaImage, Size size);

        MediaImage MakeAsOriginal(MediaImage image, MediaImage originalImage, MediaImage archivedImage, bool overrideUrl = true);

        void SaveEditedImage(MediaImage image, MediaImage archivedImage, MemoryStream croppedImageFileStream, bool overrideUrl = true);

        void SaveImage(MediaImage image);

        MediaImage MoveToHistory(MediaImage originalImage);
    }
}