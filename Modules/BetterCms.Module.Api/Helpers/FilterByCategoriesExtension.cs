// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterByCategoriesExtension.cs" company="Devbridge Group LLC">
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

using BetterCms.Core.DataContracts;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Infrastructure.Enums;

using BetterCms.Module.Root.Services;

using BetterModules.Core.Models;

namespace BetterCms.Module.Api.Helpers
{
    public static class FilterByCategoriesExtension
    {
        public static IQueryable<TModel> ApplyCategoriesFilter<TModel>(this IQueryable<TModel> query, ICategoryService categoryService, IFilterByCategories categoriesFilter)
    where TModel : Entity, ICategorized
        {
            if (categoriesFilter != null && (categoriesFilter.FilterByCategories != null || categoriesFilter.FilterByCategoriesNames != null))
            {
                var categories = new List<Guid>();

                if (categoriesFilter.FilterByCategories != null)
                {
                    categories = categoriesFilter.FilterByCategories.Where(catId => catId != Guid.Empty).Distinct().ToList();
                }

                if (categoriesFilter.FilterByCategoriesNames != null)
                {
                    var categoriesNames = categoriesFilter.FilterByCategoriesNames.Where(name => !string.IsNullOrEmpty(name)).Distinct().ToArray();
                    var categoriesIds = categoryService.GetCategoriesIds(categoriesNames);
                    categories.AddRange(categoriesIds);
                }

                if (categories.Count > 0)
                {
                    if (categoriesFilter.FilterByCategoriesConnector == FilterConnector.And)
                    {
                        foreach (var category in categories)
                        {
                            var childCategories = categoryService.GetChildCategoriesIds(category).ToArray();
                            query = query.Where(m => m.Categories.Any(cat => childCategories.Contains(cat.Category.Id) && !cat.IsDeleted && !cat.Category.IsDeleted));
                        }
                    }
                    else
                    {
                        var allCategories = new List<Guid>();
                        foreach (var category in categories)
                        {
                            allCategories.AddRange(categoryService.GetChildCategoriesIds(category));
                        }

                        query = query.Where(page => page.Categories.Any(cat => allCategories.Contains(cat.Category.Id) && !cat.IsDeleted && !cat.Category.IsDeleted));
                    }
                }
            }
            return query;
        }
    }
}