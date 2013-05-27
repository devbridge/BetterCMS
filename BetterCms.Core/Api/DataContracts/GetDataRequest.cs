using System;
using System.Linq.Expressions;

using BetterCms.Core.Models;

namespace BetterCms.Core.Api.DataContracts
{
    public class GetDataRequest<TEntity> : GetFilteredDataRequest<TEntity> where TEntity: Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetDataRequest{TEntity}" /> class.
        /// </summary>
        public GetDataRequest()
        {
            StartItemNumber = 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetDataRequest{TEntity}" /> class.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="order">The order.</param>
        /// <param name="orderDescending">if set to <c>true</c> order descending.</param>
        public GetDataRequest(int pageNumber = 1,
            int? itemsPerPage = null,
            Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, dynamic>> order = null,
            bool orderDescending = false)
            : base(filter, order, orderDescending)
        {
            StartItemNumber = 1;
            ItemsCount = itemsPerPage;
            if (pageNumber > 1 && itemsPerPage > 0)
            {
                StartItemNumber = (pageNumber - 1) * itemsPerPage.Value + 1;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetDataRequest{TEntity}" /> class.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="order">The order.</param>
        /// <param name="orderDescending">if set to <c>true</c> order descending.</param>
        /// <param name="startItemNumber">The start item number.</param>
        /// <param name="itemsCount">The items count.</param>
        public GetDataRequest(Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, dynamic>> order = null,
            bool orderDescending = false,
            int startItemNumber = 1,
            int? itemsCount = null)
            : base(filter, order, orderDescending)
        {
            StartItemNumber = startItemNumber;
            ItemsCount = itemsCount;
        }

        public int StartItemNumber { get; set; }

        public int? ItemsCount { get; set; }
    }
}
