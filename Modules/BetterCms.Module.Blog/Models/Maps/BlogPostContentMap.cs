using BetterModules.Core.Models;

namespace BetterCms.Module.Blog.Models.Maps
{
    public class BlogPostContentMap : EntitySubClassMapBase<BlogPostContent>
    {
        public BlogPostContentMap()
            : base(BlogModuleDescriptor.ModuleName)
        {
            Table("BlogPostContents");
        }
    }
}