using System;

namespace BetterCms.Module.Api.Infrastructure
{
    public interface IFilterByCategories
    {
        /// <summary>
        /// Gets or sets the categories.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        System.Collections.Generic.List<Guid> FilterByCategories { get; }

        /// <summary>
        /// Gets or sets the category names.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        System.Collections.Generic.List<string> FilterByCategoriesNames { get; }

        /// <summary>
        /// Gets or sets the categories filter connector.
        /// </summary>
        /// <value>
        /// The tags filter connector.
        /// </value>
        Enums.FilterConnector FilterByCategoriesConnector { get; }
    }
}