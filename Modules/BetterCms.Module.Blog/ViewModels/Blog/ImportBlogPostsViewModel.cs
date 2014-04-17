using System.Collections.Generic;

using BetterCms.Module.Blog.Models;

namespace BetterCms.Module.Blog.ViewModels.Blog
{
    public class ImportBlogPostsViewModel
    {
        public System.Guid FileId { get; set; }

        public List<BlogPostImportResult> BlogPosts { get; set; }

        public bool CreateRedirects { get; set; }

        public override string ToString()
        {
            return string.Format(
                "{0}, UseOriginalUrls: {1}, CreateRedirects: {2}, BlogPosts.Count: {3}",
                base.ToString(),
                CreateRedirects,
                BlogPosts != null ? BlogPosts.Count : 0);
        }
    }
}