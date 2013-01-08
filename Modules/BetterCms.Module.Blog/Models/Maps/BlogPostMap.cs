using BetterCms.Core.Models;

namespace BetterCms.Module.Blog.Models.Maps
{
    public class BlogPostMap : EntitySubClassMapBase<BlogPost>
    {
        public BlogPostMap()
            : base(BlogModuleDescriptor.ModuleName)
        {
            Table("BlogPosts");
        }
    }
}