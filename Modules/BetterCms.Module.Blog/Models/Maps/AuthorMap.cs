using BetterModules.Core.Models;

namespace BetterCms.Module.Blog.Models.Maps
{
    public class AuthorMap : EntityMapBase<Author>
    {
        public AuthorMap()
            : base(BlogModuleDescriptor.ModuleName)
        {
            Table("Authors");

            Map(x => x.Name).Not.Nullable().Length(MaxLength.Name);
            Map(x => x.Description).Nullable().Length(MaxLength.Max);

            References(x => x.Image).Cascade.SaveUpdate().LazyLoad().Nullable();
        }
    }
}
