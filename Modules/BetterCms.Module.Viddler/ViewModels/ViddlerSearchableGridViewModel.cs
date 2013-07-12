using System.Collections.Generic;

using BetterCms.Module.Root.Mvc.Grids;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.SiteSettings;

namespace BetterCms.Module.Viddler.ViewModels
{
    public class ViddlerSearchableGridViewModel<TModel> : SearchableGridViewModel<TModel> where TModel : IEditableGridItem
    {
        public ViddlerSearchableGridViewModel(IEnumerable<TModel> items, SearchableGridOptions options, int totalCount) :
            base(items, options, totalCount > 1980 ? 1980 : totalCount)
        {
        }
    }
}