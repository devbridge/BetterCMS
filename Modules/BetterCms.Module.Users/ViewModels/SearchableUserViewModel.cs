using System.Collections.Generic;

namespace BetterCms.Module.Users.ViewModels
{
    public class SearchableUserViewModel
    {
        /// <summary>
        /// Gets or sets the search query.
        /// </summary>
        /// <value>
        /// The search query.
        /// </value>
        public string SearchQuery { get; set; }

        /// <summary>
        /// Gets or sets the sitemap root nodes.
        /// </summary>
        /// <value>
        /// The root nodes.
        /// </value>
        public IList<UserViewModel> Users { get; set; }
    }
}