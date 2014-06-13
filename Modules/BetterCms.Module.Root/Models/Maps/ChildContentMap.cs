using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class ChildContentMap : EntityMapBase<ChildContent>
    {
        public ChildContentMap()
            : base(RootModuleDescriptor.ModuleName)
        {
            Table("ChildContents");

            References(f => f.Parent).Column("ParentContentId").Cascade.SaveUpdate().LazyLoad().Not.Nullable();
            References(f => f.Child).Column("ChildContentId").Cascade.SaveUpdate().LazyLoad().Not.Nullable();

            HasMany(x => x.Options).Cascade.SaveUpdate().Inverse().LazyLoad().Where("IsDeleted = 0");
        }
    }
}
