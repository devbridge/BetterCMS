using System;
using System.Linq;
using System.Linq.Expressions;

using BetterCms.Core.Api.DataContracts;
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
        /// <param name="request">The request.</param>
        /// <returns> IQueryable entity </returns>
        public static IQueryable<TEntity> AsQueryable<TEntity>(this IRepository repository, GetDataRequest<TEntity> request) where TEntity : Entity
        {
            return repository.AsQueryable<TEntity>().ApplyFilters(request);
        }
    }
}
