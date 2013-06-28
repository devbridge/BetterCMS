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

        public List<LookupKeyValue> Tags { get; set; }

        public MediaManagerViewModel()
        {
            Column = "Title";
            SetDefaultPaging();
        }
    }
}