using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using BetterCms.Core.Exceptions.DataTier;

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
        /// Converts query to the future value.
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
        /// Return the first one entity from list, or exception, if list is null.
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
        /// Return the first one entity from list, or exception, if list is null.
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
        /// Adds the paging.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="startItemNumber">The start item number.</param>
        /// <param name="itemsCount">The items count.</param>
        /// <returns>Query with paging applied.</returns>
        public static IQueryable<TEntity> AddPaging<TEntity>(this IQueryable<TEntity> query, int startItemNumber = 1, int? itemsCount = null)
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
        public static IQueryable<TEntity> AddOrder<TEntity>(this IQueryable<TEntity> query, Expression<Func<TEntity, dynamic>> order = null, bool orderDescending = false)
        {
            if (order != null)
            {
                query = (orderDescending) ? query.OrderByDescending(order) : query.OrderBy(order);
            }

            return query;
        }
    }
}
