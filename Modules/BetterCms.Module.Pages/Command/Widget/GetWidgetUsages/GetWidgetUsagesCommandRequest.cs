using System;

using BetterCms.Module.Root.Mvc.Grids.GridOptions;

namespace BetterCms.Module.Pages.Command.Widget.GetWidgetUsages
{
    public class GetWidgetUsagesCommandRequest
    {
        public Guid WidgetId { get; set; }
        
        public SearchableGridOptions Options { get; set; }
    }
}