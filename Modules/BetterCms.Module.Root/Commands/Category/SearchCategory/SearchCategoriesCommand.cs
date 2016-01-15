// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchCategoriesCommand.cs" company="Devbridge Group LLC">
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
using System.Collections.Generic;
using System.Linq;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Category;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Root.Commands.Category.SearchCategory
{
    /// <summary>
    /// A command to get tag list by filter.
    /// </summary>
    public class SearchCategoriesCommand : CommandBase, ICommand<CategorySuggestionViewModel, List<LookupKeyValue>>
    {
        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// A list of tags.
        /// </returns>
        public List<LookupKeyValue> Execute(CategorySuggestionViewModel model)
        {
            // TODO: #1210
            // return Categories(model);
            var query = Repository.AsQueryable<Models.Category>()
                .Where(category => category.Name.Contains(model.Query));

            if (model.ExistingItemsArray.Length > 0)
            {
                query = query.Where(category => !model.ExistingItems.Contains(category.Id.ToString().ToUpper()));
            }

            if (!string.IsNullOrEmpty(model.CategoryTreeForKey))
            {
                query = query.Where(c => !c.CategoryTree.IsDeleted && c.CategoryTree.AvailableFor.Any(e => e.CategorizableItem.Name == model.CategoryTreeForKey));
            }

            return query.OrderBy(category => category.Name)
                .Select(category => new LookupKeyValue { Key = category.Id.ToString(), Value = category.Name })
                .ToList();
        }


        private List<LookupKeyValue> Categories(CategorySuggestionViewModel model)
        {
            var query = Repository.AsQueryable<Models.Category>().Where(c => !c.CategoryTree.IsDeleted && !c.IsDeleted);
            if (!string.IsNullOrEmpty(model.CategoryTreeForKey))
            {
                query = query.Where(c => c.CategoryTree.AvailableFor.Any(e => e.CategorizableItem.Name == model.CategoryTreeForKey));
            }
            var allCategories = query.ToList();
            var treeLikePlainList = new List<LookupKeyValue>();
            ConstructTreeLike(
                treeLikePlainList,
                string.Empty,
                allCategories.Where(c => c.ParentCategory == null).OrderBy(c => c.DisplayOrder).ToList(),
                allCategories,
                model.Query);
            return treeLikePlainList;
        }

        private void ConstructTreeLike(List<LookupKeyValue> list, string prefix, List<Models.Category> categoriesToAdd, List<Models.Category> allCategories, string query)
        {
            foreach (var category in categoriesToAdd)
            {
                if (category.Name.ToLowerInvariant().Contains(query.ToLowerInvariant()))
                {
                    list.Add(new LookupKeyValue { Key = category.Id.ToString(), Value = string.Format("{0}{1}", prefix, category.Name) });
                }
                ConstructTreeLike(
                    list,
                    string.Format("{0}{1} => ", prefix, category.Name),
                    allCategories.Where(c => c.ParentCategory != null && c.ParentCategory.Id == category.Id).OrderBy(c => c.DisplayOrder).ToList(),
                    allCategories,
                    query);
            }
        }
    }
}