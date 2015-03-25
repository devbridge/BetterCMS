using System.Collections.Generic;

using BetterCms.Module.Root.Mvc.Grids;
using BetterCms.Module.Root.ViewModels.SiteSettings;

namespace BetterCms.Module.Root.ViewModels.Category
{
    public class CategoryTreesGridViewModel<TModel> : SearchableGridViewModel<TModel> where TModel : IEditableGridItem
    {
        public CategoryTreesGridViewModel(IEnumerable<TModel> items, CategoryTreesFilter filter, int totalCount)
            : base(items, filter, totalCount)
        {
        }
    }
}