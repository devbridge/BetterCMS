using System;

using BetterCms.Module.Root.Mvc.Grids.GridOptions;

namespace BetterCms.Module.Pages.Command.History.GetContentHistory
{
    public class GetContentHistoryRequest : SearchableGridOptions
    {
        public Guid ContentId { get; set; }
    }
}