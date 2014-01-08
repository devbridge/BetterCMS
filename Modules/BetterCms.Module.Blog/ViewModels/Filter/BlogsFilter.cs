using System;
using System.Collections.Generic;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;

namespace BetterCms.Module.Blog.ViewModels.Filter
{
    public class BlogsFilter : SearchableGridOptions
    {
        public List<LookupKeyValue> Tags { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? LanguageId { get; set; }
        public bool IncludeArchived { get; set; }
    }
}