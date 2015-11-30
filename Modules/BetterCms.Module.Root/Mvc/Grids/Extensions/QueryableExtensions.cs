// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryableExtensions.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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