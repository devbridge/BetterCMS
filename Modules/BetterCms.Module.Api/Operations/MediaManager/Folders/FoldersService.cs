// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FoldersService.cs" company="Devbridge Group LLC">
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
using System.Linq;

using BetterModules.Core.DataAccess;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.MediaManager.Folders.Folder;
using BetterCms.Module.MediaManager.Models;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.MediaManager.Folders
{
    /// <summary>
    /// Default folders service contract implementation for REST.
    /// </summary>
    public class FoldersService : Service, IFoldersService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The folder service.
        /// </summary>
        private readonly IFolderService folderService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FoldersService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public FoldersService(IRepository repository, IFolderService folderService)
        {
            this.repository = repository;
            this.folderService = folderService;
        }

        /// <summary>
        /// Gets folder list.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>GetFoldersResponse</c> with folder list.
        /// </returns>
        public GetFoldersResponse Get(GetFoldersRequest request)
        {
            request.Data.SetDefaultOrder("Title");

            var query = this.repository
                .AsQueryable<Media>()
                .Where(m => m.Original == null && m.ContentType == Module.MediaManager.Models.MediaContentType.Folder)
                .Where(f => f is MediaFolder);

            query = request.Data.ParentFolderId == null
                ? query.Where(m => m.Folder == null)
                : query.Where(m => m.Folder.Id == request.Data.ParentFolderId && !m.Folder.IsDeleted);

            if (!request.Data.IncludeArchived)
            {
                query = query.Where(m => !m.IsArchived);
            }

            var listResponse = query.Select(media =>
                    new FolderModel
                        {
                            Id = media.Id,
                            Version = media.Version,
                            CreatedBy = media.CreatedByUser,
                            CreatedOn = media.CreatedOn,
                            LastModifiedBy = media.ModifiedByUser,
                            LastModifiedOn = media.ModifiedOn,

                            Title = media.Title,
                            Type = (MediaType)((int)media.Type),
                            IsArchived = media.IsArchived
                        })
                        .ToDataListResponse(request);

            return new GetFoldersResponse
                       {
                           Data = listResponse
                       };
        }

        /// <summary>
        /// Creates a new folder.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>PostFolderResponse</c> with a new folder id.
        /// </returns>
        public PostFolderResponse Post(PostFolderRequest request)
        {
            var result =
                folderService.Put(
                    new PutFolderRequest
                    {
                        Data = request.Data,
                        User = request.User
                    });

            return new PostFolderResponse { Data = result.Data };
        }
    }
}