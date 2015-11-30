// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAuthorService.cs" company="Devbridge Group LLC">
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
namespace BetterCms.Module.Api.Operations.Blog.Authors.Author
{
    /// <summary>
    /// Contract for author service.
    /// </summary>
    public interface IAuthorService
    {
        /// <summary>
        /// Gets the specified author.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetAuthorRequest</c> with an author.</returns>
        GetAuthorResponse Get(GetAuthorRequest request);

        /// <summary>
        /// Replaces the author or if it doesn't exist, creates it.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PutAuthorResponse</c> with a author id.</returns>
        PutAuthorResponse Put(PutAuthorRequest request);

        // NOTE: do not implement: should treat the addressed member as a collection in its own right and create a new entry in it.
        // PostAuthorResponse Post(PostAuthorRequest request);

        /// <summary>
        /// Deletes the specified author.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>DeleteAuthorResponse</c> with success status.</returns>
        DeleteAuthorResponse Delete(DeleteAuthorRequest request);
    }
}