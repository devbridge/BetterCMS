using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class PageAccessMap : EntityMapBase<PageAccess>
    {
        public PageAccessMap()
            : base(RootModuleDescriptor.ModuleName)
        {
            Table("PageAccess");

            References(x => x.Page).Cascade.SaveUpdate().Not.Nullable().LazyLoad();

            Map(x => x.RoleOrUser).Length(MaxLength.Name).Not.Nullable();
            Map(x => x.AccessLevel).Not.Nullable();
        }
    }
}