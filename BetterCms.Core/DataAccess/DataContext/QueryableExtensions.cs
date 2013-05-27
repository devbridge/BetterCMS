using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using BetterCms.Core.Api.DataContracts;
using BetterCms.Core.Exceptions.DataTier;
using BetterCms.Core.Models;

using NHibernate;
using NHibernate.Linq;

namespace BetterCms.Core.DataAccess.DataContext
{
    public class FutureValueWrapper<T> : IFutureValue<T>
    {
        public FutureValueWrapper(T value)
        {
            Value = value;
        }

        public T Value { get; private set; }
    }

    /// <summary>
    /// Linq extensions container.
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Converts queryable to the future value.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns>
        /// Queryable converted to the future value.
        /// </returns>
        public static IFutureValue<int> ToRowCountFutureValue<TSource>(this IQueryable<TSource> source)
        {
            if (source.Provider is INhQueryProvider)
            {
                return source.ToFutureValue(f => f.Count());
            }
            return new FutureValueWrapper<int>(source.Count());

        }

        /// <summary>
        /// Converts queryable to the future value.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="selector">The selector.</param>
        /// <returns>
        /// Queryable converted to the future value.
        /// </returns>
        private static IFutureValue<TResult> ToFutureValue<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<IQueryable<TSource>, TResult>> selector)
            where TResult : struct
        {
            var provider = (INhQueryProvider)source.Provider;
            var method = ((MethodCallExpression)selector.Body).Method;
            var expression = Expression.Call(null, method, source.Expression);
            return (IFutureValue<TResult>)provider.ExecuteFuture(expression);
        }

        /// <summary>
        /// Return the first one entity from list, or execption, if list is null.
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
        /// Return the first one entity from list, or execption, if list is null.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns>First item.</returns>
        /// <exception cref="EntityNotFoundException">If no items found.</exception>
        public static TSource FirstOne<TSource>(this IEnumerable<TSource> source)
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
        /// <returns>Query with filters and sorting applied.</returns>
        public static IQueryable<TEntity> ApplyFilters<TEntity>(this IQueryable<TEntity> query, Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, dynamic>> order = null, bool orderDescending = false, int? pageNumber = null, int? itemsPerPage = null)
        {
            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = query.ApplyOrder(order, orderDescending);
            query = query.ApplyPaging(pageNumber, itemsPerPage);

            return query;
        }

        /// <summary>
        /// Returns query with filters and sorting applied.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="request">The request.</param>
        /// <returns>
        /// Query with filters and sorting applied.
        /// </returns>
        public static IQueryable<TEntity> ApplyFilters<TEntity>(this IQueryable<TEntity> query, GetDataRequest<TEntity> request)
            where TEntity : Entity
        {
            if (request != null)
            {
                query = query.ApplyFilters((GetFilteredDataRequest<TEntity>)request);
                query = query.ApplyItemsCount(request.StartItemNumber, request.ItemsCount);
            }

            return query;
        }
        
        /// <summary>
        /// Returns query with filters and sorting applied.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="request">The request.</param>
        /// <returns>
        /// Query with filters and sorting applied.
        /// </returns>
        public static IQueryable<TEntity> ApplyFilters<TEntity>(this IQueryable<TEntity> query, GetFilteredDataRequest<TEntity> request)
            where TEntity : Entity
        {
            if (request != null)
            {
                if (request.Filter != null)
                {
                    query = query.Where(request.Filter);
                }

                query = query.ApplyOrder(request.Order, request.OrderDescending);
            }

            return query;
        }

        /// <summary>
        /// Returns query with filters and sorting applied.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="hasInnerCollections">if set to <c>true</c> entity will be loadded with inner collections.</param>
        /// <param name="request">The request.</param>
        /// <returns>
        /// Query with filters and sorting applied.
        /// </returns>
        public static IQueryable<TEntity> ApplyFilters<TEntity>(this IQueryable<TEntity> query, bool hasInnerCollections, GetDataRequest<TEntity> request)
            where TEntity : Entity
        {
            if (request != null)
            {
                if (hasInnerCollections && request.ItemsCount > 0)
                {
                    // At first, load ids
                    var ids =
                        query.ApplyFilters(request.Filter, request.Order, request.OrderDescending).Select(entity => entity.Id).ApplyItemsCount(
                            request.StartItemNumber, request.ItemsCount).ToList();

                    // Then load entities
                    return query.Where(l => ids.Contains(l.Id)).ApplyOrder(request.Order, request.OrderDescending);
                }

                return query.ApplyFilters(request);
            }

            return query;
        }

        /// <summary>
        /// Applies the paging.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>Query with paging applied.</returns>
        public static IQueryable<TEntity> ApplyPaging<TEntity>(this IQueryable<TEntity> query, int? pageNumber = null, int? itemsPerPage = null)
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

        /// <summary>
        /// Applies the paging.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="startItemNumber">The start item number.</param>
        /// <param name="itemsCount">The items count.</param>
        /// <returns>Query with paging applied.</returns>
        public static IQueryable<TEntity> ApplyItemsCount<TEntity>(this IQueryable<TEntity> query, int startItemNumber = 1, int? itemsCount = null)
        {
            if (itemsCount > 0)
            {
                if (startItemNumber > 1)
                {
                    query = query.Skip(startItemNumber - 1);
                }
                query = query.Take(itemsCount.Value);
            }

            return query;
        }

        /// <summary>
        /// Applies the order to query.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="order">The order.</param>
        /// <param name="orderDescending">if set to <c>true</c> order by descending.</param>
        /// <returns>Ordered query</returns>
        public static IQueryable<TEntity> ApplyOrder<TEntity>(this IQueryable<TEntity> query, Expression<Func<TEntity, dynamic>> order = null, bool orderDescending = false)
        {
            if (order != null)
            {
                query = (orderDescending) ? query.OrderByDescending(order) : query.OrderBy(order);
            }

            return query;
        }
    }
}
