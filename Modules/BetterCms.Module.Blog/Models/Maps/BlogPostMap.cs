using BetterModules.Core.Models;

namespace BetterCms.Module.Blog.Models.Maps
{
    public class BlogPostMap : EntitySubClassMapBase<BlogPost>
    {
        public BlogPostMap()
            : base(BlogModuleDescriptor.ModuleName)
        {
            Table("BlogPosts");

            Map(x => x.ActivationDate).Not.Nullable();
            Map(x => x.ExpirationDate).Nullable();

            References(x => x.Author).Cascade.SaveUpdate().LazyLoad();
        }
    }
}