using BetterCms.Module.Root.ViewModels.Cms;
using BetterCms.Module.Search.Models;

namespace BetterCms.Module.Search.ViewModels
{
    public class SearchResultsViewModel
    {
        public RenderWidgetViewModel WidgetModel { get; set; }

        public string Query { get; set; }

        public SearchResults Results { get; set; }
    }
}