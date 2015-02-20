using MvcContrib.Sorting;

using NHibernate;
using NHibernate.Criterion;

using SortAndPagingOptions = BetterCms.Module.Root.Mvc.Grids.GridOptions.GridOptions;

using BetterModules.Core.DataAccess.DataContext;

namespace BetterCms.Module.Root.Mvc.Grids.Extensions
{
    public static class QueryOverExtensions
    {
        public static IFutureValue<int> ToRowCountFutureValue<TRoot, TSubType>(this IQueryOver<TRoot, TSubType> query)
        {
            return query.ToRowCountQuery().FutureValue<int>();
        }

        /// <summary>
        /// Adds the sorting and paging to nHibernate query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="options">The options.</param>
        /// <returns>The query</returns>
        public static IQueryOver<TRoot, TSubType> AddSortingAndPaging<TRoot, TSubType>(this IQueryOver<TRoot, TSubType> query, SortAndPagingOptions options)
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
        public static IQueryOver<TRoot, TSubType> AddOrder<TRoot, TSubType>(this IQueryOver<TRoot, TSubType> query, SortAndPagingOptions options)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Column))
            {
                query.UnderlyingCriteria.AddOrder(new Order(options.Column, options.Direction == SortDirection.Ascending));
            }

            return query;
        }

        /// <summary>
        /// Adds the paging to nHibernate query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="options">The options.</param>
        /// <returns>The query</returns>
        public static IQueryOver<TRoot, TSubType> AddPaging<TRoot, TSubType>(this IQueryOver<TRoot, TSubType> query, SortAndPagingOptions options)
        {
            return query.AddPaging(options.PageNumber, options.PageSize);
        }
    }
}