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
                List<Guid> categories = new List<Guid>();

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