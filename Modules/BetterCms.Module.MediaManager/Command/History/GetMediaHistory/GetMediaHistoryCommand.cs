// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetMediaHistoryCommand.cs" company="Devbridge Group LLC">
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
using System.Linq;

using BetterCms.Core.Security;

using BetterCms.Module.MediaManager.Content.Resources;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Models.Enum;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.MediaManager.ViewModels.History;

using BetterCms.Module.Root;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.MediaManager.Command.History.GetMediaHistory
{
    /// <summary>
    /// Command to load a list of the content history versions.
    /// </summary>
    public class GetMediaHistoryCommand : CommandBase, ICommand<GetMediaHistoryRequest, MediaHistoryViewModel>
    {
        /// <summary>
        /// Gets or sets the history service.
        /// </summary>
        /// <value>
        /// The history service.
        /// </value>
        private readonly IMediaHistoryService historyService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetMediaHistoryCommand"/> class.
        /// </summary>
        /// <param name="historyService">The history service.</param>
        public GetMediaHistoryCommand(IMediaHistoryService historyService)
        {
            this.historyService = historyService;
        }

        /// <summary>
        /// Gets or sets a value indicating whether user has edit role.
        /// </summary>
        /// <value>
        ///   <c>true</c> if user has edit role; otherwise, <c>false</c>.
        /// </value>
        private bool UserHasEditRole { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user has administration role.
        /// </summary>
        /// <value>
        /// <c>true</c> if user has administration role; otherwise, <c>false</c>.
        /// </value>
        private bool UserHasAdministrationRole { get; set; }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>View model with media history items.</returns>
        public MediaHistoryViewModel Execute(GetMediaHistoryRequest request)
        {
            UserHasEditRole = SecurityService.IsAuthorized(RootModuleConstants.UserRoles.EditContent);
            UserHasAdministrationRole = SecurityService.IsAuthorized(RootModuleConstants.UserRoles.Administration);

            var history = historyService.GetMediaHistory(request.MediaId, request).Select(Convert).ToList();
            return new MediaHistoryViewModel(history, request, history.Count, request.MediaId);
        }

        /// <summary>
        /// Converts the specified media.
        /// </summary>
        /// <param name="media">The media.</param>
        /// <returns>Converts media entity to view model.</returns>
        private MediaHistoryItem Convert(Media media)
        {
            var file = media.Original as MediaFile;

            var canRestore = media.Original != null 
                && (SecurityService.IsAuthorized(RootModuleConstants.UserRoles.EditContent) || SecurityService.IsAuthorized(RootModuleConstants.UserRoles.Administration))
                && (file == null || AccessControlService.GetAccessLevel(file, Context.Principal) >= AccessLevel.ReadWrite);

            return new MediaHistoryItem
                       {
                           Id = media.Id,
                           Version = media.Version,
                           StatusName = media.Original != null
                                ? MediaGlobalization.MediaHistory_Status_Archived
                                : MediaGlobalization.MediaHistory_Status_Active,
                           Status = media.Original != null
                                ? MediaHistoryStatus.Archived
                                : MediaHistoryStatus.Active,
                           PublishedOn = media.PublishedOn,
                           PublishedByUser = media.ModifiedByUser,
                           ArchivedOn = media.Original != null ? media.ModifiedOn : (DateTime?)null,
                           DisplayedFor = media.Original != null
                                   ? media.ModifiedOn - media.PublishedOn
                                   : (TimeSpan?)null,
                           CanCurrentUserRestoreIt = canRestore
                       };
        }
    }
}