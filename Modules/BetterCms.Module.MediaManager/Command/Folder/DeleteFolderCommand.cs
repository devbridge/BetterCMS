// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeleteFolderCommand.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.MediaManager.Command.Folder
{
    /// <summary>
    /// A command to delete given folder.
    /// </summary>
    public class DeleteFolderCommand : CommandBase, ICommand<DeleteFolderCommandRequest, bool>
    {
        private readonly IMediaService mediaFileService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteFolderCommand" /> class.
        /// </summary>
        /// <param name="mediaFileService">The media file service.</param>
        public DeleteFolderCommand(IMediaService mediaFileService)
        {
            this.mediaFileService = mediaFileService;
        }

        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Executed command result.</returns>
        public bool Execute(DeleteFolderCommandRequest request)
        {
            return mediaFileService.DeleteMedia(request.FolderId, request.Version, true, Context.Principal);
        }
    }
}