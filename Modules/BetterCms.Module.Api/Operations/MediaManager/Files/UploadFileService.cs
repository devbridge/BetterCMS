using System;
using System.ComponentModel;
using System.Linq;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Core.Exceptions.Api;
using BetterCms.Core.Security;
using BetterCms.Module.Api.Operations.MediaManager.Files.File;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Models.Extensions;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Models.Extensions;

namespace BetterCms.Module.Api.Operations.MediaManager.Files
{
    /// <summary>
    /// The upload file service.
    /// </summary>
    public class UploadFileService: IUploadFileService
    {
        private readonly IRepository repository;

        private readonly IMediaFileService mediaFileService;

        private readonly IAccessControlService accessControlService;

        public UploadFileService(IRepository repository, IMediaFileService mediaFileService, IAccessControlService accessControlService)
        {
            this.repository = repository;
            this.mediaFileService = mediaFileService;
            this.accessControlService = accessControlService;
        }

        /// <summary>
        /// Upload file from the stream.
        /// </summary>
        /// <param name="request">The upload file request.</param>
        /// <returns>The upload file response.</returns>
        public UploadFileResponse Post(UploadFileRequest request)
        {
            MediaFolder parentFolder = null;
            if (request.Data.FolderId.HasValue)
            {
                parentFolder = repository.AsQueryable<MediaFolder>()
                    .Where(c => c.Id == request.Data.FolderId.Value && !c.IsDeleted)
                    .FirstOne();

                if (parentFolder.Type != Module.MediaManager.Models.MediaType.File)
                {
                    throw new CmsApiValidationException("Folder must be type of an file.");
                }
            }

            var savedFile = mediaFileService.UploadFileWithStream(
                Module.MediaManager.Models.MediaType.File,
                parentFolder != null ? parentFolder.Id : Guid.Empty,
                request.Data.FileName,
                request.Data.FileStream.Length,
                request.Data.FileStream,
                request.Data.WaitForUploadResult,
                request.Data.Title,
                request.Data.Description);

            if (savedFile != null)
            {
                if (request.Data.AccessRules != null)
                {
                    if (savedFile.AccessRules != null)
                    {
                        savedFile.AccessRules.RemoveDuplicateEntities();
                    }
                    var accessRules =
                        request.Data.AccessRules.Select(
                            r => (IAccessRule)new AccessRule { AccessLevel = (AccessLevel)(int)r.AccessLevel, Identity = r.Identity, IsForRole = r.IsForRole })
                            .ToList();
                    accessControlService.UpdateAccessControl(savedFile, accessRules);
                    mediaFileService.SaveMediaFile(savedFile);
                }

                Events.MediaManagerEvents.Instance.OnMediaFileUploaded(savedFile);
            }

            return new UploadFileResponse
            {
                Data = savedFile.Id
            };
        }

        /// <summary>
        /// Reupload file from the stream.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public ReuploadFileResponse Put(ReuploadFileRequest request)
        {
            if (request.Data.Id.Equals(Guid.Empty))
            {
                throw new CmsApiValidationException("MediaFile ID must be provided");
            }

//            var file = repository.AsQueryable<MediaFile>().FirstOrDefault(f => f.Id == request.Data.Id);
//            if (file == null)
//            {
//                throw new CmsApiValidationException("File with specified ID could not be found");
//            }
            // Create and save history item
//            mediaFileService.SaveMediaFile((MediaFile)file.CreateHistoryItem());

            var savedFile = mediaFileService.UploadFileWithStream(
                Module.MediaManager.Models.MediaType.File,
                Guid.Empty,
                request.Data.FileName,
                request.Data.FileStream.Length,
                request.Data.FileStream,
                request.Data.WaitForUploadResult,
                string.Empty,
                string.Empty,
                request.Data.Id);

//            mediaFileService.SaveMediaFile(savedFile);
            return new ReuploadFileResponse { Data = savedFile.Id };
        }
    }
}