using System;

namespace BetterCms.Module.Api.Operations
{
    /// <summary>
    /// Represents container for data filter, order and paging information
    /// </summary>
    [Serializable]
    public class DataOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataOptions" /> class.
        /// NOTE: required for JavaScript serialization
        /// </summary>
        public DataOptions()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataOptions" /> class.
        /// </summary>
        /// <param name="itemsCount">The maximum count of returning items.</param>
        /// <param name="startItemNumber">The starting item number.</param>
        public DataOptions(int? itemsCount,
            int startItemNumber = 1)
        {
            Filter = new DataFilter();
            Order = new DataOrder();

            Skip = startItemNumber;
            Take = itemsCount;
        }

        /// <summary>
        /// Gets or sets the filter.
        /// </summary>
        /// <value>
        /// The filter.
        /// </value>
        public DataFilter Filter { get; set; }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public DataOrder Order { get; set; }

        /// <summary>
        /// Gets or sets the starting item number.
        /// </summary>
        /// <value>
        /// The starting item number.
        /// </value>
        public int Skip { get; set; }

        /// <summary>
        /// Gets or sets the maximum count of returning items.
        /// </summary>
        /// <value>
        /// The maximum count of returning items.
        /// </value>
        public int? Take { get; set; }
    }
}