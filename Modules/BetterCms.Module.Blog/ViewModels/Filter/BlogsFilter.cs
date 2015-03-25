using System;
using System.Collections.Generic;

using BetterCms.Module.Pages.ViewModels.Filter;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;

namespace BetterCms.Module.Blog.ViewModels.Filter
{
    public class BlogsFilter : SearchableGridOptions
    {
        public List<LookupKeyValue> Tags { get; set; }
        public List<LookupKeyValue> Categories { get; set; }
        public Guid? LanguageId { get; set; }
        public bool IncludeArchived { get; set; }
        public PageStatusFilterType? Status { get; set; }
        public SeoStatusFilterType? SeoStatus { get; set; }
    }
}