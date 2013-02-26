using System;
using System.Linq;
using System.Linq.Expressions;

using BetterCms.Core.Exceptions.DataTier;
using BetterCms.Core.Models;

namespace BetterCms.Core.DataAccess.DataContext
{
    /// <summary>
    /// Linq extensions container.
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Firsts the one.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns>First item.</returns>
        /// <exception cref="EntityNotFoundException">If no items found.</exception>
        public static TSource FirstOne<TSource>(this IQueryable<TSource> source)
        {
            var model = source.FirstOrDefault();

            if (model == null)
            {
                throw new EntityNotFoundException(typeof(TSource), Guid.Empty);
            }

            return model;
        }

        /// <summary>
        /// Returns query with filters and sorting applied.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="order">The order.</param>
        /// <param name="orderDescending">if set to <c>true</c> order descending.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns></returns>
        public static IQueryable<TEntity> ApplyFilters<TEntity>(this IQueryable<TEntity> query, Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, dynamic>> order = null, bool orderDescending = false, int? pageNumber = null, int? itemsPerPage = null) where TEntity : Entity
        {
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (order != null)
            {
                query = (orderDescending) ? query.OrderByDescending(order) : query.OrderBy(order);
            }
            
            query = query.ApplyPaging(pageNumber, itemsPerPage);

            return query;
        }

        /// <summary>
        /// Applies the paging.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns></returns>
        public static IQueryable<TEntity> ApplyPaging<TEntity>(this IQueryable<TEntity> query, int? pageNumber = null, int? itemsPerPage = null) where TEntity : Entity
        {
            if (itemsPerPage > 0)
            {
                if (pageNumber > 1)
                {
                    query = query.Skip((pageNumber.Value - 1) * itemsPerPage.Value);
                }
                query = query.Take(itemsPerPage.Value);
            }

            return query;
        }
    }
}
