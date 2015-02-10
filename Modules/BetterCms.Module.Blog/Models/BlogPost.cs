using System;

using BetterCms.Core.DataContracts;
using BetterCms.Module.Pages.Models;

namespace BetterCms.Module.Blog.Models
{
    [Serializable]
    public class BlogPost : PageProperties, ICategorized
    {
        public const string CategorizableItemKeyForBlogs = "Blog Posts";

        public virtual Author Author { get; set; }

        public virtual DateTime ActivationDate { get; set; }

        public virtual DateTime? ExpirationDate { get; set; }

        public override string GetCategorizableItemKey()
        {
            return CategorizableItemKeyForBlogs;
        }

        public override PageProperties Duplicate()
        {
            return CopyDataToDuplicate(new BlogPost());
        }

        protected override PageProperties CopyDataToDuplicate(PageProperties duplicate)
        {
            var blogPost = (BlogPost)base.CopyDataToDuplicate(duplicate);

            blogPost.Author = Author;
            blogPost.ActivationDate = ActivationDate;
            blogPost.ExpirationDate = ExpirationDate;

            return blogPost;
        }
    }
}