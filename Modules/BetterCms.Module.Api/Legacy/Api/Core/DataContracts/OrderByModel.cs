using System;
using System.Linq.Expressions;

namespace BetterCms.Core.Api.DataContracts
{
    public class OrderByModel<TModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderByModel{TModel}" /> class.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="orderByDescending">if set to <c>true</c> order by descending.</param>
        public OrderByModel(Expression<Func<TModel, dynamic>> order, bool orderByDescending = false)
        {
            if (order == null)
            {
                throw new NotImplementedException("Order expression cannot be null");
            }

            Order = order;
            OrderByDescending = orderByDescending;
        }

        /// <summary>
        /// Gets or sets the order by expression.
        /// </summary>
        /// <value>
        /// The order by expression.
        /// </value>
        public Expression<Func<TModel, dynamic>> Order { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to order by descending.
        /// </summary>
        /// <value>
        ///   <c>true</c> if order by descending; otherwise, <c>false</c>.
        /// </value>
        public bool OrderByDescending { get; set; }
    }
}
