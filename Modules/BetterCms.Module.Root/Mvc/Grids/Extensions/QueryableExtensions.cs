using System.Linq;

using MvcContrib.Sorting;

using SortAndPagingOptions = BetterCms.Module.Root.Mvc.Grids.GridOptions.GridOptions;

namespace BetterCms.Module.Root.Mvc.Grids.Extensions
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// Adds the sorting and paging to nHibernate query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="options">The options.</param>
        /// <returns>The query</returns>
        public static IQueryable<TRoot> AddSortingAndPaging<TRoot>(this IQueryable<TRoot> query, SortAndPagingOptions options)
        {
            if (options != null)
            {
                return query.AddOrder(options).AddPaging(options);
            }

            return query;
        }

        /// <summary>
        /// Adds the sorting to nHibernate query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="options">The options.</param>
        /// <returns>The query</returns>
        public static IQueryable<TRoot> AddOrder<TRoot>(this IQueryable<TRoot> query, SortAndPagingOptions options)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Column))
            {
                return query.OrderBy(options.Column, options.Direction);
            }

            return query;
        }

        /// <summary>
        /// Adds the paging to nHibernate query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="options">The options.</param>
        /// <returns>The query</returns>
        public static IQueryable<TRoot> AddPaging<TRoot>(this IQueryable<TRoot> query, SortAndPagingOptions options)
        {
            if (options != null)
            {
                if (options.PageNumber > 0)
                {
                    query = query.Skip(options.PageSize * (options.PageNumber - 1));
                }

                if (options.PageSize > 0)
                {
                    query = query.Take(options.PageSize);
                }
            }

            return query;
        }
    }
}