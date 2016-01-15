// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MediaManagerController.cs" company="Devbridge Group LLC">
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
using System.Web.Mvc;

using BetterCms.Core.Security;
using BetterCms.Core.Services.Storage;

using BetterCms.Module.MediaManager.Command.Images.GetImages;
using BetterCms.Module.MediaManager.Command.MediaManager.ArchiveMedia;
using BetterCms.Module.MediaManager.Command.MediaManager.RenameMedia;
using BetterCms.Module.MediaManager.Command.MediaManager.UnarchiveMedia;
using BetterCms.Module.MediaManager.Content.Resources;
using BetterCms.Module.MediaManager.ViewModels.MediaManager;

using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Models;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.MediaManager.Controllers
{
    /// <summary>
    /// Handles site settings logic to operate with a media manager.
    /// </summary>
    [BcmsAuthorize]
    [ActionLinkArea(MediaManagerModuleDescriptor.MediaManagerAreaName)]
    public class MediaManagerController : CmsControllerBase
    {
        /// <summary>
        /// Gets or sets the CMS configuration.
        /// </summary>
        /// <value>
        /// The CMS configuration.
        /// </value>
        public ICmsConfiguration CmsConfiguration { get; set; }

        /// <summary>
        /// Gets or sets the storage service.
        /// </summary>
        /// <value>
        /// The storage service.
        /// </value>
        public IStorageService StorageService { get; set; }

        /// <summary>
        /// Renders a media manager tabs container.
        /// </summary>
        /// <returns>
        /// Rendered media manager tabs container.
        /// </returns>
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration, RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.DeleteContent)]
        public ActionResult Index()
        {
            var viewModel = new MediaManagerIndexViewModel();
            if (CmsConfiguration.Security.AccessControlEnabled && !StorageService.SecuredUrlsEnabled)
            {
                if (!(StorageService is FileSystemStorageService && CmsConfiguration.Security.IgnoreLocalFileSystemWarning))
                {
                    viewModel.CustomFilesMessages = new UserMessages();
                    viewModel.CustomFilesMessages.AddWarn(MediaGlobalization.TokenBasedSecurity_NotSupported_Message);
                }
            }
            if (CmsConfiguration.Security.AccessControlEnabled
                && StorageService.SecuredUrlsEnabled
                && StorageService.SecuredContainerIssueWarning != null)
            {
                if (viewModel.CustomFilesMessages == null)
                {
                    viewModel.CustomFilesMessages = new UserMessages();
                }
                viewModel.CustomFilesMessages.AddWarn(StorageService.SecuredContainerIssueWarning);
            }

            var images = GetCommand<GetImagesCommand>().ExecuteCommand(new MediaManagerViewModel());
            var success = images != null;
            var view = RenderView("Index", viewModel);
            
            return ComboWireJson(success, view, images, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Renames media.
        /// </summary>
        /// <param name="media">The media data.</param>
        /// <returns>Json with status.</returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
        public ActionResult RenameMedia(MediaViewModel media)
        {
            if (ModelState.IsValid)
            {
                var response = GetCommand<RenameMediaCommand>().ExecuteCommand(media);
                if (response != null)
                {
                    return Json(new WireJson { Success = true, Data = response });
                }
            }

            return Json(new WireJson { Success = false });
        }

        /// <summary>
        /// Puts media to archive.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="version">The version.</param>
        /// <returns>Json with status</returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
        public ActionResult ArchiveMedia(string id, string version)
        {
            var request = new ArchiveMediaCommandRequest
            {
                Id = id.ToGuidOrDefault(),
                Version = version.ToIntOrDefault()
            };
            var response = GetCommand<ArchiveMediaCommand>().ExecuteCommand(request);
            if (response != null)
            {
                Messages.AddSuccess(MediaGlobalization.ArchiveMedia_ArchivedSuccessfully_Message);
            }

            return WireJson(response != null, response);
        }
        
        /// <summary>
        /// Pulls media out from archive.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="version">The version.</param>
        /// <returns>Json with status</returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
        public ActionResult UnarchiveMedia(string id, string version)
        {
            var request = new UnarchiveMediaCommandRequest
            {
                Id = id.ToGuidOrDefault(),
                Version = version.ToIntOrDefault()
            };
            var response = GetCommand<UnarchiveMediaCommand>().ExecuteCommand(request);
            if (response != null)
            {
                Messages.AddSuccess(MediaGlobalization.UnarchiveMedia_UnarchivedSuccessfully_Message);
            }

            return WireJson(response != null, response);
        }
    }
}