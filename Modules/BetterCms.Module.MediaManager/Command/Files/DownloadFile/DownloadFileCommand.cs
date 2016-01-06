using System;
using System.Linq;

using BetterCms.Core.Exceptions.Service;

using BetterCms.Core.Security;
using BetterCms.Core.Services.Storage;

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.DataAccess.DataContext.Fetching;
using BetterModules.Core.Web.Mvc.Commands;

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
        /// The file URL resolver
        /// </summary>
        private readonly IMediaFileUrlResolver fileUrlResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadFileCommand" /> class.
        /// </summary>
        /// <param name="storageService">The storage service.</param>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        public DownloadFileCommand(IStorageService storageService, ICmsConfiguration cmsConfiguration, IMediaFileUrlResolver fileUrlResolver)
        {
            this.storageService = storageService;
            this.cmsConfiguration = cmsConfiguration;
            this.fileUrlResolver = fileUrlResolver;
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
                return new DownloadFileCommandResponse { RedirectUrl = fileUrlResolver.EnsureFullPathUrl(file.PublicUrl) };
            }

            // Get download URL with security token
            try
            {
                AccessControlService.DemandAccess(file, Context.Principal, AccessLevel.Read);
            }
            catch (SecurityException)
            {
                return new DownloadFileCommandResponse { HasNoAccess = true };
            }

            var url = storageService.GetSecuredUrl(file.FileUri);
            return new DownloadFileCommandResponse { RedirectUrl = url };
        }
    }
}