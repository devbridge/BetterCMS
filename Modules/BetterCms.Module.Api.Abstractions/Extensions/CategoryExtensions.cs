// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryExtensions.cs" company="Devbridge Group LLC">
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
using System.Linq;

using BetterCms.Module.Api.Operations.Root.Categories;
using BetterCms.Module.Api.Operations.Root.Categories.Category;
using BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes;
using BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes.Node;

using CategoryNodeModel = BetterCms.Module.Api.Operations.Root.Categories.Category.CategoryNodeModel;

namespace BetterCms.Module.Api.Extensions
{
    public static class CategoryExtensions
    {
        public static PutCategoryTreeRequest ToPutRequest(this GetCategoryTreeResponse response)
        {
            var model = MapPageModel(response, false);

            return new PutCategoryTreeRequest { Data = model, Id = response.Data.Id };
        }

        public static PostCategoryTreeRequest ToPostRequest(this GetCategoryTreeResponse response)
        {
            var model = MapPageModel(response, true);

            return new PostCategoryTreeRequest { Data = model };
        }

        public static PutNodeRequest ToPutRequest(this GetNodeResponse response)
        {
            var model = MapCategoryNodeModel(response, false);

            return new PutNodeRequest { Data = model, CategoryTreeId = response.Data.CategoryTreeId, Id = response.Data.Id };
        }

        public static PostCategoryNodeRequest ToPostRequest(this GetNodeResponse response)
        {
            var model = MapCategoryNodeModel(response, true);

            return new PostCategoryNodeRequest { Data = model };
        }

        private static SaveCategoryTreeModel MapPageModel(GetCategoryTreeResponse response, bool resetIds)
        {
            var model = new SaveCategoryTreeModel
            {
                Version = response.Data.Version,
                Name = response.Data.Name,
//                AccessRules = response.AccessRules,
            };

            if (response.Nodes != null)
            {
                model.Nodes = GetSubNodes(response.Nodes.Where(n => n.ParentId == null).Select(n => ToModel(n, resetIds)).ToList(), response.Nodes, resetIds);
            }

            return model;
        }

        private static SaveCategoryTreeNodeModel ToModel(CategoryNodeModel n, bool resetIds)
        {
            var model = new SaveCategoryTreeNodeModel
            {
                Id = resetIds ? default(Guid) : n.Id,
                Version = n.Version,
                Name = n.Name,
                DisplayOrder = n.DisplayOrder,
                Macro = n.Macro
            };

            return model;
        }

        private static IList<SaveCategoryTreeNodeModel> GetSubNodes(IList<SaveCategoryTreeNodeModel> nodes, IList<CategoryNodeModel> allNodes, bool resetIds)
        {
            foreach (var node in nodes)
            {
                node.Nodes = GetSubNodes(allNodes.Where(n => n.ParentId == node.Id).Select(n => ToModel(n, resetIds)).ToList(), allNodes, resetIds);
            }

            return nodes;
        }

        private static SaveNodeModel MapCategoryNodeModel(GetNodeResponse response, bool resetIds)
        {
            var model = new SaveNodeModel
            {
                Id = resetIds ? default(Guid) : response.Data.Id,
                Version = response.Data.Version,
                ParentId = response.Data.ParentId,
                Name = response.Data.Name,
                DisplayOrder = response.Data.DisplayOrder,
                Macro = response.Data.Macro,
            };

            return model;
        }
    }
}
