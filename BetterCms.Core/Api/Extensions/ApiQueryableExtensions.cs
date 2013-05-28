using System;
using System.Linq;
using System.Linq.Expressions;

using BetterCms.Core.Api.DataContracts;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Models;

using NHibernate;

namespace BetterCms.Core.Api.Extensions
{
    /// <summary>
    /// API Linq extensions container.
    /// </summary>
    public static class ApiQueryableExtensions
    {
        /// <summary>
        /// To the row count future value.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public static IFutureValue<int> ToRowCountFutureValue<TEntity>(this IQueryable<TEntity> query, GetDataRequest<TEntity> request)
            where TEntity : Entity
        {
            var hasPaging = (request == null || request.ItemsCount > 0);
            if (hasPaging)
            {
                return query.ToRowCountFutureValue();
            }
            return null;
        }

        /// <summary>
        /// Returns query with filter applied.
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
                query = query.ApplyFilters(request.Filter);
            }

            return query;
        }

        /// <summary>
        /// Returns query with filter applied.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="request">The request.</param>
        /// <returns>
        /// Query with filters and sorting applied.
        /// </returns>
        public static Tuple<IQueryable<TEntity>, IFutureValue<int>> ApplyFiltersWithChildren<TEntity>(this IQueryable<TEntity> query, GetDataRequest<TEntity> request)
            where TEntity : Entity
        {
            if (request != null)
            {
                // If paging is needed
                if (request.ItemsCount > 0)
                {
                    // At first, load ids
                    query = query
                        .ApplyFilters(request.Filter);

                    var futureValue = query.ToRowCountFutureValue(request);

                    var ids = query
                        .AddOrderAndPaging(request)
                        .Select(entity => entity.Id)
                        .ToList();

                    // Then load entities
                    query = query.Where(l => ids.Contains(l.Id));

                    return new Tuple<IQueryable<TEntity>, IFutureValue<int>>(query, futureValue);
                }

                return new Tuple<IQueryable<TEntity>, IFutureValue<int>>(query.ApplyFilters(request), null);
            }

            return new Tuple<IQueryable<TEntity>, IFutureValue<int>>(query, null);
        }

        /// <summary>
        /// Applies the order to query.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="request">The request.</param>
        /// <returns> Ordered query </returns>
        public static IQueryable<TEntity> AddOrder<TEntity>(this IQueryable<TEntity> query, GetFilteredDataRequest<TEntity> request)
            where TEntity : Entity
        {
            if (request != null)
            {
                query = query.AddOrder(request.Order, request.OrderDescending);
            }

            return query;
        }

        /// <summary>
        /// Adds the order and paging.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="request">The request.</param>
        /// <returns> Ordered and paged query </returns>
        public static IQueryable<TEntity> AddOrderAndPaging<TEntity>(this IQueryable<TEntity> query, GetDataRequest<TEntity> request)
            where TEntity : Entity
        {
            if (request != null)
            {
                query = query.AddOrder(request);
                query = query.AddPaging(request.StartItemNumber, request.ItemsCount);
            }

            return query;
        }

        /// <summary>
        /// To the data list response.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="futureValue">The future value.</param>
        /// <returns></returns>
        public static DataListResponse<TEntity> ToDataListResponse<TEntity>(this IQueryable<TEntity> query, IFutureValue<int> futureValue = null)
            where TEntity : Entity
        {
            var items = query.ToList();
            var totalCount = futureValue != null ? futureValue.Value : items.Count;

            return new DataListResponse<TEntity>(items, totalCount);
        }

        /// <summary>
        /// Returns query with filter applied.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>
        /// Query with filters and sorting applied.
        /// </returns>
        private static IQueryable<TEntity> ApplyFilters<TEntity>(this IQueryable<TEntity> query, Expression<Func<TEntity, bool>> filter)
            where TEntity : Entity
        {
            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query;
        }
    }
}
