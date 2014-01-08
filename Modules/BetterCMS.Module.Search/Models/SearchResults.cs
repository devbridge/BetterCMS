using System.Collections.Generic;

namespace BetterCms.Module.Search.Models
{
    public class SearchResults
    {
        public SearchResults()
        {
            Items = new List<SearchResultItem>();
        }

        public int TotalResults { get; set; }

        public string Query { get; set; }

        public IList<SearchResultItem> Items { get; set; }
    }
}