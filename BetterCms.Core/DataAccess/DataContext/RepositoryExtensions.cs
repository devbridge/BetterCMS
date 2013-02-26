using System;
using System.Linq;
using System.Linq.Expressions;

using BetterCms.Core.Models;

namespace BetterCms.Core.DataAccess.DataContext
{
    /// <summary>
    /// Repository extensions
    /// </summary>
    public static class RepositoryExtensions
    {
        /// <summary>
        /// Returns query with filters and sorting applied.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="repository">The repository.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="order">The order.</param>
        /// <param name="orderDescending">if set to <c>true</c> order descending.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns></returns>
        public static IQueryable<TEntity> AsQueryable<TEntity>(this IRepository repository, Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, dynamic>> order = null, bool orderDescending = false, int? pageNumber = null, int? itemsPerPage = null) where TEntity : Entity
        {
            return repository.AsQueryable<TEntity>().ApplyFilters(filter, order, orderDescending, pageNumber, itemsPerPage);
        }
    }
}
