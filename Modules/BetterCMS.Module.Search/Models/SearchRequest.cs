namespace BetterCms.Module.Search.Models
{
    public class SearchRequest
    {
        public string Query { get; set; }

        public SearchRequest()
        {
        }        

        public SearchRequest(string query)
        {
            Query = query;
        }
    }
}