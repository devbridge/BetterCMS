// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFilesService.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Api.Operations.MediaManager.Files.File;

namespace BetterCms.Module.Api.Operations.MediaManager.Files
{
    /// <summary>
    /// Image service contract for REST.
    /// </summary>
    public interface IFilesService
    {
        /// <summary>
        /// Gets the upload file service.
        /// </summary>
        IUploadFileService Upload { get; }

        /// <summary>
        /// Gets files list.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetFilesResponse</c> with files list.</returns>
        GetFilesResponse Get(GetFilesRequest request);

        // NOTE: do not implement: replaces all the files.
        // PutTagsResponse Put(PutTagsRequest request);

        /// <summary>
        /// Creates a new file.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PostFilesResponse</c> with a new file id.</returns>
        PostFileResponse Post(PostFileRequest request);

        // NOTE: do not implement: drops all the files.
        // DeleteFilesResponse Delete(DeleteFilesRequest request);
    }
}
