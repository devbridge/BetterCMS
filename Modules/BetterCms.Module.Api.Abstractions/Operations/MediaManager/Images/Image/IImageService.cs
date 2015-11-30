// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IImageService.cs" company="Devbridge Group LLC">
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
namespace BetterCms.Module.Api.Operations.MediaManager.Images.Image
{
    /// <summary>
    /// Image service contract for REST.
    /// </summary>
    public interface IImageService
    {
        /// <summary>
        /// Gets the specified image.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetImageRequest</c> with an image.</returns>
        GetImageResponse Get(GetImageRequest request);

        /// <summary>
        /// Replaces the image or if it doesn't exist, creates it.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PutImageResponse</c> with a image id.</returns>
        PutImageResponse Put(PutImageRequest request);

        // NOTE: do not implement: should treat the addressed member as a collection in its own right and create a new entry in it.
        // PostImageResponse Post(PostImageRequest request);

        /// <summary>
        /// Deletes the specified image.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>DeleteImageResponse</c> with success status.</returns>
        DeleteImageResponse Delete(DeleteImageRequest request);
    }
}