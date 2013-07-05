using System;
using System.Linq;
using System.Linq.Expressions;

using BetterCms.Core.Api.DataContracts;
using BetterCms.Module.Api.Operations;
using BetterCms.Module.Api.Operations.Enums;

namespace BetterCms.Module.Api.Helpers
{
    /// <summary>
    /// DataOptions class extensions
    /// </summary>
    public static class DataOptionsExtensions
    {
        /// <summary>
        /// Applies the filter, the order and the paging to get request.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="options">The filtering / sorting options.</param>
        /// <param name="request">The request.</param>
        public static void ApplyTo<TModel>(this DataOptions options, GetDataRequest<TModel> request)
        {
            var creator = new DataOptionsQueryCreator<TModel>(options);

            options.ApplyFilter(request, creator);
            options.ApplyOrder(request, creator);
            options.ApplyPaging(request);
        }

        /// <summary>
        /// Applies the filter to get request.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="options">The filtering / sorting options.</param>
        /// <param name="request">The request.</param>
        /// <param name="creator">The query creator.</param>
        public static void ApplyFilter<TModel>(this DataOptions options, GetDataRequest<TModel> request, DataOptionsQueryCreator<TModel> creator = null)
        {
            if (options != null
                && options.Filter != null
                && ((options.Filter.Where != null && options.Filter.Where.Count > 0)
                    || (options.Filter.Inner != null && options.Filter.Inner.Count > 0)))
            {
                if (creator == null)
                {
                    creator = new DataOptionsQueryCreator<TModel>(options);
                }

                var query = creator.GetFilterQuery();
                var parameters = creator.GetFilterParameters();
                var filter = DynamicExpression.ParseLambda<TModel, bool>(query, parameters);

                request.Filter = filter;
            }
        }

        /// <summary>
        /// Applies the order to get request.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="options">The filtering / sorting options.</param>
        /// <param name="request">The request.</param>
        /// <param name="creator">The query creator.</param>
        public static void ApplyOrder<TModel>(this DataOptions options, GetDataRequest<TModel> request, DataOptionsQueryCreator<TModel> creator = null)
        {
            if (options != null && options.Order != null && options.Order.By != null && options.Order.By.Count > 0)
            {
                if (creator == null)
                {
                    creator = new DataOptionsQueryCreator<TModel>(options);
                }

                var query = creator.GetOrderQuery();

                var parameters = new[] { Expression.Parameter(typeof(TModel), "") };
                ExpressionParser parser = new ExpressionParser(parameters, query, new object[0]);
                var orderings = parser.ParseOrdering();
                foreach (var order in orderings)
                {
                    var expression = order.Selector;
                    var conversion = Expression.Convert(expression, typeof(object));

                    var lambda = Expression.Lambda<Func<TModel, object>>(conversion, parameters);
                    request.AddOrder(lambda, !order.Ascending);
                }
            }
        }

        /// <summary>
        /// Applies the paging to get request.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="options">The filtering / sorting options.</param>
        /// <param name="request">The request.</param>
        public static void ApplyPaging<TModel>(this DataOptions options, GetDataRequest<TModel> request)
        {
            if (options != null && options.Take > 0)
            {
                if (options.Skip > 0)
                {
                    request.StartItemNumber = options.Skip.Value;
                }
                request.ItemsCount = options.Take;
            }
        }

        /// <summary>
        /// Sets the default order.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="column">The column.</param>
        /// <param name="direction">The direction.</param>
        public static void SetDefaultOrder(this DataOptions options, string column, OrderDirection direction = OrderDirection.Asc)
        {
            if (!options.Order.By.Any())
            {
                options.Order.Add(column, direction);
            }
        }

        /// <summary>
        /// Determines whether the specified options has paging.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>
        ///   <c>true</c> if the specified options has paging; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasPaging(this DataOptions options)
        {
            return options != null && options.Take > 0;
        }
    }
}