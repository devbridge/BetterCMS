// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MediaHistoryViewModel.cs" company="Devbridge Group LLC">
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

using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.SiteSettings;

namespace BetterCms.Module.MediaManager.ViewModels.History
{
    /// <summary>
    /// Media history view model.
    /// </summary>
    public class MediaHistoryViewModel : SearchableGridViewModel<MediaHistoryItem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaHistoryViewModel"/> class.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="options">The options.</param>
        /// <param name="totalCount">The total count.</param>
        /// <param name="mediaId">The media id.</param>
        public MediaHistoryViewModel(IEnumerable<MediaHistoryItem> items, SearchableGridOptions options, int totalCount, Guid mediaId)
            : base(items, options, totalCount)
        {
            MediaId = mediaId;
        }

        /// <summary>
        /// Gets or sets the media id.
        /// </summary>
        /// <value>
        /// The media id.
        /// </value>
        public Guid MediaId { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("ContentId: {0}", MediaId);
        }
    }
}