using System;
using System.Linq.Expressions;

using BetterCms.Core.Models;

namespace BetterCms.Core.Api.DataContracts
{
    public class GetFilteredDataRequest<TEntity> where TEntity : Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetFilteredDataRequest{TEntity}" /> class.
        /// </summary>
        public GetFilteredDataRequest()
        {
            OrderDescending = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetFilteredDataRequest{TEntity}" /> class.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="order">The order.</param>
        /// <param name="orderDescending">if set to <c>true</c> order descending.</param>
        public GetFilteredDataRequest(Expression<Func<TEntity, bool>> filter = null, 
            Expression<Func<TEntity, dynamic>> order = null,
            bool orderDescending = false)
        {
            Filter = filter;
            OrderDescending = orderDescending;
            Order = order;
        }

        public Expression<Func<TEntity, bool>> Filter { get; set; }

        public Expression<Func<TEntity, dynamic>> Order { get; set; }

        public bool OrderDescending { get; set; }

        /// <summary>
        /// Sets the default order.
        /// </summary>
        /// <param name="order">The order.</param>
        public void SetDefaultOrder(Expression<Func<TEntity, dynamic>> order)
        {
            if (Order == null)
            {
                Order = order;
            }
        }
    }
}