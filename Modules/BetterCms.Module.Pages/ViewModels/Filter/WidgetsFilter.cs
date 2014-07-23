using System;

using BetterCms.Module.Root.Mvc.Grids.GridOptions;

namespace BetterCms.Module.Pages.ViewModels.Filter
{
    public class WidgetsFilter : SearchableGridOptions
    {
        public Guid? ChildContentId { get; set; }
    }
}