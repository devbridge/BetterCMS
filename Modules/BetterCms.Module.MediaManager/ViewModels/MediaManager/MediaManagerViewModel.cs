using System;
using System.Collections.Generic;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;

namespace BetterCms.Module.MediaManager.ViewModels.MediaManager
{
    public class MediaManagerViewModel : SearchableGridOptions
    {
        public Guid CurrentFolderId { get; set; }
        
        public bool IncludeArchivedItems { get; set; }

        public bool SearchInHistory { get; set; }

        public List<LookupKeyValue> Tags { get; set; }

        public MediaManagerViewModel()
        {
            Column = "Title";
            SetDefaultPaging();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, CurrentFolderId: {1}, IncludeArchivedItems: {2}", base.ToString(), CurrentFolderId, IncludeArchivedItems);
        }
    }
}