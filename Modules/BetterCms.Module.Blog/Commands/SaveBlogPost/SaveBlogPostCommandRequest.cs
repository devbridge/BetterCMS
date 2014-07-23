using System.Collections.Generic;

using BetterCms.Module.Blog.ViewModels.Blog;
using BetterCms.Module.Root.ViewModels.Option;

namespace BetterCms.Module.Blog.Commands.SaveBlogPost
{
    public class SaveBlogPostCommandRequest
    {
        public BlogPostViewModel Content { get; set; }

        public IList<ContentOptionValuesViewModel> ChildContentOptionValues { get; set; }
    }
}