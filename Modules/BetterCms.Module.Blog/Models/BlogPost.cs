using System;

using BetterCms.Module.Pages.Models;

namespace BetterCms.Module.Blog.Models
{
    [Serializable]
    public class BlogPost : PageProperties
    {
        public virtual Author Author { get; set; }

        public override PageProperties Duplicate()
        {
            return CopyDataToDuplicate(new BlogPost());
        }

        protected override PageProperties CopyDataToDuplicate(PageProperties duplicate)
        {
            var blogPost = (BlogPost)base.CopyDataToDuplicate(duplicate);

            blogPost.Author = Author;

            return blogPost;
        }
    }
}