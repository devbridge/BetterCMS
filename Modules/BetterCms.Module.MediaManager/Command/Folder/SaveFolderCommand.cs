// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SaveFolderCommand.cs" company="Devbridge Group LLC">
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

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.ViewModels.MediaManager;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.MediaManager.Command.Folder
{
    public class SaveFolderCommand : CommandBase, ICommand<MediaFolderViewModel, MediaFolderViewModel>
    {
        /// <summary>
        /// Executes this command.
        /// </summary>
        public MediaFolderViewModel Execute(MediaFolderViewModel request)
        {
            MediaFolder folder;

            if (request.Id == default(Guid))
            {
                folder = new MediaFolder();
            }
            else
            {
                folder = Repository.AsProxy<MediaFolder>(request.Id);
            }

            folder.Version = request.Version;
            folder.Title = request.Name;
            folder.Folder = request.ParentFolderId.HasValue && request.ParentFolderId.Value != default(Guid)
                                ? Repository.AsProxy<MediaFolder>(request.ParentFolderId.Value)
                                : null;
            folder.Type = request.Type;

            Repository.Save(folder);
            UnitOfWork.Commit();

            if (request.Id == default(Guid))
            {
                Events.MediaManagerEvents.Instance.OnMediaFolderCreated(folder);
            }
            else
            {
                Events.MediaManagerEvents.Instance.OnMediaFolderUpdated(folder);
            }

            return new MediaFolderViewModel
                       {
                           Id = folder.Id, 
                           Version = folder.Version, 
                           Name = folder.Title
                       };
        }
    }
}