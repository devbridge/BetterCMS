// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFolderService.cs" company="Devbridge Group LLC">
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
namespace BetterCms.Module.Api.Operations.MediaManager.Folders.Folder
{
    /// <summary>
    /// Folder service contract for REST.
    /// </summary>
    public interface IFolderService
    {
        /// <summary>
        /// Gets the specified folder.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetFolderRequest</c> with an folder.</returns>
        GetFolderResponse Get(GetFolderRequest request);

        /// <summary>
        /// Replaces the folder or if it doesn't exist, creates it.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PutFolderResponse</c> with a folder id.</returns>
        PutFolderResponse Put(PutFolderRequest request);

        // NOTE: do not implement: should treat the addressed member as a collection in its own right and create a new entry in it.
        // PostFolderResponse Post(PostFolderRequest request);

        /// <summary>
        /// Deletes the specified folder.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>DeleteFolderResponse</c> with success status.</returns>
        DeleteFolderResponse Delete(DeleteFolderRequest request);
    }
}