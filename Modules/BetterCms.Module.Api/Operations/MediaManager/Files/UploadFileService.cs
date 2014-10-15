using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.Exceptions.Api;
using BetterCms.Module.Api.Operations.MediaManager.Files.File;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Services;

using NHibernate.Linq;

namespace BetterCms.Module.Api.Operations.MediaManager.Files
{
    /// <summary>
    /// The upload file service.
    /// </summary>
    public class UploadFileService: IUploadFileService
    {
        private readonly IRepository repository;

        private readonly IMediaFileService mediaFileService;

        public UploadFileService(IRepository repository, IMediaFileService mediaFileService)
        {
            this.repository = repository;
            this.mediaFileService = mediaFileService;
        }

        /// <summary>
        /// Upload file from the stream.
        /// </summary>
        /// <param name="request">The upload file request.</param>
        /// <returns>The upload file response.</returns>
        public UploadFileResponse Post(UploadFileRequest request)
        {
            IEnumerable<MediaFolder> parentFolderFuture = null;
            if (request.Data.FolderId.HasValue)
            {
                parentFolderFuture = repository.AsQueryable<MediaFolder>()
                    .Where(c => c.Id == request.Data.FolderId.Value && !c.IsDeleted)
                    .ToFuture();
            }
            
            MediaFolder parentFolder = null;
            if (parentFolderFuture != null)
            {
                parentFolder = parentFolderFuture.First();
                if (parentFolder.Type != Module.MediaManager.Models.MediaType.File)
                {
                    throw new CmsApiValidationException("Folder must be type of an file.");
                }
            }

            var savedFile = mediaFileService.UploadFile(
                Module.MediaManager.Models.MediaType.File,
                parentFolder != null ? parentFolder.Id : Guid.Empty,
                request.Data.FileName,
                request.Data.FileStream.Length,
                request.Data.FileStream,
                false);

            return new UploadFileResponse
            {
                Data = savedFile.Id
            };
        }
    }
}