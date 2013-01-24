using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.SiteSettings;

namespace BetterCms.Module.Users.ViewModels.Role
{
    public class SiteSettingRoleListViewModel : SearchableGridViewModel<RoleItemViewModel>
    {
         /// <summary>
        /// Initializes a new instance of the <see cref="SiteSettingRoleListViewModel" /> class.
        /// </summary>
        /// <param name="items">The models.</param>
        /// <param name="options">The options.</param>
        /// <param name="totalCount">The total count.</param>
        public SiteSettingRoleListViewModel(IEnumerable<RoleItemViewModel> items, SearchableGridOptions options, int totalCount)
            : base(items, options, totalCount)
        {
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("GridOptions : {0}, SearchQuery: {1}", GridOptions, SearchQuery);
        }
    }
}