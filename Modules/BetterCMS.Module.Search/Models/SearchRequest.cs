namespace BetterCms.Module.Search.Models
{
    public class SearchRequest
    {
        public string Query { get; set; }
        
        public int Skip { get; set; }
        
        public int Take { get; set; }

        public SearchRequest(string query = null, int take = SearchModuleConstants.DefaultSearchResultsCount, int skip = 0)
        {
            Query = query;
            Skip = skip;
            Take = take;
        }
    }
}