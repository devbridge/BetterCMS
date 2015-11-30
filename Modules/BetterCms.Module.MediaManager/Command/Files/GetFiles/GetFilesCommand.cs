// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetFilesCommand.cs" company="Devbridge Group LLC">
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
using System.Linq.Expressions;
using System.Web;

using BetterModules.Core.DataAccess;
using BetterCms.Module.MediaManager.Command.MediaManager;
using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.MediaManager.Command.Files.GetFiles
{
    public class GetFilesCommand : GetMediaItemsCommandBase<MediaFile>
    {
        /// <summary>
        /// Gets the type of the current media items.
        /// </summary>
        /// <value>
        /// The type of the current media items.
        /// </value>
        protected override MediaType MediaType
        {
            get { return MediaType.File; }
        }

        protected override System.Collections.Generic.IEnumerable<Guid> GetDeniedMedias(ViewModels.MediaManager.MediaManagerViewModel request)
        {
            return AccessControlService.GetDeniedObjects<MediaFile>();
        }

        /// <summary>
        /// Appends the search filter.
        /// </summary>
        /// <param name="searchFilter">The search filter.</param>
        /// <param name="searchQuery">The search query.</param>
        /// <returns>
        /// Appended search filter
        /// </returns>
        protected override Expression<Func<Media, bool>> AppendSearchFilter(Expression<Func<Media, bool>> searchFilter, string searchQuery)
        {
            var searcQueryDecoded = HttpUtility.UrlDecode(searchQuery);

            return searchFilter.Or(m => (m is MediaFile
                && (((MediaFile)m).PublicUrl.Contains(searchQuery)) || ((MediaFile)m).PublicUrl.Contains(searcQueryDecoded)));
        }
    }
}