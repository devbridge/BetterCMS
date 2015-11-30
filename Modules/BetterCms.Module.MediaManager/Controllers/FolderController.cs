// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FolderController.cs" company="Devbridge Group LLC">
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
using System.Web.Mvc;

using BetterCms.Core.Security;
using BetterCms.Module.MediaManager.Command.Folder;
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
    /// Folder manager.
    /// </summary>
    [BcmsAuthorize]
    [ActionLinkArea(MediaManagerModuleDescriptor.MediaManagerAreaName)]
    public class FolderController : CmsControllerBase
    {
        /// <summary>
        /// An action to save the folder.
        /// </summary>
        /// <param name="folder">The folder data.</param>
        /// <returns>Json with status.</returns>
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
        [HttpPost]
        public ActionResult SaveFolder(MediaFolderViewModel folder)
        {
            if (ModelState.IsValid)
            {
                var response = GetCommand<SaveFolderCommand>().ExecuteCommand(folder);
                if (response != null)
                {
                    if (folder.Id.HasDefaultValue())
                    {
                        Messages.AddSuccess(MediaGlobalization.CreateFolder_CreatedSuccessfully_Message);
                    }
                    return Json(new WireJson { Success = true, Data = response });
                }
            }

            return Json(new WireJson { Success = false });
        }

        /// <summary>
        /// An action to delete a given folder.
        /// </summary>
        /// <param name="id">The folder id.</param>
        /// <param name="version">The version.</param>
        /// <returns>
        /// Json with status.
        /// </returns>
        [BcmsAuthorize(RootModuleConstants.UserRoles.DeleteContent)]
        [HttpPost]
        public ActionResult DeleteFolder(string id, string version)
        {
            bool success = GetCommand<DeleteFolderCommand>().ExecuteCommand(
                new DeleteFolderCommandRequest
                {
                    FolderId = id.ToGuidOrDefault(),
                    Version = version.ToIntOrDefault()
                });

            if (success)
            {
                Messages.AddSuccess(MediaGlobalization.DeleteFolder_DeletedSuccessfully_Message);
            }
            else
            {
                Messages.AddError(MediaGlobalization.DeleteFolder_DeletedError_Message);
            }

            return Json(new WireJson(success));
        }
    }
}