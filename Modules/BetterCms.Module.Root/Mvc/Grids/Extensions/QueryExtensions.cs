using System;
using System.Linq;
using System.Linq.Expressions;

using MvcContrib.Sorting;

using NHibernate;
using NHibernate.Linq;

using SortAndPagingOptions = BetterCms.Module.Root.Mvc.Grids.GridOptions.GridOptions;

namespace BetterCms.Module.Root.Mvc.Grids.Extensions
{
    public class FutureValueWrapper<T> : IFutureValue<T>
    {
        public FutureValueWrapper(T value)
        {
            Value = value;
        }

        public T Value { get; private set; }
    }

    public static class QueryExtensions
    {
        public static IFutureValue<int> ToRowCountFutureValue<TSource>(this IQueryable<TSource> source)
        {
            if (source.Provider is INhQueryProvider)
            {
                return source.ToFutureValue(f => f.Count());
            }
            return new FutureValueWrapper<int>(source.Count());

        }

        private static IFutureValue<TResult> ToFutureValue<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<IQueryable<TSource>, TResult>> selector)
            where TResult : struct
        {
                var provider = (INhQueryProvider)source.Provider;
                var method = ((MethodCallExpression)selector.Body).Method;
                var expression = Expression.Call(null, method, source.Expression);
                return (IFutureValue<TResult>)provider.ExecuteFuture(expression);
        }

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