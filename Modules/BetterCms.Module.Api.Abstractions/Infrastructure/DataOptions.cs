using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Infrastructure
{
    /// <summary>
    /// Represents container for data filter, order and paging information
    /// </summary>
    [DataContract]
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
        [DataMember]
        public DataFilter Filter { get; set; }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        [DataMember]
        public DataOrder Order { get; set; }

        /// <summary>
        /// Gets or sets the starting item number.
        /// </summary>
        /// <value>
        /// The starting item number.
        /// </value>
        [DataMember]
        public int? Skip { get; set; }

        /// <summary>
        /// Gets or sets the maximum count of returning items.
        /// </summary>
        /// <value>
        /// The maximum count of returning items.
        /// </value>
        [DataMember]
        public int? Take { get; set; }
    }
}