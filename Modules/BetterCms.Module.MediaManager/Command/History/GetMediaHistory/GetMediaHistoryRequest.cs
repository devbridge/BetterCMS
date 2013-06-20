using System;

using BetterCms.Module.Root.Mvc.Grids.GridOptions;

namespace BetterCms.Module.MediaManager.Command.History.GetMediaHistory
{
    public class GetMediaHistoryRequest : SearchableGridOptions
    {
        public Guid MediaId { get; set; }
    }
}