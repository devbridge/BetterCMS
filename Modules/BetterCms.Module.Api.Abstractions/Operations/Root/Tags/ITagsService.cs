// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITagsService.cs" company="Devbridge Group LLC">
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
namespace BetterCms.Module.Api.Operations.Root.Tags
{
    /// <summary>
    /// Tags service contract for REST.
    /// </summary>
    public interface ITagsService
    {
        /// <summary>
        /// Gets the tags list.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetTagsResponse</c> with tags list.</returns>
        GetTagsResponse Get(GetTagsRequest request);

        // NOTE: do not implement: replaces all the tags.
        // PutTagsResponse Put(PutTagsRequest request);

        /// <summary>
        /// Creates a new tag.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PostTagsResponse</c> with a new tag id.</returns>
        PostTagResponse Post(PostTagRequest request);

        // NOTE: do not implement: drops all the tags.
        // DeleteTagsResponse Delete(DeleteTagsRequest request);
    }
}