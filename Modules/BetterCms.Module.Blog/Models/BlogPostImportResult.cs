namespace BetterCms.Module.Blog.Models
{
    public class BlogPostImportResult
    {
        public bool Success { get; set; }

        public string Id { get; set; }

        public string Title { get; set; }
        
        public string PageUrl { get; set; }

        public string ErrorMessage { get; set; }
        
        public string WarnMessage { get; set; }
    }
}