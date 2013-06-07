using System;

using BetterCms.Module.Root.Mvc.Grids.GridOptions;

namespace BetterCms.Module.MediaManager.ViewModels.MediaManager
{
    public class MediaManagerViewModel : SearchableGridOptions
    {
        public Guid CurrentFolderId { get; set; }

        public MediaManagerViewModel()
        {
            Column = "Title";
            SetDefaultPaging();
        }
    }
}