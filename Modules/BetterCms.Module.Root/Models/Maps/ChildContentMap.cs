using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class ChildContentMap : EntityMapBase<ChildContent>
    {
        public ChildContentMap()
            : base(RootModuleDescriptor.ModuleName)
        {
            Table("ChildContents");

            Map(x => x.AssignmentIdentifier).Not.Nullable();

            References(f => f.Parent).Column("ParentContentId").Cascade.SaveUpdate().LazyLoad().Not.Nullable();
            References(f => f.Child).Column("ChildContentId").Cascade.SaveUpdate().LazyLoad().Not.Nullable();

            HasMany(x => x.Options).Inverse().Cascade.SaveUpdate().LazyLoad().Where("IsDeleted = 0");
        }
    }
}
