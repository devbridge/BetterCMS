// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INodesService.cs" company="Devbridge Group LLC">
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
namespace BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes
{
    /// <summary>
    /// Service contract for category nodes.
    /// </summary>
    public interface INodesService
    {
        /// <summary>
        /// Gets the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetCategoryNodesResponse</c> with nodes list.</returns>
        GetCategoryNodesResponse Get(GetCategoryNodesRequest request);


        // NOTE: do not implement: replaces all the category nodes.
        // PutCategoryNodesResponse Put(PutCategoryNodesRequest request);

        /// <summary>
        /// Creates a new category node.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PostCategoryNodeResponse</c> with a new category node id.</returns>
        PostCategoryNodeResponse Post(PostCategoryNodeRequest request);

        // NOTE: do not implement: drops all the category nodes.
        // DeleteCategoryNodesResponse Delete(DeleteCategoryNodesRequest request);
    }
}