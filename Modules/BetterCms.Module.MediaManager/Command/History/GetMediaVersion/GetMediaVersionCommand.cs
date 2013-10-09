using System;
using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataAccess.DataContext.Fetching;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Security;

using BetterCms.Module.MediaManager.Content.Resources;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Models.Extensions;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.MediaManager.ViewModels.History;

using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.MediaManager.Command.History.GetMediaVersion
{
    /// <summary>
    /// Command to get media preview.
    /// </summary>
    public class GetMediaVersionCommand : CommandBase, ICommand<Guid, MediaPreviewViewModel>
    {
        /// <summary>
        /// The file service
        /// </summary>
        private readonly IMediaFileService fileService;

        /// <summary>
        /// The CMS configuration
        /// </summary>
        private readonly ICmsConfiguration cmsConfiguration;

        /// <summary>
        /// The file URL resolver
        /// </summary>
        private readonly IMediaFileUrlResolver fileUrlResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetMediaVersionCommand" /> class.
        /// </summary>
        /// <param name="fileService">The file service.</param>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        public GetMediaVersionCommand(IMediaFileService fileService, ICmsConfiguration cmsConfiguration, IMediaFileUrlResolver fileUrlResolver)
        {
            this.fileService = fileService;
            this.cmsConfiguration = cmsConfiguration;
            this.fileUrlResolver = fileUrlResolver;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Media preview.</returns>
        public MediaPreviewViewModel Execute(Guid request)
        {
            var response = new MediaPreviewViewModel();

            var media = Repository
                .AsQueryable<Media>(m => m.Id == request)
                .Fetch(m => m.Original)
                .FirstOne();

            var image = media as MediaImage;
            if (image != null)
            {
                response.AddProperty(MediaGlobalization.MediaHistory_Preview_Properties_Caption, image.Caption);
                response.AddProperty(MediaGlobalization.MediaHistory_Preview_Properties_ImageDimensions, string.Format("{0} x {1}", image.Width, image.Height));
                response.AddProperty(MediaGlobalization.MediaHistory_Preview_Properties_PublicThumbnailUrl, fileUrlResolver.EnsureFullPathUrl(image.PublicThumbnailUrl), true);

                response.AddProperty(image.Caption, fileUrlResolver.EnsureFullPathUrl(image.PublicUrl), isImageUrl: true);
            }

            var file = media as MediaFile;
            if (file != null)
            {
                if (cmsConfiguration.Security.AccessControlEnabled)
                {
                    AccessControlService.DemandAccess(file.Original as MediaFile ?? file, Context.Principal, AccessLevel.Read);
                }

                var publicUrl = fileService.GetDownloadFileUrl(MediaType.File, file.Id, fileUrlResolver.EnsureFullPathUrl(file.PublicUrl));

                response.AddProperty(MediaGlobalization.MediaHistory_Preview_Properties_Title, file.Title);
                response.AddProperty(MediaGlobalization.MediaHistory_Preview_Properties_Description, file.Description);
                response.AddProperty(MediaGlobalization.MediaHistory_Preview_Properties_FileSize, file.SizeAsText());
                response.AddProperty(MediaGlobalization.MediaHistory_Preview_Properties_PublicUrl, publicUrl, true);

                if (media.Image != null)
                {
                    response.AddProperty(media.Image.Caption, fileUrlResolver.EnsureFullPathUrl(media.Image.PublicUrl), isImageUrl: true);
                }
            }

            response.Properties = response.Properties.OrderByDescending(o => o.Title).ToList();

            return response;
        }
    }
}