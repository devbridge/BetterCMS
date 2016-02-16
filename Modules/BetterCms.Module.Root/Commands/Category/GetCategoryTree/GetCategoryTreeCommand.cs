// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetCategoryTreeCommand.cs" company="Devbridge Group LLC">
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

using BetterCms.Module.Root.Accessors;
using BetterCms.Module.Root.Helpers;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Category;

using BetterModules.Core.DataAccess;
using BetterModules.Core.Web.Mvc.Commands;

using NHibernate;
using NHibernate.Linq;

namespace BetterCms.Module.Root.Commands.Category.GetCategoryTree
{
    public class GetCategoryTreeCommand : CommandBase, ICommand<Guid, CategoryTreeViewModel>
    {
        /// <summary>
        /// Gets or sets the CMS configuration.
        /// </summary>
        /// <value>
        /// The CMS configuration.
        /// </value>
        public ICmsConfiguration CmsConfiguration { get; set; }

        public IRepository repository { get; set; }

        public CategoryTreeViewModel Execute(Guid categoryTreeId)
        {
            if (categoryTreeId.HasDefaultValue())
            {
                var categorizableItems = Repository.AsQueryable<CategorizableItem>().ToList();
                return new CategoryTreeViewModel()
                {
                    ShowMacros = CmsConfiguration.EnableMacros,
                    CategorizableItems = categorizableItems
                        .Select(i => new CategorizableItemViewModel { Id = i.Id, Name = i.Name, IsSelected = true })
                        .OrderBy(i => i.Name)
                        .ToList()
                };
            }

            var categorizableItemsFuture = Repository.AsQueryable<CategorizableItem>().ToFuture();
            var selectedCategorizableItemsFuture = Repository.AsQueryable<CategoryTreeCategorizableItem>()
                .Where(i => i.CategoryTree.Id == categoryTreeId)
                .ToFuture();

            IQueryable<CategoryTree> sitemapQuery = Repository.AsQueryable<CategoryTree>()
                .Where(map => map.Id == categoryTreeId)
                .FetchMany(map => map.Categories);

            var categoryTree = sitemapQuery.Distinct().ToFuture().ToList().First();

            var selectedItems = selectedCategorizableItemsFuture.ToList();
            var model = new CategoryTreeViewModel
            {
                Id = categoryTree.Id,
                Version = categoryTree.Version,
                Title = categoryTree.Title,
                Macro = categoryTree.Macro,
                RootNodes =
                    CategoriesHelper.GetCategoryTreeNodesInHierarchy(
                        CmsConfiguration.EnableMultilanguage,
                        categoryTree.Categories.Distinct().Where(f => f.ParentCategory == null).ToList(),
                        categoryTree.Categories.Distinct().ToList(),
                        null),
                ShowMacros = CmsConfiguration.EnableMacros
//                CategorizableItems = categorizableItemsFuture.ToList()
//                    .Select(i => new CategorizableItemViewModel { Id = i.Id, Name = i.Name, IsSelected = selectedItems.Any(s => s.CategorizableItem.Id == i.Id) })
//                    .OrderBy(i => i.Name)
//                    .ToList()
            };
            model.CategorizableItems =
                categorizableItemsFuture.ToList()
                    .Select(i => new CategorizableItemViewModel
                    {
                        Id = i.Id,
                        Name = i.Name,
                        IsSelected = selectedItems.Any(s => s.CategorizableItem.Id == i.Id),
                    })
                    .OrderBy(i => i.Name)
                    .ToList();
            var countFutures = new Dictionary<string, IFutureValue<int>>();

            // Collect futures
            foreach (var categorizableItem in model.CategorizableItems)
            {
                var name = categorizableItem.Name;
                var accessor = CategoryAccessors.Accessors.First(ca => ca.Name == name);
                countFutures.Add(name, accessor.CheckIsUsed(repository, categoryTree));
            }

            // Evaluate futures
            foreach (var countFuture in countFutures)
            {
                var name = countFuture.Key;
                var categorizableItem = model.CategorizableItems.First(c => c.Name == name);
                categorizableItem.IsDisabled = categorizableItem.IsSelected && countFuture.Value.Value > 0;
            }

            return model;
        }

    }
}