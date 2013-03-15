using System;
using System.Linq.Expressions;

using BetterCms.Core.Exceptions.DataTier;
using BetterCms.Core.Models;

using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace BetterCms.Core.DataAccess.DataContext
{
    /// <summary>
    /// NHibernate extensions container.
    /// </summary>
    public static class QueryOverExtensions
    {
        /// <summary>
        /// Firsts the specified query.
        /// </summary>
        /// <typeparam name="TReturn">The type of the return.</typeparam>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <param name="query">The query.</param>
        /// <returns>First item.</returns>
        /// <exception cref="EntityNotFoundException">If no items found.</exception>
        public static TReturn First<TReturn, TInput>(this IQueryOver<TInput> query)
        {
            var viewModel = query.SingleOrDefault<TReturn>();

            if (viewModel == null)
            {
                throw new EntityNotFoundException(typeof(TReturn), Guid.Empty);
            }

            return viewModel;
        }


        /// <summary>
        /// Adds the paging to nHibernate query.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root.</typeparam>
        /// <typeparam name="TSubType">The type of the sub type.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>The query</returns>
        public static IQueryOver<TRoot, TSubType> AddPaging<TRoot, TSubType>(this IQueryOver<TRoot, TSubType> query, int? pageNumber, int? itemsPerPage)
        {
            if (itemsPerPage > 0)
            {
                if (pageNumber > 1)
                {
                    query.UnderlyingCriteria.SetFirstResult(itemsPerPage.Value * (pageNumber.Value - 1));
                }

                query.UnderlyingCriteria.SetMaxResults(itemsPerPage.Value);
            }

            return query;
        }

        /// <summary>
        /// Returns query with filters and sorting applied.
        /// </summary>
        /// <typeparam name="TRoot">The type of the entity.</typeparam>
        /// <typeparam name="TSubType">The type of the sub type.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="order">The order.</param>
        /// <param name="orderDescending">if set to <c>true</c> order descending.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>Query with filters and sorting applied</returns>
        public static IQueryOver<TRoot, TSubType> ApplyFilters<TRoot, TSubType>(this IQueryOver<TRoot, TSubType> query, Expression<Func<TSubType, bool>> filter = null, Expression<Func<TSubType, dynamic>> order = null, bool orderDescending = false, int? pageNumber = null, int? itemsPerPage = null)
        {
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (order != null)
            {
                query = (orderDescending) ? query.OrderBy(order).Desc : query.OrderBy(order).Asc;
            }

            query = query.AddPaging(pageNumber, itemsPerPage);

            return query;
        }

        /// <summary>
        /// Returns query with sub-query with filters and sorting applied.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root.</typeparam>
        /// <typeparam name="TSubType">The type of the sub type.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="order">The order.</param>
        /// <param name="orderDescending">if set to <c>true</c> order by descending.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>Query with sub-query with filters and sorting applied</returns>
        public static IQueryOver<TRoot, TSubType> ApplySubQueryFilters<TRoot, TSubType>(this IQueryOver<TRoot, TSubType> query, Expression<Func<TSubType, bool>> filter = null, Expression<Func<TSubType, dynamic>> order = null, bool orderDescending = false, int? pageNumber = null, int? itemsPerPage = null)
            where TRoot : Entity
            where TSubType : Entity
        {
            // If added paging, adding subQuery
            if (itemsPerPage > 0)
            {
                var idSubQuery = (QueryOver<TRoot, TSubType>)QueryOver.Of<TSubType>()
                        .ApplyFilters(filter, order, orderDescending, pageNumber, itemsPerPage)
                        .Select(Projections.Property<TSubType>(l => l.Id));

                return query
                    .WithSubquery
                    .WhereProperty(m => m.Id)
                    .In(idSubQuery)
                    .TransformUsing(Transformers.DistinctRootEntity);
            }

            // If no paging, subquery is not required
            return query
                .ApplyFilters(filter, order, orderDescending, pageNumber, itemsPerPage)
                .TransformUsing(Transformers.DistinctRootEntity);
        }
    }
}
