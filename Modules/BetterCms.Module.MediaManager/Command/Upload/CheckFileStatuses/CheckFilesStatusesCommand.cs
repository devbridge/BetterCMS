// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckFilesStatusesCommand.cs" company="Devbridge Group LLC">
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
using System.Collections.Generic;
using System.Linq;

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Models.Extensions;
using BetterCms.Module.MediaManager.ViewModels.MediaManager;

using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.MediaManager.Command.Upload.CheckFileStatuses
{
    public class CheckFilesStatusesCommand : CommandBase, ICommand<List<string>, IList<MediaFileViewModel>>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The list of media file view models
        /// </returns>
        public IList<MediaFileViewModel> Execute(List<string> request)
        {
            IList<MediaFileViewModel> files = null;
            var ids = new List<Guid>(request.Count);

            foreach (var id in request)
            {
                var guid = id.ToGuidOrDefault();
                if (!guid.HasDefaultValue())
                {
                    ids.Add(guid);
                }
            }

            if (ids.Count > 0)
            {
                files = Repository
                    .AsQueryable<MediaFile>()
                    .Where(f => ids.Contains(f.Id))
                    .ToList()
                    .Select(f => new MediaFileViewModel
                                     {
                                         Id = f.Id,
                                         IsProcessing = f.GetIsProcessing(),
                                         IsFailed = f.GetIsFailed()
                                     })
                    .ToList();
            }

            return files ?? new List<MediaFileViewModel>();
        }
    }
}