using BetterCms.Module.Root.Mvc.Grids.GridOptions;

namespace BetterCms.Module.Root.Services.Categories.Tree
{
    public class GetCategoryTreeRequest : SearchableGridOptions
    {
        public bool WithPaging { get; set; }

        public bool WithOrder { get; set; }
    }
}