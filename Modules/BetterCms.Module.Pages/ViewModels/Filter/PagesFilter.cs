using System;
using System.Collections.Generic;

using BetterCms.Module.Root.Mvc.Grids.GridOptions;

namespace BetterCms.Module.Pages.ViewModels.Filter
{
    public class PagesFilter : SearchableGridOptions
    {
        public IList<Guid> Tags { get; set; }
        public Guid? CategoryId { get; set; }
    }
}