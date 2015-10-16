using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

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

        /// <summary>
        /// Order by.
        /// </summary>
        /// <typeparam name="TEntity">The type of the root.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="column">The column.</param>
        /// <param name="direction">The direction.</param>
        /// <returns>Ordered query</returns>
        public static IQueryable<TEntity> ThenBy<TEntity>(this IQueryable<TEntity> query, string column, SortDirection direction)
        {
            if (string.IsNullOrEmpty(column)) return query;
            Type type = typeof(TEntity);
            PropertyInfo property = type.GetProperty(column);
            if (property == null) throw new InvalidOperationException(string.Format("Could not find a property called '{0}' on type {1}", (object)column, (object)type));
            ParameterExpression parameterExpression = Expression.Parameter(type, "p");
            LambdaExpression lambdaExpression = Expression.Lambda(
                (Expression)Expression.MakeMemberAccess((Expression)parameterExpression, (MemberInfo)property), new ParameterExpression[1] { parameterExpression });
            MethodCallExpression methodCallExpression = Expression.Call(
                typeof(Queryable),
                direction == SortDirection.Ascending ? "ThenBy" : "ThenByDescending",
                new Type[2] { type, property.PropertyType },
                query.Expression,
                (Expression)Expression.Quote((Expression)lambdaExpression));
            return query.Provider.CreateQuery<TEntity>((Expression)methodCallExpression);
        }
    }
}