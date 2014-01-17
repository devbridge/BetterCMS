namespace BetterCms.Module.Search.Models
{
    public class SearchResultItem
    {
        public string Title { get; set; }

        public string Link { get; set; }

        public string FormattedUrl { get; set; }

        public string Snippet { get; set; }

        public bool IsDenied { get; set; }
    }
}