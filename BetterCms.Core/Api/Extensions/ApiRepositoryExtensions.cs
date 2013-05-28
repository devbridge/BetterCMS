using BetterCms.Core.Api.DataContracts;
using BetterCms.Core.DataAccess;
using BetterCms.Core.Models;

namespace BetterCms.Core.Api.Extensions
{
    /// <summary>
    /// API Repository extensions
    /// </summary>
    public static class ApiRepositoryExtensions
    {
        /// <summary>
        /// Returns data list response with filters, sorting and paging applied.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="repository">The repository.</param>
        /// <param name="request">The request.</param>
        /// <returns>Sorted, filtered and paged data list response</returns>
        public static DataListResponse<TEntity> ToDataListResponse<TEntity>(this IRepository repository, GetDataRequest<TEntity> request) where TEntity : Entity
        {
            var query = repository
                .AsQueryable<TEntity>()
                .ApplyFilters(request);

            var totalCount = query.ToRowCountFutureValue(request);

            query = query.AddOrderAndPaging(request);

            return query.ToDataListResponse(totalCount);
        }
    }
}
