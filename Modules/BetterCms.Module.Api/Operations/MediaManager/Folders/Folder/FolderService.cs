// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FolderService.cs" company="Devbridge Group LLC">
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
using System.Linq;

using BetterCms.Core.Exceptions.Api;

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.Exceptions.DataTier;

using NHibernate.Linq;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.MediaManager.Folders.Folder
{
    /// <summary>
    /// Default folder CRUD service.
    /// </summary>
    public class FolderService : Service, IFolderService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The media service.
        /// </summary>
        private readonly IMediaService mediaService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FolderService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="mediaService">The media service.</param>
        public FolderService(IRepository repository, IUnitOfWork unitOfWork, IMediaService mediaService)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.mediaService = mediaService;
        }

        /// <summary>
        /// Gets the specified folder.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>GetFolderRequest</c> with an folder.
        /// </returns>
        public GetFolderResponse Get(GetFolderRequest request)
        {
            var model =
                repository.AsQueryable<MediaFolder>(media => media.Id == request.FolderId && media.ContentType == Module.MediaManager.Models.MediaContentType.Folder)
                    .Select(
                        media =>
                        new FolderModel
                            {
                                Id = media.Id,
                                Version = media.Version,
                                CreatedBy = media.CreatedByUser,
                                CreatedOn = media.CreatedOn,
                                LastModifiedBy = media.ModifiedByUser,
                                LastModifiedOn = media.ModifiedOn,
                                Title = media.Title,
                                IsArchived = media.IsArchived,
                                Type = (MediaType)(int)media.Type,
                                ParentFolderId = media.Folder != null ? (Guid?)media.Folder.Id : null
                            }).FirstOne();

            return new GetFolderResponse { Data = model };
        }

        /// <summary>
        /// Replaces the folder or if it doesn't exist, creates it.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>PutFolderResponse</c> with a folder id.
        /// </returns>
        public PutFolderResponse Put(PutFolderRequest request)
        {
            IEnumerable<MediaFolder> parentFolderFuture = null;
            if (request.Data.ParentFolderId.HasValue)
            {
                parentFolderFuture = repository.AsQueryable<MediaFolder>()
                    .Where(c => c.Id == request.Data.ParentFolderId.Value && !c.IsDeleted)
                    .ToFuture();
            }

            var mediaFolder = repository.AsQueryable<MediaFolder>()
                .Fetch(media => media.Folder)
                .Distinct()
                .ToFuture()
                .FirstOrDefault(folder => folder.Id == request.Id);

            MediaFolder parentFolder = null;
            if (parentFolderFuture != null)
            {
                parentFolder = parentFolderFuture.First();
                if (parentFolder.Type != (Module.MediaManager.Models.MediaType)(int)request.Data.Type)
                {
                    throw new CmsApiValidationException("Parent folder type does not match to this folder type.");
                }
            }

            var createFolder = mediaFolder == null;
            if (createFolder)
            {
                mediaFolder = new MediaFolder
                                  {
                                      Id = request.Id.GetValueOrDefault(),
                                      ContentType = Module.MediaManager.Models.MediaContentType.Folder,
                                      Type = (Module.MediaManager.Models.MediaType)(int)request.Data.Type
                                  };
            }
            else if (request.Data.Version > 0)
            {
                mediaFolder.Version = request.Data.Version;
            }

            unitOfWork.BeginTransaction();

            mediaFolder.Title = request.Data.Title;
            mediaFolder.Folder = parentFolder;

            mediaFolder.PublishedOn = DateTime.Now;

            var archivedMedias = new List<Media>();
            var unarchivedMedias = new List<Media>();
            if (mediaFolder.IsArchived != request.Data.IsArchived)
            {
                if (request.Data.IsArchived)
                {
                    archivedMedias.Add(mediaFolder);
                    mediaService.ArchiveSubMedias(mediaFolder, archivedMedias);
                }
                else
                {
                    unarchivedMedias.Add(mediaFolder);
                    mediaService.UnarchiveSubMedias(mediaFolder, unarchivedMedias);
                }
            }

            mediaFolder.IsArchived = request.Data.IsArchived;

            repository.Save(mediaFolder);

            unitOfWork.Commit();

            // Fire events.
            if (createFolder)
            {
                Events.MediaManagerEvents.Instance.OnMediaFolderCreated(mediaFolder);
            }
            else
            {
                Events.MediaManagerEvents.Instance.OnMediaFolderUpdated(mediaFolder);
            }

            foreach (var archivedMedia in archivedMedias.Distinct())
            {
                Events.MediaManagerEvents.Instance.OnMediaArchived(archivedMedia);
            }

            foreach (var archivedMedia in unarchivedMedias.Distinct())
            {
                Events.MediaManagerEvents.Instance.OnMediaUnarchived(archivedMedia);
            }

            return new PutFolderResponse { Data = mediaFolder.Id };
        }

        /// <summary>
        /// Deletes the specified folder.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>DeleteFolderResponse</c> with success status.
        /// </returns>
        public DeleteFolderResponse Delete(DeleteFolderRequest request)
        {
            if (request.Data == null || request.Id.HasDefaultValue())
            {
                return new DeleteFolderResponse { Data = false };
            }

            var result = mediaService.DeleteMedia(request.Id, request.Data.Version, false);

            return new DeleteFolderResponse { Data = result };
        }
    }
}