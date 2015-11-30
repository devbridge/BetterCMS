// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MediaPreviewViewModel.cs" company="Devbridge Group LLC">
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

namespace BetterCms.Module.MediaManager.ViewModels.History
{
    /// <summary>
    /// Media preview model.
    /// </summary>
    public class MediaPreviewViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaPreviewViewModel" /> class.
        /// </summary>
        public MediaPreviewViewModel()
        {
            Properties = new List<MediaPreviewPropertyViewModel>();
        }

        /// <summary>
        /// Gets or sets the list of preview properties.
        /// </summary>
        /// <value>
        /// The preview items.
        /// </value>
        public List<MediaPreviewPropertyViewModel> Properties { get; set; }

        /// <summary>
        /// Adds new property view model to propertie slist.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="value">The value.</param>
        /// <param name="isUrl">if set to <c>true</c> is URL.</param>
        /// <param name="isImageUrl">if set to <c>true</c> is image URL.</param>
        public void AddProperty(string title, string value, bool isUrl = false, bool isImageUrl = false)
        {
            Properties.Add(new MediaPreviewPropertyViewModel
                               {
                                   Title = title, 
                                   Value = value,
                                   IsImageUrl = isImageUrl,
                                   IsUrl = isUrl
                               });
        }

        /// <summary>
        /// Adds new image view model to propertie slist.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="value">The value.</param>
        public void AddImage(string title, string value)
        {
            AddProperty(title, value, false, true);
        }
        
        /// <summary>
        /// Adds new URL view model to propertie slist.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="value">The value.</param>
        public void AddUrl(string title, string value)
        {
            AddProperty(title, value, true);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Properties count: {0}", Properties.Count);
        }
    }
}