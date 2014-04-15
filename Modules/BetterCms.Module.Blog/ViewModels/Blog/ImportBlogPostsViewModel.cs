using System.IO;

namespace BetterCms.Module.Blog.ViewModels.Blog
{
    public class ImportBlogPostsViewModel
    {
        public bool UseOriginalUrls { get; set; }

        public bool CreateRedirects { get; set; }

        public Stream FileStream { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, UseOriginalUrls: {1}, CreateRedirects: {2}", base.ToString(), UseOriginalUrls, CreateRedirects);
        }
    }
}