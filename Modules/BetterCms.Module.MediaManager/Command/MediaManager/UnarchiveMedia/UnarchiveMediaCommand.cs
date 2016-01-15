// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnarchiveMediaCommand.cs" company="Devbridge Group LLC">
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
using System.Collections.Generic;
using System.Linq;

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.ViewModels.MediaManager;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.DataContracts;
using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.MediaManager.Command.MediaManager.UnarchiveMedia
{
    /// <summary>
    /// Command for unarchive a media
    /// </summary>
    public class UnarchiveMediaCommand : CommandBase, ICommand<UnarchiveMediaCommandRequest, MediaViewModel>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public MediaViewModel Execute(UnarchiveMediaCommandRequest request)
        {
            UnitOfWork.BeginTransaction();

            Media media = Repository.AsProxy<Media>(request.Id);
            media.Version = request.Version;
            media.IsArchived = false;
            Repository.Save(media);

            var unarchivedMedias = new List<Media> { media };
            UnarchiveSubMedias(media, unarchivedMedias);

            UnitOfWork.Commit();

            // Notify
            foreach (var archivedMedia in unarchivedMedias.Distinct())
            {
                Events.MediaManagerEvents.Instance.OnMediaUnarchived(archivedMedia);
            }

            return new MediaViewModel
            {
                Id = media.Id,
                Version = media.Version
            };
        }

        private void UnarchiveSubMedias(IEntity media, List<Media> unarchivedMedias)
        {
            var subItems = Repository.AsQueryable<Media>().Where(m => m.Folder != null && m.Folder.Id == media.Id).ToList();
            foreach (var subItem in subItems)
            {
                if (subItem.IsArchived)
                {
                    subItem.IsArchived = false;
                    unarchivedMedias.Add(subItem);

                    Repository.Save(subItem);
                }
                UnarchiveSubMedias(subItem, unarchivedMedias);
            }
        }
    }
}