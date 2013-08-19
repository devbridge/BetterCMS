using System;
using System.Linq;
using System.Web;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Security;
using BetterCms.Core.Services.Storage;

using BetterCms.Module.MediaManager.Content.Resources;
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
        /// The storage service
        /// </summary>
        private readonly IStorageService storageService;

        /// <summary>
        /// The CMS configuration
        /// </summary>
        private readonly ICmsConfiguration cmsConfiguration;

        /// <summary>
        /// The access control service
        /// </summary>
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
        /// <returns>
        /// Response type of <see cref="DownloadFileCommandResponse" />
        /// </returns>
        /// <exception cref="System.Web.HttpException">403;Access Forbidden</exception>
        public DownloadFileCommandResponse Execute(Guid id)
        {
            // Load file
            var file = Repository
                .AsQueryable<MediaFile>(f => f.Id == id && !f.IsDeleted)
                .Select(f => new
                        {
                            FileUri = f.FileUri,
                            Title = f.Title,
                            OriginalFileExtension = f.OriginalFileExtension,
                            Type = f.Type,
                            PublicUrl = f.PublicUrl
                        })
                .FirstOne();

            // Access control is ALWAYS disabled for images
            var accesControlEnabled = cmsConfiguration.AccessControlEnabled && file.Type != MediaType.Image;

            if (!accesControlEnabled || !storageService.SecuredUrlsEnabled)
            {
                return new DownloadFileCommandResponse { RedirectUrl = file.PublicUrl };
            }

            // Get download URL with security token
            if (storageService.SecuredUrlsEnabled)
            {
                var principal = SecurityService.GetCurrentPrincipal();
                var accessLevel = accessControlService.GetAccessLevel(id, principal);

                if (accessLevel == AccessLevel.Deny)
                {
                    throw new HttpException(403, "Access Forbidden");
                }

                var url = storageService.GetSecuredUrl(file.FileUri);
                return new DownloadFileCommandResponse { RedirectUrl = url };
            }

            return null;
        }
    }
}