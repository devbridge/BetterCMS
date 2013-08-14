using System;
using System.Linq;
using System.Web;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Security;
using BetterCms.Core.Services.Storage;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.MediaManager.Command.Files.DownloadFile
{
    /// <summary>
    /// Gets file info for downloading.
    /// </summary>
    public class DownloadFileCommand : CommandBase, ICommand<Guid, DownloadFileCommandResponse>
    {
        /// <summary>
        /// Gets or sets the storage.
        /// </summary>
        /// <value>
        /// The storage.
        /// </value>
        private readonly IStorageService storageService;

        private readonly ICmsConfiguration cmsConfiguration;

        private readonly IAccessControlService accessControlService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadFileCommand" /> class.
        /// </summary>
        /// <param name="storageService">The storage service.</param>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        /// <param name="accessControlService">The access control service.</param>
        public DownloadFileCommand(IStorageService storageService, ICmsConfiguration cmsConfiguration, IAccessControlService accessControlService)
        {
            this.storageService = storageService;
            this.cmsConfiguration = cmsConfiguration;
            this.accessControlService = accessControlService;
        }

        /// <summary>
        /// Executes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Response type of <see cref="DownloadFileCommandResponse"/></returns>
        public DownloadFileCommandResponse Execute(Guid id)
        {
            if (cmsConfiguration.AccessControlEnabled)
            {
                var principal = SecurityService.GetCurrentPrincipal();
                var accessLevel = accessControlService.GetAccessLevel(id, principal);

                if (accessLevel == AccessLevel.Deny)
                {
                    throw new HttpException(403, "Access Forbidden");
                }

                var fileUri = Repository
                    .AsQueryable<MediaFile>(f => f.Id == id && !f.IsDeleted)
                    .Select(f => f.FileUri)
                    .FirstOrDefault();

                var url = storageService.GetSecuredUrl(fileUri);
                return new DownloadFileCommandResponse { RedirectUrl = url };
            }

            var file = Repository.FirstOrDefault<MediaFile>(f => f.Id == id && !f.IsDeleted);
            if (file != null)
            {
                var response = storageService.DownloadObject(file.FileUri);
                if (response != null)
                {
                    return new DownloadFileCommandResponse
                        {
                            FileStream = response.ResponseStream,
                            // TODO: Change so that content type is determined from file extension or stored in the database
                            ContentMimeType = System.Net.Mime.MediaTypeNames.Application.Octet, // Specify the generic octet-stream MIME type.
                            FileDownloadName = string.Format("{0}{1}", System.IO.Path.GetFileNameWithoutExtension(file.Title), file.OriginalFileExtension)
                        };
                }
            }

            return null;
        }
    }
}