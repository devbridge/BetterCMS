namespace BetterCms.Module.Blog.Api.DataContracts.Filters
{
    public class BlogPostFilter
    {
        public string Like { get; set; }

        public string[] Tags { get; set; }
    }
}