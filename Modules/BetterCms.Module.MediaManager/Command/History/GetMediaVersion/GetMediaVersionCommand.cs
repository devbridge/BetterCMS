// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetMediaVersionCommand.cs" company="Devbridge Group LLC">
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

using BetterCms.Core.Security;

using BetterCms.Module.MediaManager.Content.Resources;
using BetterCms.Module.MediaManager.Helpers;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Models.Extensions;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.MediaManager.ViewModels.History;

using BetterCms.Module.Root.Mvc;

using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.DataAccess.DataContext.Fetching;
using BetterModules.Core.Web.Mvc.Commands;

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

            var media = Repository.AsQueryable<Media>(m => m.Id == request).Fetch(m => m.Original).FirstOne();
            var file = media as MediaFile;

            if (file != null)
            {
                var image = media as MediaImage;

                if (cmsConfiguration.Security.AccessControlEnabled)
                {
                    AccessControlService.DemandAccess(file.Original as MediaFile ?? file, Context.Principal, AccessLevel.Read);
                }

                response.AddProperty(MediaGlobalization.MediaHistory_Preview_Properties_Title, file.Title);

                if (image != null)
                {
                    response.AddProperty(MediaGlobalization.MediaHistory_Preview_Properties_Caption, image.Caption);
                }

                var publicUrl = fileService.GetDownloadFileUrl(MediaType.File, file.Id, fileUrlResolver.EnsureFullPathUrl(file.PublicUrl));
                response.AddUrl(MediaGlobalization.MediaHistory_Preview_Properties_PublicUrl, publicUrl);

                if (image != null)
                {
                    response.AddUrl(
                        MediaGlobalization.MediaHistory_Preview_Properties_PublicThumbnailUrl,
                        fileUrlResolver.EnsureFullPathUrl(
                            image.PublicThumbnailUrl + string.Format("?{0}", DateTime.Now.ToString(MediaManagerModuleDescriptor.HardLoadImageDateTimeFormat))));
                }

                response.AddProperty(MediaGlobalization.MediaHistory_Preview_Properties_FileSize, file.SizeAsText());

                if (image != null)
                {
                    var dimensionCalculator = new ImageDimensionsCalculator(image);

                    if (dimensionCalculator.Height != dimensionCalculator.ResizedCroppedHeight || dimensionCalculator.Width != dimensionCalculator.ResizedCroppedWidth)
                    {
                        response.AddProperty(MediaGlobalization.MediaHistory_Preview_Properties_CroppedImageDimensions,
                            string.Format("{0} x {1}", dimensionCalculator.ResizedCroppedWidth, dimensionCalculator.ResizedCroppedHeight));
                    }
                    response.AddProperty(MediaGlobalization.MediaHistory_Preview_Properties_ImageDimensions,
                        string.Format("{0} x {1}", dimensionCalculator.Width, dimensionCalculator.Height));
                }

                if (!string.IsNullOrWhiteSpace(file.Description))
                {
                    response.AddProperty(MediaGlobalization.MediaHistory_Preview_Properties_Description, file.Description);
                }

                if (media.Image != null)
                {
                    response.AddImage(
                        media.Image.Caption,
                        fileUrlResolver.EnsureFullPathUrl(
                            media.Image.PublicUrl + string.Format("?{0}", DateTime.Now.ToString(MediaManagerModuleDescriptor.HardLoadImageDateTimeFormat))));
                }

                if (image != null)
                {
                    response.AddImage(
                        image.Caption,
                        fileUrlResolver.EnsureFullPathUrl(
                            image.PublicUrl + string.Format("?{0}", DateTime.Now.ToString(MediaManagerModuleDescriptor.HardLoadImageDateTimeFormat))));
                }
            }

            return response;
        }
    }
}