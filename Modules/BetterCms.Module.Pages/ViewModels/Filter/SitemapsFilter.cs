using System.Collections.Generic;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;

namespace BetterCms.Module.Pages.ViewModels.Filter
{
    public class SitemapsFilter : SearchableGridOptions
    {
        public List<LookupKeyValue> Tags { get; set; }
    }
}