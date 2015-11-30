// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MediaPreviewPropertyViewModel.cs" company="Devbridge Group LLC">
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
namespace BetterCms.Module.MediaManager.ViewModels.History
{
    /// <summary>
    /// View model for rendering media version properties
    /// </summary>
    public class MediaPreviewPropertyViewModel
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether property is URL.
        /// </summary>
        /// <value>
        ///   <c>true</c> if property is URL; otherwise, <c>false</c>.
        /// </value>
        public bool IsUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether property is image URL.
        /// </summary>
        /// <value>
        ///   <c>true</c> if property is image URL; otherwise, <c>false</c>.
        /// </value>
        public bool IsImageUrl { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, Title: {1}, Value: {2}, IsUrl: {3}, IsImageUrl: {4}", Title, Value, IsUrl, IsImageUrl);
        }
    }
}