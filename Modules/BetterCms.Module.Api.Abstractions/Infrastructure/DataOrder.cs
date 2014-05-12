using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure.Enums;

namespace BetterCms.Module.Api.Infrastructure
{
    /// <summary>
    /// Represents container for ordering items list
    /// </summary>
    [DataContract]
    [Serializable]
    public class DataOrder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataOrder" /> class.
        /// </summary>
        public DataOrder()
        {
            By = new List<OrderItem>();
        }

        /// <summary>
        /// Gets or sets the list of order items.
        /// </summary>
        /// <value>
        /// The list of order items.
        /// </value>
        [DataMember]
        public List<OrderItem> By { get; set; }

        /// <summary>
        /// Adds the order item to orderings list.
        /// </summary>
        /// <param name="field">The ordering field.</param>
        /// <param name="direction">The order direction.</param>
        public void Add(string field, OrderDirection direction = OrderDirection.Asc)
        {
            var filterItem = new OrderItem(field, direction);

            By.Add(filterItem);
        }
    }
}