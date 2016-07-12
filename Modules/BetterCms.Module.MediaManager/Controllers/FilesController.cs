// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilesController.cs" company="Devbridge Group LLC">
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
using System.Web;
using System.Web.Mvc;

using BetterCms.Core.Security;
using BetterCms.Core.Services.Storage;

using BetterCms.Module.MediaManager.Command.Files.DownloadFile;
using BetterCms.Module.MediaManager.Command.Files.GetFile;
using BetterCms.Module.MediaManager.Command.Files.GetFiles;
using BetterCms.Module.MediaManager.Command.Files.SaveFile;
using BetterCms.Module.MediaManager.Command.MediaManager.DeleteMedia;
using BetterCms.Module.MediaManager.Content.Resources;
using BetterCms.Module.MediaManager.ViewModels.File;
using BetterCms.Module.MediaManager.ViewModels.MediaManager;

using BetterCms.Module.Root;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Models;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.MediaManager.Controllers
{
    /// <summary>
    /// Handles site settings logic for Media module Files tab.
    /// </summary>
    [ActionLinkArea(MediaManagerModuleDescriptor.MediaManagerAreaName)]
    public class FilesController : CmsControllerBase
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
        /// Gets the files list.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>
        /// List of files.
        /// </returns>
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.DeleteContent, RootModuleConstants.UserRoles.Administration)]
        public ActionResult GetFilesList(MediaManagerViewModel options)
        {
            var success = true;
            if (options == null)
            {
                options = new MediaManagerViewModel();
            }
            options.SetDefaultPaging();

            var model = GetCommand<GetFilesCommand>().ExecuteCommand(options);
            if (model == null)
            {
                success = false;
            }

            return WireJson(success, model);
        }

        /// <summary>
        /// Deletes file.
        /// </summary>
        /// <param name="id">The file id.</param>
        /// <param name="version">The file entity version.</param>
        /// <returns>
        /// Json with result status.
        /// </returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.DeleteContent)]
        public ActionResult FileDelete(string id, string version)
        {
            var request = new DeleteMediaCommandRequest
            {
                Id = id.ToGuidOrDefault(),
                Version = version.ToIntOrDefault()
            };
            if (GetCommand<DeleteMediaCommand>().ExecuteCommand(request))
            {
                Messages.AddSuccess(MediaGlobalization.DeleteFile_DeletedSuccessfully_Message);
                return Json(new WireJson
                {
                    Success = true
                });
            }

            return Json(new WireJson { Success = false });
        }

        /// <summary>
        /// Gets files list to insert to content.
        /// </summary>
        /// <returns>
        /// The view.
        /// </returns>
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.Administration)]
        public ActionResult FileInsert()
        {
            if (CmsConfiguration.Security.AccessControlEnabled && !StorageService.SecuredUrlsEnabled)
            {
                Messages.AddWarn(MediaGlobalization.TokenBasedSecurity_NotSupported_Message);
            }
            if (CmsConfiguration.Security.AccessControlEnabled
                && StorageService.SecuredUrlsEnabled
                && StorageService.SecuredContainerIssueWarning != null)
            {
                Messages.AddWarn(StorageService.SecuredContainerIssueWarning);
            }

            var files = GetCommand<GetFilesCommand>().ExecuteCommand(new MediaManagerViewModel());
            var success = files != null;
            var view = RenderView("FileInsert", new MediaImageViewModel());

            return ComboWireJson(success, view, files, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Downloads the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        /// File to download.
        /// </returns>
        /// <exception cref="System.Web.HttpException">404;Page Not Found</exception>
        public ActionResult Download(string id)
        {
            var model = GetCommand<DownloadFileCommand>().ExecuteCommand(id.ToGuidOrDefault());
            if (model != null)
            {
                if (model.HasNoAccess)
                {
                    throw new HttpException(403, "403 Access Forbidden");
                }

                if (!string.IsNullOrWhiteSpace(model.RedirectUrl))
                {
                    return Redirect(model.RedirectUrl);
                }

                model.FileStream.Position = 0;
                return File(model.FileStream, model.ContentMimeType, model.FileDownloadName);
            }

            throw new HttpException(404, "Page Not Found");
        }

        /// <summary>
        /// Edits the file.
        /// </summary>
        /// <param name="fileId">The file id.</param>
        /// <returns>The view.</returns>
        [HttpGet]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
        public ActionResult FileEditor(string fileId)
        {
            var model = GetCommand<GetFileCommand>().ExecuteCommand(fileId.ToGuidOrDefault());
            var view = RenderView("FileEditor", model ?? new FileViewModel());

            return ComboWireJson(model != null, view, model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Edits the file.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Json response.
        /// </returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
        public ActionResult FileEditor(FileViewModel model)
        {
            if (GetCommand<SaveFileDataCommand>().ExecuteCommand(model))
            {
                var result = GetCommand<GetFileCommand>().ExecuteCommand(model.Id.ToGuidOrDefault());
                return Json(new WireJson { Success = result != null, Data = result });
            }

            return Json(new WireJson { Success = false });
        }
    }
}
