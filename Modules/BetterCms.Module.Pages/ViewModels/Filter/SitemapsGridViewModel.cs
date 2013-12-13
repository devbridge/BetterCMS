using System.Collections.Generic;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc.Grids;
using BetterCms.Module.Root.ViewModels.SiteSettings;

namespace BetterCms.Module.Pages.ViewModels.Filter
{
    public class SitemapsGridViewModel<TModel> : SearchableGridViewModel<TModel> where TModel : IEditableGridItem
    {
        public IEnumerable<LookupKeyValue> Tags { get; set; }

        public SitemapsGridViewModel(IEnumerable<TModel> items, SitemapsFilter filter, int totalCount)
            : base(items, filter, totalCount)
        {
            Tags = filter.Tags;
        }
    }
}