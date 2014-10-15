using System.Collections.Generic;

using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.SiteSettings;

namespace BetterCms.Module.Pages.ViewModels.Setting
{
    public class ConfigurationSettingsListViewModel : SearchableGridViewModel<ConfigurationSettingItemViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationSettingsListViewModel" /> class.
        /// </summary>
        /// <param name="items">The models.</param>
        /// <param name="options">The options.</param>
        /// <param name="totalCount">The total count.</param>
        public ConfigurationSettingsListViewModel(IEnumerable<ConfigurationSettingItemViewModel> items, SearchableGridOptions options, int totalCount)
            : base(items, options, totalCount)
        {
        }
    }
}