// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TemplateViewModel.cs" company="Devbridge Group LLC">
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

namespace BetterCms.Module.Pages.ViewModels.Page
{
    /// <summary>
    /// Template view model.
    /// </summary>
    public class TemplateViewModel
    {
        /// <summary>
        /// Gets or sets the template id.
        /// </summary>
        /// <value>
        /// The template id.
        /// </value>
        public Guid TemplateId { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }
        
        /// <summary>
        /// Gets or sets the template preview image url.
        /// </summary>
        /// <value>
        /// The template preview image url.
        /// </value>
        public string PreviewUrl { get; set; }

        /// <summary>
        /// Gets or sets the preview thumbnail URL.
        /// </summary>
        /// <value>
        /// The preview thumbnail URL.
        /// </value>
        public string PreviewThumbnailUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether template is master page.
        /// </summary>
        /// <value>
        /// <c>true</c> if template is master page; otherwise, <c>false</c>.
        /// </value>
        public bool IsMasterPage { get; set; }

        /// <summary>
        /// Gets or sets the URL hash.
        /// </summary>
        /// <value>
        /// The URL hash.
        /// </value>
        public string MasterUrlHash { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether master page has circular references to current.
        /// </summary>
        /// <value>
        /// <c>true</c> if master page has circular references to current; otherwise, <c>false</c>.
        /// </value>
        public bool IsCircularToCurrent { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Id: {0}, Name: {1}, IsActive: {2}", TemplateId, Title, IsActive);
        }
    }
}