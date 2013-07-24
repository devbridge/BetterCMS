using System;
using System.Linq;
using System.Linq.Expressions;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Infrastructure.Enums;
using BetterCms.Module.Api.Operations;

using ServiceStack.OrmLite;

namespace BetterCms.Module.Api.Helpers
{
    public static class FilterByTagExtensions
    {
        public static IQueryable<TModel> ApplyTagsFilter<TModel>(this IQueryable<TModel> query, IFilterByTags tagsFilter, 
            Func<string, Expression<Func<TModel, bool>>> filterByTagName)
        {
            if (tagsFilter.FilterByTags != null && tagsFilter.FilterByTags.Any(tag => !string.IsNullOrWhiteSpace(tag)))
            {
                var predicate = (tagsFilter.FilterByTagsConnector == FilterConnector.Or)
                    ? PredicateBuilder.False<TModel>()
                    : PredicateBuilder.True<TModel>();

                foreach (var tagName in tagsFilter.FilterByTags)
                {
                    if (!string.IsNullOrWhiteSpace(tagName))
                    {
                        Expression<Func<TModel, bool>> whereClause = filterByTagName(tagName);
                        if (tagsFilter.FilterByTagsConnector == FilterConnector.Or)
                        {
                            predicate = predicate.Or(whereClause);
                        }
                        else
                        {
                            predicate = predicate.And(whereClause);
                        }
                    }
                }

                query = query.Where(predicate);
            }

            return query;
        }
    }
}