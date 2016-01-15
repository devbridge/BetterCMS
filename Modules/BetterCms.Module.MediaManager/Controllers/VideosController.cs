// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VideosController.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.MediaManager.Command.MediaManager.DeleteMedia;
using BetterCms.Module.MediaManager.Command.Videos.GetVideos;
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
    /// Video manager.
    /// </summary>
    [BcmsAuthorize]
    [ActionLinkArea(MediaManagerModuleDescriptor.MediaManagerAreaName)]
    public class VideosController : CmsControllerBase
    {
        /// <summary>
        /// Gets the videos list.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>
        /// List of videos
        /// </returns>
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent, RootModuleConstants.UserRoles.DeleteContent)]
        public ActionResult GetVideosList(MediaManagerViewModel options)
        {
            var success = true;
            if (options == null)
            {
                options = new MediaManagerViewModel();
            }
            options.SetDefaultPaging();

            var model = GetCommand<GetVideosCommand>().ExecuteCommand(options);
            if (model == null)
            {
                success = false;
            }

            return Json(new WireJson { Success = success, Data = model });
        }

        /// <summary>
        /// Deletes video.
        /// </summary>
        /// <param name="id">The video id.</param>
        /// <param name="version">The video entity version.</param>
        /// <returns>
        /// Json with result status.
        /// </returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.DeleteContent)]
        public ActionResult VideoDelete(string id, string version)
        {
            var request = new DeleteMediaCommandRequest
            {
                Id = id.ToGuidOrDefault(),
                Version = version.ToIntOrDefault()
            };
            if (GetCommand<DeleteMediaCommand>().ExecuteCommand(request))
            {
                Messages.AddSuccess(MediaGlobalization.DeleteVideo_DeletedSuccessfully_Message);
                return Json(new WireJson
                {
                    Success = true
                });
            }

            return Json(new WireJson { Success = false });
        }
    }
}
