using System;

namespace BetterCms.Module.Blog.Models
{
    public class Blog
    {
        public virtual string Name { get; set; }

        public virtual DateTime? PublishDate { get; set; }

        public virtual string Description { get; set; }

        public virtual string Text { get; set; }

        //public virtual User Publisher { get; set; }
    }
}