using System;
using System.Linq.Expressions;

using BetterCms.Core.Models;

namespace BetterCms.Core.Api.DataContracts
{
    public abstract class GetDataRequest<TEntity> : GetFilteredDataRequest<TEntity> where TEntity : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetDataRequest{TEntity}" /> class.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="order">The order.</param>
        /// <param name="orderDescending">if set to <c>true</c> order descending.</param>
        /// <param name="itemsCount">The items count.</param>
        /// <param name="startItemNumber">The start item number.</param>
        public GetDataRequest(Expression<Func<TEntity, bool>> filter = null, 
            Expression<Func<TEntity, dynamic>> order = null,
            bool orderDescending = false,
            int? itemsCount = null,
            int startItemNumber = 1) 
            : base(filter, order, orderDescending)
        {
            StartItemNumber = startItemNumber;
            ItemsCount = itemsCount;
        }

        public int StartItemNumber { get; set; }

        public int? ItemsCount { get; set; }

        /// <summary>
        /// Sets the items count.
        /// </summary>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="pageNumber">The page number.</param>
        public void AddPaging(int itemsPerPage, int pageNumber = 1)
        {
            if (itemsPerPage <= 0)
            {
                throw new InvalidOperationException("Items per page count must be greater than zero.");
            }

            if (pageNumber <= 0)
            {
                throw new InvalidOperationException("Page number must be greater than zero.");
            }

            StartItemNumber = (pageNumber - 1) * itemsPerPage + 1;
            ItemsCount = itemsPerPage;
        }
    }
}