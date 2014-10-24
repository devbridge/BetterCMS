using System;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Exceptions.Api;
using BetterCms.Core.Security;
using BetterCms.Module.Api.Operations.MediaManager.Files.File;
using BetterCms.Module.MediaManager.Models;
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
    }
}