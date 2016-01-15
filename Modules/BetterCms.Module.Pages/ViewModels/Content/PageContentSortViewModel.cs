// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageContentSortViewModel.cs" company="Devbridge Group LLC">
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

namespace BetterCms.Module.Pages.ViewModels.Content
{
    using System;

    /// <summary>
    /// Edit page content options view model.
    /// </summary>
    public class PageContentSortViewModel
    {
        /// <summary>
        /// Gets or sets page id.
        /// </summary>
        /// <value>
        /// The page id.
        /// </value>
        public Guid PageId { get; set; }

        /// <summary>
        /// Gets or sets the list of page content ids.
        /// </summary>
        /// <value>
        /// The list of page content ids and versions.
        /// </value>
        public IList<ContentSortViewModel> PageContents { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("PageId: {0}", PageId);
        }
    }
}