using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure.Enums;

namespace BetterCms.Module.Api.Infrastructure
{
    /// <summary>
    /// Represents class for ordering items
    /// </summary>
    [DataContract]
    [Serializable]
    public class OrderItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderItem" /> class.
        /// NOTE: required for JavaScript serialization
        /// </summary>
        public OrderItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderItem" /> class.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="direction">The order direction.</param>
        public OrderItem(string field, OrderDirection direction = OrderDirection.Asc)
        {
            Direction = direction;
            Field = field;
        }

        /// <summary>
        /// Gets or sets the ordering field.
        /// </summary>
        /// <value>
        /// The ordering field.
        /// </value>
        [DataMember]
        public string Field { get; set; }

        /// <summary>
        /// Gets or sets the order direction.
        /// </summary>
        /// <value>
        /// The order direction.
        /// </value>
        [DataMember]
        public OrderDirection Direction { get; set; }

        /// <summary>
        /// Gets a value indicating whether query must be ordered by descending.
        /// </summary>
        /// <value>
        ///   <c>true</c> if query must be ordered by descending; otherwise, <c>false</c>.
        /// </value>
        public bool OrderByDescending
        {
            get
            {
                return Direction == OrderDirection.Desc;
            }
        }
    }
}