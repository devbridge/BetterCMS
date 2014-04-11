using System.IO;

namespace BetterCms.Module.Blog.ViewModels.Blog
{
    public class ImportBlogPostsViewModel
    {
        public bool UseOriginalUrls { get; set; }

        public bool CreateRedirects { get; set; }

        public Stream FileStream { get; set; }
    }
}