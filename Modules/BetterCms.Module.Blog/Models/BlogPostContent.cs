using System;

using BetterCms.Module.Pages.Models;

namespace BetterCms.Module.Blog.Models
{
    [Serializable]
    public class BlogPostContent : HtmlContent
    {
        public override Root.Models.Content CopyDataTo(Root.Models.Content content)
        {
            var copy = (BlogPostContent)base.CopyDataTo(content);
            
            return copy;
        }

        public override Root.Models.Content Clone()
        {
            return CopyDataTo(new BlogPostContent());
        }
    }
}