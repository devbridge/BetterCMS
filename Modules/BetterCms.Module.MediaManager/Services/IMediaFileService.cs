// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMediaFileService.cs" company="Devbridge Group LLC">
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
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using BetterCms.Module.MediaManager.Models;

using NHibernate;

namespace BetterCms.Module.MediaManager.Services
{
    public interface IMediaFileService
    {
        void RemoveFile(Guid fileId, int version, bool doNotCheckVersion = false);

        MediaFile UploadFile(MediaType type, Guid rootFolderId, string fileName, long fileLength, Stream fileStream,
            bool isTemporary = true, string title = "", string description = "");

        MediaFile UploadFileWithStream(MediaType type, Guid rootFolderId, string fileName, long fileLength, Stream fileStream,
            bool WaitForUploadResult = false, string title = "", string description = "", Guid? reuploadMediaId = null);

        string CreateRandomFolderName();

        Uri GetFileUri(MediaType type, string folderName, string fileName);

        string GetPublicFileUrl(MediaType type, string folderName, string fileName);

        Task UploadMediaFileToStorageAsync<TMedia>(Stream sourceStream, Uri fileUri, Guid mediaId, Action<TMedia, ISession> updateMediaAfterUpload, Action<TMedia> updateMediaAfterFail, bool ignoreAccessControl) where TMedia : MediaFile;
        
        void UploadMediaFileToStorageSync<TMedia>(Stream sourceStream, Uri fileUri, TMedia media, Action<TMedia> updateMediaAfterUpload, Action<TMedia> updateMediaAfterFail, bool ignoreAccessControl) where TMedia : MediaFile;

        string GetDownloadFileUrl(MediaType type, Guid id, string fileUrl);

        void SaveMediaFile(MediaFile file);

        void SwapOriginalMediaWithVersion(MediaFile originalEntity, MediaFile newVersion, ISession session = null);

        void MoveFilesToTrashFolder();
    }
}