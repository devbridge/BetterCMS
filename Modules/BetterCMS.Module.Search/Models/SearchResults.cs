using System.Collections.Generic;

namespace BetterCms.Module.Search.Models
{
    public class SearchResults
    {
        public SearchInformation SearchInformation { get; set; }

        public string Query { get; set; }

        public List<SearchResultItem> Items { get; set; }

        public SearchResults()
        {
            Items = new List<SearchResultItem>();
            SearchInformation = new SearchInformation();
        }
    }
}