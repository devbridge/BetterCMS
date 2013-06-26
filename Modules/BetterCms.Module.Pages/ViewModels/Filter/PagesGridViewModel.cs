using System;
using System.Collections.Generic;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc.Grids;
using BetterCms.Module.Root.ViewModels.SiteSettings;

namespace BetterCms.Module.Pages.ViewModels.Filter
{
    public class PagesGridViewModel<TModel> : SearchableGridViewModel<TModel> where TModel : IEditableGridItem
    {
        public IEnumerable<LookupKeyValue> Tags { get; set; }
        public Guid? CategoryId { get; set; }
        public IEnumerable<LookupKeyValue> Categories { get; set; }
        public bool IncludeArchived { get; set; }

        public PagesGridViewModel(IEnumerable<TModel> items, PagesFilter filter, int totalCount, IEnumerable<LookupKeyValue> categories) : base(items, filter, totalCount)
        {
            Tags = filter.Tags;
            CategoryId = filter.CategoryId;
            Categories = categories;
            IncludeArchived = filter.IncludeArchived;
        }
    }
}