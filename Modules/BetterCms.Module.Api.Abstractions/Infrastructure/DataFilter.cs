using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure.Enums;

namespace BetterCms.Module.Api.Infrastructure
{
    /// <summary>
    /// Represents container for filtering items list
    /// </summary>
    [DataContract]
    [Serializable]
    public class DataFilter
    {
        /// <summary>
        /// Gets or sets the filter connector.
        /// </summary>
        /// <value>
        /// The filter connector.
        /// </value>
        [DataMember]
        public FilterConnector Connector { get; set; }

        /// <summary>
        /// Gets or sets the list filter items.
        /// </summary>
        /// <value>
        /// The list of filter items.
        /// </value>
        [DataMember]
        public List<FilterItem> Where { get; set; }

        /// <summary>
        /// Gets or sets the list of inner filters.
        /// </summary>
        /// <value>
        /// The list of inner filters.
        /// </value>
        [DataMember]
        public List<DataFilter> Inner { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataFilter" /> class.
        /// NOTE: required for JavaScript serialization
        /// </summary>
        public DataFilter()
            : this(FilterConnector.And)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataFilter" /> class.
        /// </summary>
        /// <param name="connector">The filter connector.</param>
        public DataFilter(FilterConnector connector)
        {
            Where = new List<FilterItem>();
            Inner = new List<DataFilter>();
            Connector = connector;
        }

        /// <summary>
        /// Adds the specified filtering field.
        /// </summary>
        /// <param name="field">The filtering field.</param>
        /// <param name="value">The filtering value.</param>
        /// <param name="operation">The filtering operation.</param>
        public void Add(string field, object value, FilterOperation operation = FilterOperation.Equal)
        {
            var filterItem = new FilterItem(field, value, operation);

            Where.Add(filterItem);
        }
    }
}