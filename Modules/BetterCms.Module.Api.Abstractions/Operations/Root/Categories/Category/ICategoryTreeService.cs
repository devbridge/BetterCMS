// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICategoryTreeService.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes;
using BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes.Node;
using BetterCms.Module.Api.Operations.Root.Categories.Category.Tree;
using BetterCms.Module.Api.Operations.Root.CategorizableItems;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category
{
    /// <summary>
    /// Service contract for category operations.
    /// </summary>
    public interface ICategoryTreeService
    {
        /// <summary>
        /// Gets the tree.
        /// </summary>
        /// <value>
        /// The tree.
        /// </value>
        INodesTreeService Tree { get; }

        /// <summary>
        /// Gets the nodes.
        /// </summary>
        /// <value>
        /// The nodes.
        /// </value>
        INodesService Nodes { get; }

        /// <summary>
        /// Gets the node.
        /// </summary>
        /// <value>
        /// The node.
        /// </value>
        INodeService Node { get; }

        /// <summary>
        /// Gets the categorizable items.
        /// </summary>
        /// <value>
        /// The categorizable items.
        /// </value>
        ICategorizableItemsService CategorizableItems { get; }

        /// <summary>
        /// Gets the category specified in request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetCategoriesResponse</c> with category data.</returns>
        GetCategoryTreeResponse Get(GetCategoryTreeRequest request);

        /// <summary>
        /// Puts the category specified in request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PutCategoriesResponse</c> with updated category data.</returns>
        PutCategoryTreeResponse Put(PutCategoryTreeRequest request);

        /// <summary>
        /// Deletes the category specified in request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>DeleteCategoriesResponse</c> with success status.</returns>
        DeleteCategoryTreeResponse Delete(DeleteCategoryTreeRequest request);
    }
}