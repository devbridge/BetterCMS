using System;
using System.Collections.Generic;

namespace BetterCms.Module.Api.Operations
{
    /// <summary>
    /// Represents container for data filter, order and paging information
    /// </summary>
    [Serializable]
    public class DataOptions
    {
        /// <summary>
        /// The field exceptions - fields, that are not available for filtering / sorting
        /// </summary>
        private IList<string> fieldExceptions;

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
        /// <param name="take">Items count to retrieve.</param>
        /// <param name="skip">Items count to skip.</param>
        public DataOptions(int? take, int skip = 0)
        {
            Filter = new DataFilter();
            Order = new DataOrder();

            Skip = skip;
            Take = take;
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
        public int? Skip { get; set; }

        /// <summary>
        /// Gets or sets the maximum count of returning items.
        /// </summary>
        /// <value>
        /// The maximum count of returning items.
        /// </value>
        public int? Take { get; set; }

        /// <summary>
        /// Gets the field exceptions - when there is prohibited filtering by field.
        /// </summary>
        /// <value>
        /// The field exceptions.
        /// </value>
        public IList<string> FieldExceptions
        {
            get
            {
                if (fieldExceptions == null)
                {
                    fieldExceptions = new List<string>();
                }
                return fieldExceptions;
            }
        }
    }
}