using System.IO;

namespace BetterCms.Module.Blog.Commands.ImportBlogPosts
{
    public class ImportBlogPostsRequest
    {
        public bool UseOriginalUrls { get; set; }
        
        public bool CreateRedirects { get; set; }

        public Stream FileStream { get; set; }
    }
}