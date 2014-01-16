using BetterCms.Module.Root.ViewModels.Cms;

namespace BetterCms.Module.Search.ViewModels
{
    public class SearchRequestViewModel
    {
        public SearchRequestViewModel()
        {
            Skip = 0;
            Take = SearchModuleConstants.DefaultSearchResultsCount;
        }

        public int Skip { get; set; }
        
        public int Take { get; set; }

        public string Query { get; set; }

        public RenderWidgetViewModel WidgetModel { get; set; }
    }
}