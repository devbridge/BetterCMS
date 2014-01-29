using System.ComponentModel.DataAnnotations;

using BetterCms.Module.Root.ViewModels.Cms;
using BetterCms.Module.Search.Content.Resources;

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

        [StringLength(SearchModuleConstants.SearchQueryMaximumLength, MinimumLength = 1, ErrorMessageResourceType = typeof(SearchGlobalization), ErrorMessageResourceName = "SearchForm_Query_MaximumLengthMessage")]
        public string Query { get; set; }

        public RenderWidgetViewModel WidgetModel { get; set; }
    }
}