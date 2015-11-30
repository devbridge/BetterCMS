// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RenderPageImageViewModel.cs" company="Devbridge Group LLC">
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
namespace BetterCms.Module.Pages.ViewModels.Page
{
    public class RenderPageImageViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderPageImageViewModel" /> class.
        /// </summary>
        /// <param name="image">The image.</param>
        public RenderPageImageViewModel(MediaManager.Models.MediaImage image = null)
        {
            if (image != null)
            {
                Id = image.Id;
                Version = image.Version;
                Title = image.Title;
                Caption = image.Caption;
                PublicUrl = image.PublicUrl;
                PublicThumbnailUrl = image.PublicThumbnailUrl;
                Height = image.Height;
                Width = image.Width;
            }
        }

        /// <summary>
        /// Gets or sets the image id.
        /// </summary>
        /// <value>
        /// The image id.
        /// </value>
        public System.Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the image version.
        /// </summary>
        /// <value>
        /// The image version.
        /// </value>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the image title.
        /// </summary>
        /// <value>
        /// The image title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the image  caption.
        /// </summary>
        /// <value>
        /// The image caption.
        /// </value>
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets the image size.
        /// </summary>
        /// <value>
        /// The image size.
        /// </value>
        public string Size { get; set; }

        /// <summary>
        /// Gets or sets the image public URL.
        /// </summary>
        /// <value>
        /// The image  public URL.
        /// </value>
        public string PublicUrl { get; set; }

        /// <summary>
        /// Gets or sets the image thumbnail public URL.
        /// </summary>
        /// <value>
        /// The image thumbnail public URL.
        /// </value>
        public string PublicThumbnailUrl { get; set; }

        /// <summary>
        /// Gets or sets the image width.
        /// </summary>
        /// <value>
        /// The image width.
        /// </value>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the image height.
        /// </summary>
        /// <value>
        /// The image height.
        /// </value>
        public int Height { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, Id: {1}, Version: {2}, Title: {3}, Caption: {4}", base.ToString(), Id, Version, Title, Caption);
        }
    }
}