﻿using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.MediaManager.Models;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.MediaManager.Files
{
    public class FilesService : Service, IFilesService
    {
        private readonly IRepository repository;

        public FilesService(IRepository repository)
        {
            this.repository = repository;
        }

        public GetFilesResponse Get(GetFilesRequest request)
        {
            // TODO: throw new validation exception if request.IncludeFiles == false && request.IncludeFolders == false

            request.SetDefaultOrder("Title");

            var query = repository
                .AsQueryable<Media>()
                .Where(m => m.Original == null && m.Folder.Id == request.FolderId);

            if (!request.IncludeArchived)
            {
                query = query.Where(m => !m.IsArchived);
            }

            if (request.IncludeFolders && request.IncludeFiles)
            {
                query = query.Where(media => (media is MediaFile || media is MediaFolder));
            }
            else if (!request.IncludeFolders)
            {
                query = query.Where(media => media is MediaFile);
            }
            else
            {
                query = query.Where(media => media is MediaFolder);
            }

            // TODO: filter by tags !!!

            var listResponse = query.Select(media =>
                    new MediaModel
                        {
                            Id = media.Id,
                            Version = media.Version,
                            CreatedBy = media.CreatedByUser,
                            CreatedOn = media.CreatedOn,
                            LastModifiedBy = media.ModifiedByUser,
                            LastModifiedOn = media.ModifiedOn,

                            Title = media.Title,
                            MediaContentType = media is MediaFolder ? MediaContentType.Folder : MediaContentType.File,
                            FileExtension = media is MediaFile ? ((MediaFile)media).OriginalFileExtension : null,
                            FileSize = media is MediaFile ? ((MediaFile)media).Size : (long?)null,
                            FileUrl = media is MediaFile ? ((MediaFile)media).PublicUrl : null,
                            // TODO: need implementation ThumbnailUrl = ????

                        }).ToDataListResponse(request);

            return new GetFilesResponse
                       {
                           Data = listResponse
                       };
        }
    }
}