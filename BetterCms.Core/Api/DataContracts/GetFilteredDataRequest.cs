using System;
using System.Linq.Expressions;

namespace BetterCms.Core.Api.DataContracts
{
    public abstract class GetFilteredDataRequest<TEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetFilteredDataRequest{TEntity}" /> class.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="order">The order.</param>
        /// <param name="orderDescending">if set to <c>true</c> order by descending.</param>
        public GetFilteredDataRequest(Expression<Func<TEntity, bool>> filter = null, 
            Expression<Func<TEntity, dynamic>> order = null,
            bool orderDescending = false)
        {
            Filter = filter;
            OrderDescending = orderDescending;
            Order = order;
        }

        /// <summary>
        /// Gets or sets the filter.
        /// </summary>
        /// <value>
        /// The filter.
        /// </value>
        public Expression<Func<TEntity, bool>> Filter { get; set; }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public Expression<Func<TEntity, dynamic>> Order { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether order by descending.
        /// </summary>
        /// <value>
        ///   <c>true</c> if order by descending; otherwise, <c>false</c>.
        /// </value>
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
