using System.Collections.Generic;

using BetterCms.Module.Blog.Models;

namespace BetterCms.Module.Blog.Commands.ImportBlogPosts
{
    public class ImportBlogPostsResponse
    {
        public IList<BlogPostImportResult> Results { get; set; }
    }
}