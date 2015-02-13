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

namespace BetterCms.Module.Api.Helpers
{
    public static class FilterByCategoriesExtension
    {
        public static IQueryable<TModel> ApplyCategoriesFilter<TModel>(this IQueryable<TModel> query, IFilterByCategories categoriesFilter)
    where TModel : Entity, ICategorized
        {
            if (categoriesFilter != null && categoriesFilter.FilterByCategories != null)
            {
                var tags = categoriesFilter.FilterByCategories.Where(catId => catId != Guid.Empty).Distinct().ToArray();

                if (tags.Length > 0)
                {
                    if (categoriesFilter.FilterByCategoriesConnector == FilterConnector.And)
                    {
                        query = query.Where(page => page.Categories.Count(cat => tags.Contains(cat.Category.Id) && !cat.IsDeleted && !cat.Category.IsDeleted) == tags.Length);
                    }
                    else
                    {
                        query = query.Where(page => page.Categories.Any(cat => tags.Contains(cat.Category.Id) && !cat.IsDeleted && !cat.Category.IsDeleted));
                    }
                }
            }
            return query;
        }
    }
}