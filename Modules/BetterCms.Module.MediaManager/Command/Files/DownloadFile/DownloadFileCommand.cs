using System;
using System.Linq;
using System.Web;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataAccess.DataContext.Fetching;
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
            var fileQuery = Repository.AsQueryable<MediaFile>(f => f.Id == id && !f.IsDeleted);

            if (cmsConfiguration.Security.AccessControlEnabled)
            {
                fileQuery = fileQuery.FetchMany(f => f.AccessRules);
            }

            var file = fileQuery.ToList().FirstOne();            

            // Access control is ALWAYS disabled for images
            var accesControlEnabled = cmsConfiguration.Security.AccessControlEnabled && file.Type != MediaType.Image;

            if (!accesControlEnabled || !storageService.SecuredUrlsEnabled)
            {
                return new DownloadFileCommandResponse { RedirectUrl = file.PublicUrl };
            }

            // Get download URL with security token
            var principal = SecurityService.GetCurrentPrincipal();
            var accessLevel = accessControlService.GetAccessLevel(file, principal);

            if (accessLevel == AccessLevel.Deny)
            {
                throw new HttpException(403, "Access Forbidden");
            }

            var url = storageService.GetSecuredUrl(file.FileUri);
            return new DownloadFileCommandResponse { RedirectUrl = url };
        }
    }
}