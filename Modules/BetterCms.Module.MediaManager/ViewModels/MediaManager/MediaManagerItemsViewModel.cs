using System;
using System.Collections.Generic;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.SiteSettings;

namespace BetterCms.Module.MediaManager.ViewModels.MediaManager
{
    [Serializable]
    public class MediaManagerItemsViewModel : SearchableGridViewModel<MediaViewModel>
    {
        public MediaPathViewModel Path { get; set; }

        public IEnumerable<LookupKeyValue> Tags { get; set; }

        public IEnumerable<LookupKeyValue> Categories { get; set; }

        public MediaManagerItemsViewModel(IEnumerable<MediaViewModel> items, SearchableGridOptions options, int totalCount) : 
            base(items, options, totalCount)
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
            return string.Format("{0}, Path: {1}", base.ToString(), Path);
        }
    }
}