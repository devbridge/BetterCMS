// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AlbumViewModel.cs" company="Devbridge Group LLC">
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

namespace BetterCms.Module.ImagesGallery.ViewModels
{
    public class AlbumViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AlbumViewModel" /> class.
        /// </summary>
        public AlbumViewModel()
        {
            Images = new List<ImageViewModel>();
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the images count.
        /// </summary>
        /// <value>
        /// The images count.
        /// </value>
        public int ImagesCount { get; set; }

        /// <summary>
        /// Gets or sets the last update date.
        /// </summary>
        /// <value>
        /// The last update date.
        /// </value>
        public DateTime? LastUpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the cover image URL.
        /// </summary>
        /// <value>
        /// The cover image URL.
        /// </value>
        public string CoverImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the list of image view models.
        /// </summary>
        /// <value>
        /// The list of image view models.
        /// </value>
        public List<ImageViewModel> Images { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to load CMS styles.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to load CMS styles; otherwise, <c>false</c>.
        /// </value>
        public bool LoadCmsStyles { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether header should be rendered.
        /// </summary>
        /// <value>
        ///   <c>true</c> if header should be rendered; otherwise, <c>false</c>.
        /// </value>
        public bool RenderHeader { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to render back URL.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to render back URL; otherwise, <c>false</c>.
        /// </value>
        public bool RenderBackUrl { get; set; }

        /// <summary>
        /// Gets or sets the count of images per section.
        /// </summary>
        /// <value>
        /// The count of images per section.
        /// </value>
        public int ImagesPerSection { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Title: {0}, Url: {1}, ImagesCount: {2}", Title, Url, ImagesCount);
        }
    }
}