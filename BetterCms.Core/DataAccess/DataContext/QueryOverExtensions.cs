using System;

using BetterCms.Core.Exceptions.DataTier;

using NHibernate;

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
    }
}
