using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Models;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Infrastructure.Enums;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Services;

namespace BetterCms.Module.Api.Helpers
{
    public static class FilterByCategoriesExtension
    {
        public static IQueryable<TModel> ApplyCategoriesFilter<TModel>(this IQueryable<TModel> query, ICategoryService categoryService, IFilterByCategories categoriesFilter)
    where TModel : Entity, ICategorized
        {
            if (categoriesFilter != null && categoriesFilter.FilterByCategories != null)
            {
                var categories = categoriesFilter.FilterByCategories.Where(catId => catId != Guid.Empty).Distinct().ToArray();

                if (categories.Length > 0)
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