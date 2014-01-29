using BetterCms.Module.Root.ViewModels.Cms;
using BetterCms.Module.Search.Models;

namespace BetterCms.Module.Search.ViewModels
{
    public class SearchResultsViewModel
    {
        public SearchResults Results { get; set; }

        public RenderWidgetViewModel WidgetViewModel { get; set; }

        public string ErrorMessage { get; set; }
    }
}