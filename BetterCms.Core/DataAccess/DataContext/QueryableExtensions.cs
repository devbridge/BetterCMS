using System;
using System.Linq;

using BetterCms.Core.Exceptions.DataTier;

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
    }
}
