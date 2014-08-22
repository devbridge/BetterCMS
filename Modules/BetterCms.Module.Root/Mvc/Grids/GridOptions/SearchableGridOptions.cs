using System;

namespace BetterCms.Module.Root.Mvc.Grids.GridOptions
{
    [Serializable]
    public class SearchableGridOptions : GridOptions
    {
        /// <summary>
        /// Gets or sets the search query.
        /// </summary>
        /// <value>
        /// The search query.
        /// </value>
        public string SearchQuery { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, SearchQuery: {1}", base.ToString(), SearchQuery);
        }

        /// <summary>
        /// Populates the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        public override void CopyFrom(GridOptions options)
        {
            var searchableGridOptions = options as SearchableGridOptions;
            if (searchableGridOptions != null)
            {
                SearchQuery = searchableGridOptions.SearchQuery;
            }

            base.CopyFrom(options);
        }
    }
}