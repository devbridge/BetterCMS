using System;
using System.Collections.Generic;

using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.SiteSettings;

namespace BetterCms.Module.MediaManager.ViewModels.MediaManager
{
    [Serializable]
    public class MediaManagerItemsViewModel : SearchableGridViewModel<MediaViewModel>
    {
        public MediaPathViewModel Path { get; set; }

        public MediaManagerItemsViewModel(IEnumerable<MediaViewModel> items, SearchableGridOptions options, int totalCount) : 
            base(items, options, totalCount)
        {
        }
    }
}