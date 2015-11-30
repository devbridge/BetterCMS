// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HistoryController.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.MediaManager.Command.History.GetMediaHistory;
using BetterCms.Module.MediaManager.Command.History.GetMediaVersion;
using BetterCms.Module.MediaManager.Command.History.RestoreMediaVersion;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Mvc;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.MediaManager.Controllers
{
    /// <summary>
    /// Media history controller.
    /// </summary>
    [BcmsAuthorize]
    [ActionLinkArea(MediaManagerModuleDescriptor.MediaManagerAreaName)]
    public class HistoryController : CmsControllerBase
    {
        /// <summary>
        /// Medias the history.
        /// </summary>
        /// <param name="mediaId">The content id.</param>
        /// <returns>Media history view.</returns>
        [HttpGet]
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration, RootModuleConstants.UserRoles.EditContent)]
        public ActionResult MediaHistory(string mediaId)
        {
            var model = GetCommand<GetMediaHistoryCommand>().ExecuteCommand(new GetMediaHistoryRequest
                {
                    MediaId = mediaId.ToGuidOrDefault(),
                });

            return View(model);
        }

        /// <summary>
        /// Medias the history.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Media history view.</returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
        public ActionResult MediaHistory(GetMediaHistoryRequest request)
        {
            var model = GetCommand<GetMediaHistoryCommand>().ExecuteCommand(request);

            return View(model);
        }

        /// <summary>
        /// Medias the version.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Media preview.</returns>
        [HttpGet]
        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration, RootModuleConstants.UserRoles.EditContent)]
        public ActionResult MediaVersion(string id)
        {
            var model = GetCommand<GetMediaVersionCommand>().ExecuteCommand(id.ToGuidOrDefault());
           
            return View(model);
        }

        /// <summary>
        /// Restores the media version.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="shouldOverrideString">Override public url when restore version.</param>
        /// <returns>WireJson result.</returns>
        [HttpPost]
        [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
        public ActionResult RestoreMediaVersion(string id, string shouldOverrideString)
        {
            bool shouldOverride;
            if (!bool.TryParse(shouldOverrideString, out shouldOverride))
            {
                shouldOverride = true;
            }

            var result =
                GetCommand<RestoreMediaVersionCommand>()
                    .ExecuteCommand(new RestoreMediaVersionRequest { VersionId = id.ToGuidOrDefault(), ShouldOverridUrl = shouldOverride });
            
            return WireJson(result);
        }
    }
}
